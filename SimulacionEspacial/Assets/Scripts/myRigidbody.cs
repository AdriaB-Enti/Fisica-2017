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
    Vector3 torque;
    Vector3 totalForce;
    Vector3 velocity;
    Vector3 position;
    Vector3 w;
    Quaternion q;



    void Start () {
        //setejar els atributs?
        transformationT = new Matrix4x4();
        iBody           = new Matrix3();
        rotation        = new Matrix3();
        inertiaTensor   = new Matrix3();

        Pt          = new Vector3(100,0,0);
        L           = new Vector3();
        torque      = new Vector3();
        totalForce  = new Vector3();
        velocity    = new Vector3();
        position    = new Vector3();        //agafar posicio inicial GO?
        w           = new Vector3();
        q           = Quaternion.identity;  //agafar rotació inicial del GO?

    }

    void Update () {
        //actualitzar el transform 
        euler();
        transform.position = position;

	}

    void euler()
    {
        Pt = Pt + UnityEngine.Time.deltaTime * (mass * totalForce);
        L = L + UnityEngine.Time.deltaTime * torque;

        velocity = Pt / mass;

        position = position + UnityEngine.Time.deltaTime * velocity;

        //rotation = q.toMat3()

        //inertiaTensor = rotation * iBody * rotation;
        //w = inertiaTensor * L;

        Quaternion q2 = new Quaternion(0, w.x, w.y, w.z);
        Quaternion time;
        //Quaternion time = (1.0f / 2.0f) * q2 * q;

        //q = q + time;

        //q.normalize;

        //transformationT = q.toMat4();


        //translate
        //rotate
        //scale
        

    }
}
