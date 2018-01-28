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

        public Matrix3 getTransposed()
        {
            Matrix3 transposedMat = new Matrix3();
            for (int r = 0; r < matrix.GetLength(0); r++)
            {
                for (int c = 0; c < matrix.GetLength(1); c++)
                {
                    transposedMat.matrix[r,c] = matrix[c,r];
                }
            }
            return transposedMat;
        }

        //Mat3x3 * Mat3x3
        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 result = new Matrix3();

            for (int r = 0; r < m1.matrix.GetLength(0); r++)
            {
                for (int c = 0; c < m1.matrix.GetLength(1); c++)
                {
                    for (int i = 0; i < m1.matrix.GetLength(0); i++)    //sumem les mult. de files per columnes
                    {
                        result.matrix[r, c] += m1.matrix[r,i] * m2.matrix[i,c];
                    }
                }
            }

            return result;
        }

        //Mat3x3 * myVector3----------------------HI HA ALGUN ERROR EN EL CODI
        public static Vector3 operator *(Matrix3 m1, myVector3 vector3)   //TODO: CAMBIAR PER MYVECTOR3-----------------------------------------------------------------
        {
            myVector3 result = new myVector3(0);

            for (int r = 0; r < m1.matrix.GetLength(0); r++)
            {
                for (int i = 0; i < 3; i++)    //Columna de la matriu i del vector3
                {
                    result.setByIndex(r, (result.getByIndex(r) + m1.matrix[r, i] * vector3.getByIndex(i)));
                }
            }

            return result.toUnityVector3();//------------------------------
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
            string text = "";
            for (int r = 0; r < matrix.GetLength(0); r++)
            {
                for (int c = 0; c < matrix.GetLength(1); c++)
                {
                    text += matrix[r, c];
                    if (c < 2)
                    {
                        text += ", ";
                    }
                }
                text += "\n";
            }
            UnityEngine.MonoBehaviour.print(text);
        }
    }
}