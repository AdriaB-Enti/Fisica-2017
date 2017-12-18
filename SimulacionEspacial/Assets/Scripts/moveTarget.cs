using UnityEngine;
using System.Collections;

public class moveTarget : MonoBehaviour {

    float speed = 15;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(Time.deltaTime*speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(-Time.deltaTime * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(0, 0, Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0, 0, -Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            transform.position += new Vector3(0, Time.deltaTime * speed, 0);
        }
        if (Input.GetKey(KeyCode.Plus))
        {
            transform.position += new Vector3(0, -Time.deltaTime * speed, 0);
        }
    }
}
