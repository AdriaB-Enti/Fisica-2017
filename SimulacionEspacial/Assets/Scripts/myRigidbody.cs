//Code By Adrià Biarnés - 3B (ENTI)
using UnityEngine;
using System.Collections;
using myClasses;

public class myRigidbody : MonoBehaviour {

    //Matrix4x4 transformationT;
    float mass = 97;
    Matrix3 iBody;
    Matrix3 rotation;
    Matrix3 inertiaTensor;

    Vector3 Pt; //Linear momentum
    Vector3 L;  //Angular momentum
    public Vector3 totalTorque;
    public Vector3 totalForce;
    Vector3 velocity;
    Vector3 position;
    Vector3 w;
    Quaternion q;


    //Usant les meves classes:
    myVector3 myPt; //Linear momentum
    myVector3 myL;  //Angular momentum
    public myVector3 mytotalTorque;
    public myVector3 mytotalForce;
    myVector3 myvelocity;
    myVector3 myposition;
    myVector3 myw;
    MyQuaternion myq;

    Transform centerOfMassTransform;

    void Start () {
        //setejar els atributs?
        //iBody = Matrix3.iBodyBox(mass, 1.95f, 1f, 1.3f); //VALORS ARBITRARIS--------------------------- POSAR ALGO MÉS PRECÍS
        iBody = Matrix3.iBodyBox(mass, 2f, 2f, 2f); //TESTING----------- DELETE------------------------------------------------
        rotation    = new Matrix3();
        inertiaTensor = new Matrix3();

        Pt          = new Vector3(0,0,0);
        L           = new Vector3();
        totalTorque = new Vector3();
        totalForce  = new Vector3();
        velocity    = new Vector3();
        position    = transform.position;  //CANVIAR?------------------------------------------------------
        w           = new Vector3();        //CANVIAR!!!------------------------------------------------------
        q           = transform.rotation;  //CANVIAR?------------------------------------------------------
        myq = new MyQuaternion(transform.rotation);
    }

    void Update () {

        euler();

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
        //print("inertia Tensor:");
        //inertiaTensor.print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print("inertia Tensor inverted:");
        //inertiaTensor.getInverse().print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        myVector3 convertedL = myVector3.unityVec3ToMyVec3(L);  //TODO: fer servir nomes myVector3
        //w = inertiaTensor.getInverse() * L;
        w = (inertiaTensor.getInverse() * convertedL);
        //print(w);   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print(Matrix3.error);   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        myq = new MyQuaternion(q);
        MyQuaternion myq2 = new MyQuaternion(myVector3.unityVec3ToMyVec3(w));       //canviar
        MyQuaternion mytime = ((1.0f / 2.0f) * myq2) * myq;
        myq = myq + mytime;
        myq.normalize();


        Quaternion q2 = new Quaternion(w.x, w.y, w.z, 0);
        Quaternion time = multiplyQuat(q2, (1.0f / 2.0f)) * q;
        q = sumQuat(q, time);
        normalizeQuat(q);

        //transform.rotation = q;
        transform.rotation = myq.toUnityQuat();

        transform.position = position;

        
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
