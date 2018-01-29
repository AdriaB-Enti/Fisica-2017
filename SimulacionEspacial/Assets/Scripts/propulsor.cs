using UnityEngine;
using System.Collections;
using myClasses;

public class propulsor : MonoBehaviour
{

    public float forceMagnitude = 1;
    public myRigidbody myrigidBody;
    myVector3 forward;
    void Start()
    {

    }

    void Update()
    {
        forward = myVector3.unityToMyVec(transform.forward);    //CANVIAR..............------------------------.......

        //Sumar forces
        myrigidBody.totalForce += forceMagnitude * forward;
        //Sumar torque
        myVector3 r = myVector3.unityToMyVec(transform.position) - myrigidBody.getCenterOfMass();
        myrigidBody.totalTorque += myVector3.Cross(r, forceMagnitude * forward);
        
    }
}

