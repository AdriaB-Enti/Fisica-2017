using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twistConstraint : MonoBehaviour
{
    public bool cancelTwist;

    public Transform tester;    //per debugar


    [SerializeField]
    Transform parent;

    public enum ForwardDir { X, Y, Z };
    public ForwardDir localForward;


    Quaternion getTwist()
    {

        Quaternion localRotation = Quaternion.Inverse(parent.rotation) * transform.rotation;
        // this is the same than 
        //Quaternion localRotation = transform.localRotation;

        Quaternion twist = localRotation;

        switch (localForward)
        {
            case ForwardDir.X:
                twist.y = 0;
                twist.z = 0;
                break;

            case ForwardDir.Y:
                twist.x = 0;
                twist.z = 0;
                break;

            case ForwardDir.Z:
                twist.x = 0;
                twist.y = 0;
                break;


        }
        print(transform.up);

        twist = norm(twist);
        return twist;


    }

    Quaternion getSwing()
    {



        Quaternion localRotation = Quaternion.Inverse(parent.rotation) * transform.rotation;
        Quaternion twist = getTwist();

        Quaternion swing = localRotation * Quaternion.Inverse(twist);
        return swing;

    }
    //https://en.wikipedia.org/wiki/Vector_projection
    //https://en.wikipedia.org/wiki/Dot_product#Scalar_projection_and_first_properties
    //

    void Update() //--tornar a posar a lateUpdate?
    {
        if (cancelTwist)
        {
            //mirar si el fordward va totalment cap amunt o avall...? potser no fa falta

            Vector3 planeNormal = Vector3.Cross(transform.forward, Vector3.up);
            //COMPROVAR: donara problemes si la normal està "a l'altra banda"? - podria mirar l'angle entre el transform.up i les dues normals, i triar la que estigui més aprop? - o potser no dona problemes
            float cosNormal = Vector3.Dot(transform.up, planeNormal) / (transform.up.magnitude * planeNormal.magnitude); //cosinus (per veure l'angle) entre la normal del pla i la Y local
            if (cosNormal < 0) //si es més petit que 0 vol dir que esta a més de 90º i a menys de 270
            {
                planeNormal *= -1;  //crec que no fa res.....
            }


            //Projeccció a la normal del pla (Vector3.right=normal pla)
            //Vector3 projNormPla = (Vector3.Dot(transform.up, planeNormal) / Vector3.Dot(transform.up, transform.up)) * planeNormal; //el dot del mateix dot es pot substituir per el quadrat del modul.... val la pena? seria 1 
            Vector3 projNormPla = Vector3.Dot(transform.up, planeNormal.normalized) * planeNormal.normalized;

            //Projecció del vector local y al pla format per el world Y i el vector local forward
            Vector3 projYPla = transform.up - projNormPla;

            tester.transform.position = transform.position + transform.up - projYPla;

            if (projYPla.y < 0) //comprovem que s'hagi projectat cap amunt i no cap avall -- es un pel cutre...
            {
                projYPla *= -1;
            }

            projYPla.Normalize(); //per si de cas

            Debug.DrawRay(transform.position, projYPla, Color.red);

            Debug.DrawRay(transform.position, projNormPla*100, Color.white);


            Debug.Log(projNormPla);

            Debug.DrawRay(transform.position, planeNormal, Color.green);

            Debug.DrawRay(transform.position, transform.up, Color.yellow);

            Debug.DrawRay(transform.position, Vector3.up, Color.blue);

            Debug.DrawRay(transform.position, transform.forward, Color.white);



            Vector3 r1 = projYPla;
            Vector3 r2 = transform.up;
            
                // find the components (cos i sin) using dot and cross product
                float cos = Vector3.Dot(r1, r2) / (r1.magnitude * r2.magnitude);
            float sin = Vector3.Cross(r1, r2).magnitude / (r1.magnitude * r2.magnitude);

            // The axis of rotation 
            Vector3 axis = Vector3.Cross(r1, r2).normalized;

            // find the angle between r1 and r2 (and clamp values if needed avoid errors)
            float theta = Mathf.Acos(cos);

            //Optional. correct angles if needed, depending on angles invert angle if sin component is negative
            //if (sin < 0)
                //theta = -theta;



            // obtain an angle value between -pi and pi, and then convert to degrees
            //theta[i] = TODO8
            theta *= Mathf.Rad2Deg;
            

            if (theta>2)
            {
                Debug.Log(theta);
                transform.rotation = Quaternion.AngleAxis(-theta, transform.forward) * transform.rotation; //descomentar

            }
            
        }
    

	}

    public Quaternion norm(Quaternion q)
    {
        float module = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        Quaternion q2 = new Quaternion(q.x/module,q.y/module,q.z/module,q.w/module) ; 
        return q2;


    }

}
