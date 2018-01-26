using UnityEngine;
using System.Collections;

public class propulsor : MonoBehaviour {

    public float forceMagnitude = 1;
    public myRigidbody mrigidBody;

	void Start () {

    }
	
	void Update () {
        mrigidBody.totalForce += forceMagnitude * transform.forward;
	}
}
