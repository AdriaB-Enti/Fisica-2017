using UnityEngine;
using System.Collections;

public class propulsor : MonoBehaviour {

    public float forceMagnitude = 1;
    public myRigidbody myrigidBody;

	void Start () {

    }
	
	void Update () {
        //Sumar forces
        myrigidBody.totalForce += forceMagnitude * transform.forward;   //CANVIAR.....................

        //Sumar torque
        Vector3 r = transform.position - myrigidBody.getCenterOfMass();

        myrigidBody.totalTorque += Vector3.Cross(r, forceMagnitude * transform.forward);
	}
}
