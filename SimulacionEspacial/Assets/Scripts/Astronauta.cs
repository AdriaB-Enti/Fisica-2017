using UnityEngine;
using System.Collections;

public class Astronauta : MonoBehaviour {
    [SerializeField]
    public GameObject[] propulsors = new GameObject[4];
    //GetComponent<Rigidbody>().centerOfMass -- pot ser útil
    // Use this for initialization
    void Start () {
	
	}
	
	// de momento se le va bastante la olla
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 force = transform.rotation * new Vector3(-10, 0, 0);    //no es pot posar gaire més o s'envà 
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[0].transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[1].transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[2].transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[3].transform.position);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //GetComponent<Rigidbody>().AddForceAtPosition();
            GetComponent<Rigidbody>().AddForce(new Vector3(100, 0, 0)); //fer-ho per cada propulsor
        }
        if (Input.GetKey(KeyCode.A))    //propulsores lado izquierdo
        {
            Vector3 force = transform.rotation * new Vector3(-50, 0, 0);
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[0].transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[2].transform.position);
        }
        if (Input.GetKey(KeyCode.D))    //propulsores lado izquierdo
        {
            Vector3 force = transform.rotation * new Vector3(-50, 0, 0);
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[1].transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(force, propulsors[3].transform.position);
        }


        //TODO: afegir 
    }
}
