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
            matrix = new float[,] { { n, n, n }, { n, n, n }, { n, n, n }, { n, n, n } };
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