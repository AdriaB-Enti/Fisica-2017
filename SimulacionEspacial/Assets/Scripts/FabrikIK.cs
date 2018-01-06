using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FabrikIK : MonoBehaviour
{
    public Transform[] joints;
    public Transform target;
    //testing
    public Transform projection;
    public Transform anotherProjection;
    //public Transform plane, plane2;
    public Transform[] planes;
    public Quaternion[] planCopyRotations;

    private Vector3[] copy;
    private float[] distances;
    private bool done;

    float threshold_distance = 0.1f;
    public int maxIterations = 10;



    void Start()
    {
        distances = new float[joints.Length - 1];
        copy = new Vector3[joints.Length];
        planCopyRotations = new Quaternion[planes.Length];
        //-----------------------------------------------guardar les rotacions originals dels plans? (planeCopyRotations)-----------------
    }

    void Update()
    {
        // Copy the joints positions to work with
        // and calculate all the distances
        for (int i = 0; i < joints.Length; i++)
        {
            copy[i] = joints[i].position;
            if (i < joints.Length - 1)
            {
                distances[i] = (joints[i + 1].position - joints[i].position).magnitude;
            }
        }

        //copy plane rotations
        for (int i = 0; i < planes.Length; i++)
        {
            planCopyRotations[i] = planes[i].rotation;
        }

        done = (Vector3.Distance(target.position, joints[joints.Length - 1].position) < threshold_distance);
        if (!done)
        {
            float targetRootDist = Vector3.Distance(copy[0], target.position);

            // Update joint positions
            if (targetRootDist > distances.Sum())
            {
                // The target is unreachable
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

                    // 1- FORWARD REACHING
                    copy[copy.Length - 1] = target.position;
                    for (int i = copy.Length - 1; i > 0; i--)
                    {
                        Vector3 temp = (copy[i - 1] - copy[i]).normalized;
                        temp = temp * distances[i - 1];
                        copy[i - 1] = temp + copy[i];
                    }

                    // 2- BACKWARD REACHING
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

            //actualitzar rotacions plans
            updatePlaneRotations();

            //---------------------CONSTRAINTS-------------------
            Vector3 vectorToPlane;
            Vector3 pointInLine;
            float escalar;
            Vector3 projectedPoint;

            for (int i = 1; i <= 3; i++)     //joints que necessiten constraints
            {
                //Projectem el punt del joint al pla
                vectorToPlane = -planes[i-1].up;                                            //vector com la normal del pla, en direcció al pla
                pointInLine = copy[i] + vectorToPlane;
                escalar = Vector3.Dot(planes[i-1].up.normalized, (copy[i-1] - copy[i])) /
                    (Vector3.Dot(planes[i-1].up.normalized, (pointInLine - copy[i])));      //copy[1]->punt en el pla
                projectedPoint = copy[i] + escalar * vectorToPlane;                         //sense tenir en compte les distancies
                if (escalar != 0)
                {
                    projection.position = copy[i-1] + (projectedPoint - copy[i-1]).normalized * distances[i-1];    //vector director en el pla * distancia que toqui
                    //CONTROLAR QUAN copy[0] i el projected son el mateix punt, fer algo?
                    copy[i] = projection.position;
                }
                repositionCopyNodes(i+1);
            }
            //---------------------END CONSTRAINTS-------------------

            updateJointRotations();

        }
    }

    //actualitza la rotació del pla segons les posicions dels copy
    void updatePlaneRotations()
    {
        //plane 1
        //buscar la projecció dels joint 1 i el copy 1 al pla 

        /*Vector3 vectorToPlane = -Vector3.up;    //només funcionarà si la nau no està girada
        Vector3 pointInLine = copy[1] + vectorToPlane;
        float escalar = Vector3.Dot(Vector3.up, (copy[0] - copy[1])) /
                    (Vector3.Dot(Vector3.up, (pointInLine - copy[1])));
        Vector3 projectedPoint = copy[1] + escalar * vectorToPlane;
        Vector3 projection = copy[1];
        if (escalar != 0)
        {
            projection = copy[0] + (projectedPoint - copy[0]).normalized * distances[0];
        }*/
        Vector3 jointProjection = getProjectionToPlane(Vector3.up, joints[0].position, joints[1].position); //VIGILAR SI FAIG SERVIR EL COPY O EL JOINT (EL COPY DESPRES S'ACABA MODIFICANT)
        Vector3 copyProjection = getProjectionToPlane(Vector3.up, joints[0].position, copy[1]); //VIGILAR SI FAIG SERVIR EL COPY O EL JOINT (EL COPY DESPRES S'ACABA MODIFICANT)

        anotherProjection.position = copyProjection;

        //buscar la rotació en l'eix Y entre copy[0] i copy[1]
        Quaternion planeRotation = getRotationFrom(joints[0].position, joints[0].position, jointProjection, copyProjection);

        //guardar la rotació 
        //planCopyRotations[i] = 
        planes[0].rotation = planeRotation * planes[0].rotation;
    }

    //Rotació entre dos punts amb un punt d'origen donat
    Quaternion getRotationFrom(Vector3 originA, Vector3 originB, Vector3 pointA, Vector3 pointB)  //afegeixo un altre origen?
    {
        //originA -> joints[i]
        //originB -> copy[1]
        //pointA  -> joints[i+1]

        Vector3 a = pointA - originA;
        Vector3 b = pointB - originB;
        Vector3 axis = Vector3.Cross(a, b).normalized;

        float cosa = Vector3.Dot(a, b) / (a.magnitude * b.magnitude);
        float sina = Vector3.Cross(a.normalized, b.normalized).magnitude;
        float alpha = Mathf.Atan2(sina, cosa);

        return new Quaternion(axis.x * Mathf.Sin(alpha / 2), axis.y * Mathf.Sin(alpha / 2), axis.z * Mathf.Sin(alpha / 2), Mathf.Cos(alpha / 2));
    }


    //Projeccio del punt al pla (retorna el mateix punt si ja estava al pla)
    Vector3 getProjectionToPlane(Vector3 planeNormal, Vector3 pointInPlane, Vector3 pointToProject)
    {
        Vector3 pointInLine = pointToProject - planeNormal;
        float escalar = Vector3.Dot(Vector3.up.normalized, (pointInPlane - pointToProject)) /
                    (Vector3.Dot(Vector3.up.normalized, (pointInLine - pointToProject)));
        return (pointToProject + escalar * (-planeNormal));
    }

    // Update all original joint rotations
    void updateJointRotations()
    {
        for (int i = 0; i <= joints.Length - 2; i++)
        {
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

    //recol·loquem la resta de nodes (començant per el startNode)
    void repositionCopyNodes(int startNode)
    {
        for (int i = startNode; i <= copy.Length - 1; i++)
        {
            Vector3 temp = (copy[i] - copy[i - 1]);
            if (temp.magnitude > 0.000001f)
            {
                temp = (copy[i] - copy[i - 1]).normalized;
                temp = temp * distances[i - 1];
                copy[i] = temp + copy[i - 1];
            }
        }
    }

    /*
     * Rota el primer joint sense aplicar-li twist 
     */
    void rotateWithoutTwist(int joint, Quaternion newRotation)
    {



    }

    //Actualitza la rotació del primer (no està dintre  joint de tots (només és un twist)
    void updateTwistJoint() {

    }
}