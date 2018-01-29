//Code By Adrià Biarnés - 3B (ENTI)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class simulationController : MonoBehaviour {

    public static bool simulationPaused = false;
    public static bool ikActivat = false;
    public bool thrustersStopped = false;

    public myRigidbody myRigidB;

    public GameObject velocitat, velocitatAngular;
    public UnityEngine.UI.InputField massaField;    //no es tindra en compte el Ibody...

    public Camera[] cameras = new Camera[3];
    int currentCamera = 0;

    public GameObject canvas;

    public propulsor[] propulsorsBack = new propulsor[4];
    public propulsor[] propulsorsLeft = new propulsor[4];
    public propulsor[] propulsorsRight = new propulsor[4];
    public propulsor[] propulsorsFront = new propulsor[4];

    // Use this for initialization
    void Start () {
        cameras[1].enabled = false;
        cameras[2].enabled = false;
        massaField.text = ""+myRigidB.mass;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.R))    //Resets the scene
        {
            //Application.LoadLevel(Application.loadedLevel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.P))    //pauses simulation (no physics are calculed)
        {
            simulationPaused = !simulationPaused;
        }
        if (Input.GetKeyDown(KeyCode.I))    //enables-disables IK (braç robòtic)
        {
            ikActivat = !ikActivat;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            changeCamera();
        }


        if (!simulationPaused)
        {
            updateTextValues();
        }

    }

    void changeCamera()
    {
        currentCamera++;
        if (currentCamera>2)
        {
            currentCamera = 0;
        }
        //if (!cameras[cameraIndex].enabled)
        //{
        Camera.main.enabled = false;
        cameras[currentCamera].enabled = true;
        //}
    }

    public void togglePanel()
    {
         canvas.SetActive(!canvas.activeSelf);
    }

    public void updateTextValues()
    {
        velocitat.GetComponent<UnityEngine.UI.Text>().text = myRigidB.velocity.getString();
        velocitatAngular.GetComponent<UnityEngine.UI.Text>().text = myRigidB.w.getString();
        
    }

    public void disableAllThrusters()
    {
        foreach (propulsor prop in propulsorsBack)
        {
            prop.forceMagnitude = 0;
        }
        foreach (propulsor prop in propulsorsFront)
        {
            prop.forceMagnitude = 0;
        }
        foreach (propulsor prop in propulsorsLeft)
        {
            prop.forceMagnitude = 0;
        }
        foreach (propulsor prop in propulsorsRight)
        {
            prop.forceMagnitude = 0;
        }
    }

    public void activateBackThrusters()
    {
        propulsorsBack[0].forceMagnitude = 0.0003f;
        propulsorsBack[1].forceMagnitude = 0.0003f;
        propulsorsBack[2].forceMagnitude = 1f;
        propulsorsBack[3].forceMagnitude = 1f;
        /*foreach (propulsor prop in propulsorsBack)
        {
            prop.forceMagnitude = 1;
        }*/
    }

    public void activateLeftThrusters()
    {
        foreach (propulsor prop in propulsorsLeft)
        {
            prop.forceMagnitude = 1;
        }
    }

    public void activateRightThrusters()
    {
        foreach (propulsor prop in propulsorsRight)
        {
            prop.forceMagnitude = 1;
        }
    }

    public void activateFrontThrusters()
    {
        foreach (propulsor prop in propulsorsFront)
        {
            prop.forceMagnitude = 1;
        }
    }


    public void changeMass(string newMass)
    {
        /*int nm = 0;
        if (int.TryParse(newMass, out nm))
        {
            myRigidB.mass = nm;
        }*/
    }
}
