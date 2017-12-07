using UnityEngine;
using System.Collections;

namespace myClasses
{
    public class Matrix4 {
        public float[,] matrix;

        //Inicialitza a 0
        public Matrix4()
        {
            matrix = new float[4, 4];
        }
        public Matrix4(float n)
        {
            matrix = new float[,] { { n, n, n, n }, { n, n, n, n }, { n, n, n, n }, { n, n, n, n } };
        }

        //determinant
        //inverse
        //isIdentity
        //rotation
        //transpose
        //print
        //operadors

    }
}
