//Code By Adrià Biarnés - 3B (ENTI)
using UnityEngine;
using System.Collections;
using myClasses;

public class myRigidbody : MonoBehaviour {
    //Start values
    const float DEFAULTMASS = 240f; //MMU+astronaut
    //Size of the "box"
    const float HEIGHT  = 1.87f;
    const float WIDTH   = 1.049f;
    const float DEPTH   = 1.186f;
    float mass = DEFAULTMASS;

    Matrix3 iBody;
    Matrix3 rotation;
    Matrix3 inertiaTensor;

    myVector3 P;                     //Linear momentum
    myVector3 L;                      //Angular momentum
    public myVector3 totalTorque;     //Total torque sum
    public myVector3 totalForce;      //Total force sum
    myVector3 position;
    myVector3 velocity;               //Speed
    myVector3 w;                      //Angular speed
    MyQuaternion q;                   //Current rotation

    Transform centerOfMassTransform;

    /* Variables amb classes de Unity per fer proves
    Vector3 Pt; //Linear momentum
    Vector3 L;  //Angular momentum
    public Vector3 totalTorque;
    public Vector3 totalForce;
    Vector3 velocity;
    Vector3 position;
    Vector3 w;
    Quaternion q;
    */

    void Start () {
        iBody       = Matrix3.iBodyBox(mass, HEIGHT, WIDTH, DEPTH);

        //Set all variables to the initial values
        rotation        = new Matrix3();
        inertiaTensor   = new Matrix3();
        P            = new myVector3(0);
        L             = new myVector3(0);
        totalTorque   = new myVector3(0);
        totalForce    = new myVector3(0);
        velocity      = new myVector3(0);
        w             = new myVector3(0);
        q             = new MyQuaternion(transform.rotation);
        position      = myVector3.unityToMyVec(transform.position);
    }

    void Update () {

        euler();

        //Reset the total force and torque
        totalTorque   = new myVector3(0);
        totalForce    = new myVector3(0);
    }

    void euler()
    {
        P = P + UnityEngine.Time.deltaTime * (mass * totalForce);
        L = L + UnityEngine.Time.deltaTime * totalTorque;
        velocity = P / mass;
        position = position + UnityEngine.Time.deltaTime * velocity;

        rotation = new Matrix3(q);
        inertiaTensor = rotation * iBody * rotation.getTransposed();

        w = inertiaTensor.getInverse() * L;

        MyQuaternion q2 = new MyQuaternion(w);
        MyQuaternion time = ((1.0f / 2.0f) * q2) * q;
        q = q + time;
        q.normalize();

        //Update the position and rotation
        transform.rotation = q.toUnityQuat();
        transform.position = position.toUnityVector3();

    }

    public myVector3 getCenterOfMass()
    {
        return position;
    }

    /* Les següents funcions eren per fer proves amb les classes de unity
    public Vector3 getCenterOfMassVec3()
    {
        return transform.position;
    }
    Quaternion normalizeQuat(Quaternion quat)
    {
        float sqrtSum = Mathf.Sqrt(quat.w * quat.w + quat.x * quat.x + quat.y * quat.y + quat.z * quat.z);
        return new Quaternion(quat.x/sqrtSum, quat.y/sqrtSum, quat.z/sqrtSum, quat.w/sqrtSum);
    }
    Quaternion multiplyQuat(Quaternion quat, float scalar)
    {
        return new Quaternion(quat.x * scalar, quat.y * scalar, quat.z * scalar, quat.w * scalar);
    }
    Quaternion sumQuat(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.x+q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
    }
    */
}
