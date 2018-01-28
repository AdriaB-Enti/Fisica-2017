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
    //Vector3 w;
    Quaternion q;

    Transform centerOfMassTransform;

    void Start () {
        //setejar els atributs?
        transformationT = new Matrix4x4();
        iBody           = new Matrix3();
        rotation        = new Matrix3();
        inertiaTensor   = new Matrix3();

        Pt          = new Vector3(0,0,0);
        L           = new Vector3();
        totalTorque = new Vector3();
        totalForce  = new Vector3();
        velocity    = new Vector3();
        position    = new Vector3();        //agafar posicio inicial GO?
        //w           = new Vector3();
        q           = Quaternion.identity;  //agafar rotació inicial del GO?

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
        Pt = Pt + UnityEngine.Time.deltaTime * (mass * totalForce);
        L = L + UnityEngine.Time.deltaTime * totalTorque;

        velocity = Pt / mass;

        position = position + UnityEngine.Time.deltaTime * velocity;

        //Opció 1:
        //rotation = q.toMat3()

        //inertiaTensor = rotation * iBody * rotation;
        //w = inertiaTensor * L;

        //Quaternion q2 = new Quaternion(0, w.x, w.y, w.z);
        //Quaternion time;
        //Quaternion time = (1.0f / 2.0f) * q2 * q;

        //q = q + time;

        //q.normalize;

        //transformationT = q.toMat4();

        //translate
        //rotate
        //scale

        //Opció 2 -----------PER FER TESTING
        Vector3 axis = totalTorque.normalized;
        //float 
        float w = -L.magnitude / 2;  //velocitat angular 
        transform.Rotate(axis, w * UnityEngine.Time.deltaTime*Mathf.Rad2Deg);

    }

    public Vector3 getCenterOfMass()
    {
        return position;
    }
}
