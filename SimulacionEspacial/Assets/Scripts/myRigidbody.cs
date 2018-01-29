using UnityEngine;
using System.Collections;
using myClasses;

public class myRigidbody : MonoBehaviour {

    float mass = 97;
    Matrix4x4 transformationT;
    Matrix3 iBody;
    Matrix3 rotation;
    Matrix3 inertiaTensor;

    Vector3 Pt; //Momento lineal
    Vector3 L; //Momento angular
    public Vector3 totalTorque;
    public Vector3 totalForce;
    Vector3 velocity;
    Vector3 position;
    Vector3 w;
    Quaternion q;

    Transform centerOfMassTransform;

    void Start () {
        //setejar els atributs?
        transformationT = new Matrix4x4();
        //iBody = Matrix3.iBodyBox(mass, 1.95f, 1f, 1.3f); //VALORS ARBITRARIS--------------------------- POSAR ALGO MÉS PRECÍS
        iBody = Matrix3.iBodyBox(mass, 2f, 2f, 2f); //TESTING----------- DELETE------------------------------------------------
        rotation = new Matrix3();
        inertiaTensor   = new Matrix3();

        Pt          = new Vector3(0,0,0);
        L           = new Vector3();
        totalTorque = new Vector3();
        totalForce  = new Vector3();
        velocity    = new Vector3();
        position    = transform.position;  //CANVIAR?------------------------------------------------------
        w           = new Vector3();        //CANVIAR!!!------------------------------------------------------
        q           = transform.rotation;  //CANVIAR?------------------------------------------------------
        
    }

    void Update () {
        //actualitzar el transform 

        //sumar les forces i els torques dels propulsors

        euler();
        transform.position = position;

        //resetejar les forces i el torque
        totalForce = new Vector3();
        totalTorque = new Vector3();
	}

    void euler()
    {
        //print(Matrix3.error);   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        Pt = Pt + UnityEngine.Time.deltaTime * (mass * totalForce);
        L = L + UnityEngine.Time.deltaTime * totalTorque;

        velocity = Pt / mass;

        position = position + UnityEngine.Time.deltaTime * velocity;
        //print("Quaternion: " + q.w + " x" + q.x + " y" + q.y + " z" + q.z);   //det = 0   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        //Opció 1:
        rotation = new Matrix3(q);
        //print("Rotation:");   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //rotation.print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print("Rotation transposed:");
        //rotation.getTransposed().print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print("iBody:");   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //iBody.print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        inertiaTensor = rotation * iBody * rotation.getTransposed();
        print("inertia Tensor:");
        inertiaTensor.print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        print("inertia Tensor inverted:");
        inertiaTensor.getInverse().print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        myVector3 convertedL = myVector3.unityVec3ToMyVec3(L);  //TODO: fer servir nomes myVector3
        //w = inertiaTensor.getInverse() * L;
        w = (inertiaTensor.getInverse() * convertedL);
        print(w);   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print(Matrix3.error);   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        Quaternion q2 = new Quaternion(w.x, w.y, w.z, 0);
        
        Quaternion time = multiplyQuat(q2, (1.0f / 2.0f)) * q; //s'ha de normalitzar???

        q = sumQuat(q, time);

        normalizeQuat(q);

        transform.rotation = new Quaternion(q.x,q.y,q.z,q.w);
        //transform.rotation = q;

        
        //transformationT = q.toMat4();

        //translate
        //rotate
        //scale

        //Opció 2 -----------PER FER TESTING
        /*
        Vector3 axis = totalTorque.normalized;
        float w = -L.magnitude / 2;  //velocitat angular 
        transform.Rotate(axis, w * UnityEngine.Time.deltaTime*Mathf.Rad2Deg);
        */
    }

    public Vector3 getCenterOfMass()
    {
        return position;
    }



    //TEMPORAL----------------------------------------------------------------------
    Quaternion vec3ToQuat(Vector3 vector)
    {
        return new Quaternion(vector.x, vector.y, vector.z, 0);
    }

    Quaternion normalizeQuat(Quaternion quat)
    {
        float sqrtSum = Mathf.Sqrt(quat.w * quat.w + quat.x * quat.x + quat.y * quat.y + quat.z * quat.z);
        return new Quaternion(quat.x/sqrtSum, quat.y/sqrtSum, quat.z/sqrtSum, quat.w/sqrtSum);
    }

    Quaternion multiplyQuat(Quaternion quat, float scalar)
    {
        //normalitzar???
        return new Quaternion(quat.x * scalar, quat.y * scalar, quat.z * scalar, quat.w * scalar);
    }

    Quaternion sumQuat(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.x+q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
    }
    
}
