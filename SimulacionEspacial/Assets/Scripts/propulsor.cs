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
        //myrigidBody.totalForce += forceMagnitude * transform.forward;   //CANVIAR..............------------------------.......
        //Sumar torque
        //Vector3 r = transform.position - myrigidBody.getCenterOfMass();
        //myrigidBody.totalTorque += Vector3.Cross(r, forceMagnitude * transform.forward);


        //Haig d'esbrinar com pillar el forward del transform....?--------------------------------------------
        //Sumar forces
        myrigidBody.mytotalForce += forceMagnitude * forward;
        //Sumar torque
        myVector3 r = myVector3.unityToMyVec(transform.position) - myrigidBody.getCenterOfMass();
        myrigidBody.mytotalTorque += myVector3.Cross(r, forceMagnitude * forward);
        
    }
}

