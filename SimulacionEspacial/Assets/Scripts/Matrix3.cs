using UnityEngine;
using System.Collections;

namespace myClasses
{
    public class Matrix3
    {
        public float[,] matrix;

        //Inicialitza a 0
        public Matrix3()
        {
            matrix = new float[3, 3];
        }
        public Matrix3(float n)
        {
            matrix = new float[,] { { n, n, n }, { n, n, n }, { n, n, n }};
        }

        public Matrix3(MyQuaternion quat)
        {
            //TODO----------------------------------------------------------------------
        }

        public Matrix3(UnityEngine.Quaternion quat) //METODE JESUS - Si no va-> provar amb els signes com a internet
        {
            matrix = new float[,] { 
                { 1-2*quat.y*quat.y - 2*quat.z*quat.z,  2*quat.x*quat.y + 2*quat.w*quat.z,        2*quat.x*quat.z - 2*quat.w*quat.y }, 
                { 2*quat.x*quat.y - 2*quat.w*quat.z,  1 - 2*quat.x*quat.x - 2*quat.z*quat.z,    2*quat.y*quat.z + 2*quat.w*quat.x }, 
                { 2*quat.x*quat.z + 2*quat.w*quat.y,  2*quat.y*quat.z - 2*quat.w*quat.x,        1 - 2*quat.x*quat.x -2*quat.y*quat.y } };
        }

        public static Matrix3 iBodyBox(float mass, float height, float width, float depth)
        {
            Matrix3 iBody = new Matrix3();
            iBody.matrix = new float[,] { 
                { (1/12)*mass*(height*height + depth*depth),  0,  0 }, 
                { 0,  (1/12)*mass*(width*width + depth*depth),  0 }, 
                { 0,  0,  (1/12)*mass*(width*width + height*height) } };
            return iBody;
        }

        //TODO: TRANSPOSADA
        public void transpose()
        {

        }

        public float determinant()
        {
            return  matrix[0, 0] * matrix[1, 1] * matrix[2, 2] +
                    matrix[0, 1] * matrix[1, 2] * matrix[2, 0] +
                    matrix[0, 2] * matrix[1, 0] * matrix[2, 1] -
                    matrix[2, 0] * matrix[1, 1] * matrix[0, 2] -
                    matrix[2, 1] * matrix[1, 2] * matrix[0, 0] -
                    matrix[2, 2] * matrix[1, 0] * matrix[0, 1];
        }


        public void print()
        {
            foreach (int item in matrix)
            {
                UnityEngine.MonoBehaviour.print(item);
            }
        }


    }
}