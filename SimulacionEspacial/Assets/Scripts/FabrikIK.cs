//Code By Adrià Biarnés - 3B (ENTI)
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FabrikIK : MonoBehaviour
{
    public Transform[] joints;
    public Transform arm0;
    public Transform target;

    //Fabrik vars
    private Vector3[] copy;
    private float[] distances;
    private bool done;
    float threshold_distance = 0.1f;
    public int maxIter = 10;


    //Constraints + Debug vars -- (No les faig servir)
    public Transform projection;        //DEBUG
    public Transform anotherProjection; //DEBUG
    public Transform[] copyDEBUG;        //DEBUG
    //public Transform plane, plane2;
    public Transform[] planes;
    public Quaternion[] planeCopyRotations;
    public Vector3[] planeNormals;



    void Start()
    {
        distances = new float[joints.Length - 1];
        copy = new Vector3[joints.Length];

        planeCopyRotations = new Quaternion[planes.Length];
        planeNormals = new Vector3[planes.Length];
    }

    void Update()
    {
        if (simulationController.ikActivat)
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
                planeCopyRotations[i] = planes[i].rotation;
                planeNormals[i] = planes[i].up;
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
                    while (!done && counter < maxIter)
                    {
                        counter++;

                        //Forward
                        copy[copy.Length - 1] = target.position;
                        for (int i = copy.Length - 1; i > 0; i--)
                        {
                            Vector3 temp = (copy[i - 1] - copy[i]).normalized;
                            temp = temp * distances[i - 1];
                            copy[i - 1] = temp + copy[i];
                        }

                        //Backward
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

                //Plane Constraints (no funcionaven bé)
                //updatePlaneCopyRotations();
                //constraints();

                updateJointRotations();
            }
        }
        
    }

    // Update all original joint rotations and positions
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






    /*constraints - no funciona
    void constraints()
    {
        //actualitzar rotacions plans
        updatePlaneCopyRotations();

        Vector3 vectorToPlane;
        Vector3 pointInLine;
        float escalar;
        Vector3 projectedPoint;

        for (int i = 1; i <= 3; i++)     //joints que necessiten constraints
        {
            //Projectem el punt del joint al pla
            //vectorToPlane = -planes[i - 1].up;                                            //vector com la normal del pla, en direcció al pla--------------------------------
            vectorToPlane = -planeNormals[i - 1];                                            //vector com la normal del pla, en direcció al pla--------------------------------
            pointInLine = copy[i] + vectorToPlane;
            escalar = Vector3.Dot(planeNormals[i - 1].normalized, (copy[i - 1] - copy[i])) / (Vector3.Dot(planeNormals[i - 1].normalized, (pointInLine - copy[i])));      //copy[1]->punt en el pla----------
                                                                                                                                                                          //escalar = Vector3.Dot(planes[i-1].up.normalized, (copy[i-1] - copy[i])) /
                                                                                                                                                                          //(Vector3.Dot(planes[i-1].up.normalized, (pointInLine - copy[i])));      //copy[1]->punt en el pla
            projectedPoint = copy[i] + escalar * vectorToPlane;                         //sense tenir en compte les distancies
            if (escalar != 0)
            {
                projection.position = copy[i - 1] + (projectedPoint - copy[i - 1]).normalized * distances[i - 1];    //vector director en el pla * distancia que toqui
                                                                                                                     //CONTROLAR QUAN copy[0] i el projected son el mateix punt, fer algo?
                copy[i] = projection.position;
            }
            repositionCopyNodes(i + 1);
        }
    }

    //TODO---------------------------------------------------------------------------------------------------------------------------------------
    //Rota el primer joint sense aplicar-li twist 
    void rotateWithoutTwist(int joint, Quaternion newRotation)
    {

    }

    //Actualitza la rotació del primer (no està dintre  joint de tots (només és un twist)
    void updateTwistJoint()
    {

    }

    //actualitza la rotació del pla segons les posicions dels copy
    void updatePlaneCopyRotations()
    {
        //plane 1
        //buscar la projecció dels joint 1 i el copy 1 al pla
        Vector3 jointProjection = getProjectionToPlane(Vector3.up, joints[0].position, joints[1].position);
        Vector3 copyProjection = getProjectionToPlane(Vector3.up, joints[0].position, copy[1]);

        //anotherProjection.position = copyProjection;    //------------------------------------------------------------------------------DEBUG

        //buscar la rotació en l'eix Y entre copy[0] i copy[1]
        Quaternion planeRotation = getRotationFrom(joints[0].position, joints[0].position, jointProjection, copyProjection);
        arm0.rotation = planeRotation * arm0.rotation;
        //guardar la rotació 
        //planCopyRotations[i] = 
        planeCopyRotations[0] = planeRotation * planes[0].rotation;                         //PER ESBORRAR SEGURAMENT-------------------------------------
        planeCopyRotations[1] = planeRotation * planes[1].rotation;

        //actualitzar rotacio plane 3
        //hem d'obtenir el vector de la normal del pla2 i rotarlo

        //DEBUG
        Debug.DrawRay(joints[0].position, planeNormals[0] * 30, Color.blue);
        Debug.DrawRay(joints[2].position, planeNormals[1] * 30, Color.blue);
        Debug.DrawRay(joints[3].position, planeNormals[2] * 30, Color.blue);

        planeNormals[0] = rotateVecWithQuat(planeNormals[0], planeRotation);
        planeNormals[1] = rotateVecWithQuat(planeNormals[1], planeRotation);

        //Necessitem trasladar els punts joints[2] i joints[3] per tal de poder projectar correctament els punts
        Vector3 translateVec = copy[2] - joints[2].position; //vector per trasladar joints[2] a copy[2]
        Vector3 joint3Translated = joints[3].position + translateVec;
        copyDEBUG[5].position = joint3Translated;

        Debug.DrawRay(joint3Translated, copy[2] - joint3Translated, Color.white);
        
        Vector3 joint3projec = getProjectionToPlane(planeNormals[1], copy[2], joint3Translated); //DONA PROBLEMES - DONA INFINIT
        Vector3 copy3projec = getProjectionToPlane(planeNormals[1], copy[2], copy[3]);
        Quaternion plane3VertRot = getRotationFrom(copy[2], copy[2], joint3projec, copy3projec);

        print(joint3projec);
        print(copy3projec);

        planeNormals[2] = rotateVecWithQuat(planeNormals[2], planeRotation * plane3VertRot);

        for (int i = 0; i < copy.Length; i++)
        {
            copyDEBUG[i].position = copy[i];
        }

        //DEBUG
        Debug.DrawRay(copy[0], planeNormals[0] * 30, Color.red);
        Debug.DrawRay(copy[2], planeNormals[1] * 30, Color.red);
        Debug.DrawRay(copy[3], planeNormals[2] * 30, Color.red);

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

    //Projeccio del punt al pla (retorna el mateix punt si ja estava al pla) sense tenir en compte les distances (la distancia fins el pointInPlane pot ser qualsevol)
    //http://geomalgorithms.com/a05-_intersect-1.html
    Vector3 getProjectionToPlane(Vector3 planeNormal, Vector3 pointInPlane, Vector3 pointToProject)
    {
        Vector3 pointInLine = pointToProject - planeNormal;         //--------------------------segurament l'error esta en el Vector3.up
        float escalar = Vector3.Dot(planeNormal.normalized, (pointInPlane - pointToProject)) /
                    (Vector3.Dot(planeNormal.normalized, (pointInLine - pointToProject)));
        return (pointToProject + escalar * (-planeNormal));
    }

    void updatePlaneRotations()//---------------------------------------------------------------------------DELETE (i el atribut plane rotations)
    {
        for (int i = 0; i <= 0; i++)    //CANVIAR PER planes.Lenght
        {
            planes[i].rotation = planeCopyRotations[i];
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


    Vector3 rotateVecWithQuat(Vector3 vec, Quaternion rotation)
    {
        Quaternion vecQuat = new Quaternion(vec.x, vec.y, vec.z, 0);
        Quaternion result = rotation * vecQuat * Quaternion.Inverse(rotation);
        return new Vector3(result.x, result.y, result.z);
    }
    */
}