using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IK_FABRIK2 : MonoBehaviour
{
    public Transform[] joints;
    public Transform target;
    //testing
    public Transform projection;
    public Transform plane, plane2;

    private Vector3[] copy;
    private float[] distances;
    private bool done;

    float threshold_distance = 0.1f;
    public int maxIterations = 10;



    void Start()
    {
        distances = new float[joints.Length - 1];
        copy = new Vector3[joints.Length];
    }

    void Update()
    {
        // Copy the joints positions to work with
        // and calculate all the distances
        //TODO1
        for (int i = 0; i < joints.Length; i++)
        {
            copy[i] = joints[i].position;
            if (i < joints.Length - 1)
            {
                distances[i] = (joints[i + 1].position - joints[i].position).magnitude;
            }
        }


        done = (Vector3.Distance(target.position, joints[joints.Length - 1].position) < threshold_distance);
        if (!done)
        {
            float targetRootDist = Vector3.Distance(copy[0], target.position);


            // Update joint positions
            if (targetRootDist > distances.Sum())
            {
                // The target is unreachable
                //TODO3
                for (int i = 0; i < copy.Length - 1; i++)
                {
                    float dist = (target.position - copy[i]).magnitude;
                    float ratio = distances[i] / dist;

                    copy[i + 1] = (1 - ratio) * copy[i] + ratio * target.position;

                }
            }
            else
            {


                int counter = 0;
                // The target is reachable
                while (!done && counter < maxIterations)
                {
                    counter++;

                    // STAGE 1: FORWARD REACHING
                    //TODO5
                    copy[copy.Length - 1] = target.position;
                    for (int i = copy.Length - 1; i > 0; i--)
                    {
                        Vector3 temp = (copy[i - 1] - copy[i]).normalized;
                        temp = temp * distances[i - 1];
                        copy[i - 1] = temp + copy[i];
                    }

                    // STAGE 2: BACKWARD REACHING
                    //TODO6
                    copy[0] = joints[0].position;
                    for (int i = 0; i < copy.Length - 2; i++)
                    {
                        Vector3 temp = (copy[i + 1] - copy[i]).normalized;
                        temp = temp * distances[i];
                        copy[i + 1] = temp + copy[i];
                    }
                    done = (Vector3.Distance(target.position, joints[joints.Length - 1].position) < threshold_distance);
                }
            }

            //---------------------TESTING CONSTRAINTS-------------------
            //Projectem el punt del joint 1 al pla
            Vector3 vectorToPlane = -plane.up;                                  //vector com la normal del pla, en direcció al pla
            Vector3 pointInLine = copy[1] + vectorToPlane;
            float escalar = Vector3.Dot(plane.up.normalized, (copy[0] - copy[1])) /
                (Vector3.Dot(plane.up.normalized, (pointInLine - copy[1])));    //copy[0]->punt en el pla
            Vector3 projectedPoint = copy[1] + escalar * vectorToPlane;         //sense tenir en compte les distancies
            if(escalar != 0)
            {
                projection.position = copy[0] + (projectedPoint - copy[0]).normalized * distances[0];    //vector director pla * distancia que toqui
                //CONTROLAR QUAN copy[0] i el projected son el mateix punt, fer algo...--------------------------
                copy[1] = projection.position;
                Debug.Log("QUE");
            }
            //recol·loquem la resta de nodes
            for (int i = 2; i <= copy.Length - 1; i++)
            {
                Vector3 temp = (copy[i] - copy[i - 1]);
                if(temp.magnitude > 0.000001f)
                {
                    temp = (copy[i] - copy[i - 1]).normalized;
                    temp = temp * distances[i-1];
                    copy[i] = temp + copy[i-1];
                }
            }


            //Projectem el punt del joint 2 al pla
            vectorToPlane = -plane2.up;                                          //vector com la normal del pla, en direcció al pla
            pointInLine = copy[2] + vectorToPlane;
            escalar = Vector3.Dot(plane2.up.normalized, (copy[1] - copy[2])) /
                (Vector3.Dot(plane2.up.normalized, (pointInLine - copy[2])));    //copy[1]->punt en el pla
            projectedPoint = copy[2] + escalar * vectorToPlane;                 //sense tenir en compte les distancies
            if (escalar != 0)
            {
                projection.position = copy[1] + (projectedPoint - copy[1]).normalized * distances[1];    //vector director pla * distancia que toqui
                //CONTROLAR QUAN copy[0] i el projected son el mateix punt, fer algo...--------------------------
                copy[2] = projection.position;
                Debug.Log("QUE");
            }
            //recol·loquem la resta de nodes                                    //----------------fer un mètode
            for (int i = 3; i <= copy.Length - 1; i++)
            {
                Vector3 temp = (copy[i] - copy[i - 1]);
                if (temp.magnitude > 0.000001f)
                {
                    temp = (copy[i] - copy[i - 1]).normalized;
                    temp = temp * distances[i - 1];
                    copy[i] = temp + copy[i - 1];
                }
            }
            //---------------------ENDING TESTING CONSTRAINTS-------------------



            // Update original joint rotations
            for (int i = 0; i <= joints.Length - 2; i++)
            {
                
                //TODO 4.
                //without rotations
                //joints[i].position = copy[i];

                //with rotations:
                Vector3 a, b;
                a = joints[i + 1].position - joints[i].position;
                b = copy[i + 1] - copy[i];
                Vector3 axis = Vector3.Cross(a, b).normalized;

                float cosa = Vector3.Dot(a, b) / (a.magnitude * b.magnitude);
                float sina = Vector3.Cross(a.normalized, b.normalized).magnitude;

                float alpha = Mathf.Atan2(sina, cosa);

                //float alpha = Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude));



                Quaternion q = new Quaternion(axis.x * Mathf.Sin(alpha / 2), axis.y * Mathf.Sin(alpha / 2), axis.z * Mathf.Sin(alpha / 2), Mathf.Cos(alpha / 2));
                joints[i].position = copy[i];
                joints[i].rotation = q * joints[i].rotation;

            }
        }
    }

}