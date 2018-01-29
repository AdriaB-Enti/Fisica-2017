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
        iBody       = Matrix3.iBodyBox(mass, 1.95f, 1f, 1.3f); //VALORS ARBITRARIS--------------------------- POSAR ALGO MÉS PRECÍS
        rotation    = new Matrix3();
        inertiaTensor = new Matrix3();

        Pt          = new Vector3(0,0,0);
        L           = new Vector3();
        totalTorque = new Vector3();
        totalForce  = new Vector3();
        velocity    = new Vector3();
        position    = transform.position;  //CANVIAR?------------------------------------------------------
        w           = new Vector3();        //CANVIAR!!!------------------------------------------------------
        q = transform.rotation;  //CANVIAR?------------------------------------------------------


        myPt            = new myVector3(0);
        myL             = new myVector3(0);
        mytotalTorque   = new myVector3(0);
        mytotalForce    = new myVector3(0);
        myvelocity      = new myVector3(0);
        myposition      = myVector3.unityToMyVec(transform.position);
        myw             = new myVector3(0);        //CANVIAR!!!------------------------------------------------------
        myq             = new MyQuaternion(transform.rotation);
    }

    void Update () {

        euler();

        //resetejar les forces i el torque
        //totalForce = new Vector3();
        //totalTorque = new Vector3();

        mytotalTorque = new myVector3(0);
        mytotalForce = new myVector3(0);
    }

    void euler()
    {
        //print(Matrix3.error);   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //Pt = Pt + UnityEngine.Time.deltaTime * (mass * totalForce);
        //L = L + UnityEngine.Time.deltaTime * totalTorque;

        //velocity = Pt / mass;
        //position = position + UnityEngine.Time.deltaTime * velocity;
        //print("Quaternion: " + q.w + " x" + q.x + " y" + q.y + " z" + q.z);   //det = 0   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        //Opció 1:
        //rotation = new Matrix3(q);
        //print("Rotation:");   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //rotation.print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print("Rotation transposed:");
        //rotation.getTransposed().print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print("iBody:");   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //iBody.print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        //print("inertia Tensor:");
        //inertiaTensor.print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print("inertia Tensor inverted:");
        //inertiaTensor.getInverse().print();   //---------------------------------------------------------------------------------------------------------------------------DEBUG


        myPt = myPt + UnityEngine.Time.deltaTime * (mass * mytotalForce);
        myL = myL + UnityEngine.Time.deltaTime * mytotalTorque;
        myvelocity = myPt / mass;
        myposition = myposition + UnityEngine.Time.deltaTime * myvelocity;
        rotation = new Matrix3(myq);

        inertiaTensor = rotation * iBody * rotation.getTransposed();

        //myVector3 convertedL = myVector3.unityVec3ToMyVec3(L);  //TODO: fer servir nomes myVector3
        myw = inertiaTensor.getInverse() * myL;
        //w = (inertiaTensor.getInverse() * convertedL);
        //print(w);   //---------------------------------------------------------------------------------------------------------------------------DEBUG
        //print(Matrix3.error);   //---------------------------------------------------------------------------------------------------------------------------DEBUG

        MyQuaternion myq2 = new MyQuaternion(myw);       //canviar
        MyQuaternion mytime = ((1.0f / 2.0f) * myq2) * myq;
        myq = myq + mytime;
        myq.normalize();


        //Quaternion q2 = new Quaternion(w.x, w.y, w.z, 0);
        //Quaternion time = multiplyQuat(q2, (1.0f / 2.0f)) * q;
        //q = sumQuat(q, time);
        //normalizeQuat(q);

        //transform.rotation = q;
        //transform.position = position;

        //Update the position and rotation
        transform.rotation = myq.toUnityQuat();
        transform.position = myposition.toUnityVector3();

    }

    public Vector3 getCenterOfMassVec3()        //............................................................................. CANVIAR
    {
        return transform.position;
    }

    public myVector3 getCenterOfMass()        //............................................................................. CANVIAR
    {
        return myposition;
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
