using UnityEngine;
using System.Collections;

public class propulsor : MonoBehaviour {

    public float forceMagnitude = 1;
    public myRigidbody myrigidBody;

	void Start () {

    }
	
	void Update () {
        //Sumar forces
        myrigidBody.totalForce += forceMagnitude * transform.forward;

        //Sumar torque
        Vector3 r = myrigidBody.getCenterOfMass() - transform.position;

        myrigidBody.totalTorque += Vector3.Cross(r, forceMagnitude * transform.forward);
	}
}
