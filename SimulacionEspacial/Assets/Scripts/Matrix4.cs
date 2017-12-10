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

        public float determinant()
        {
            return  matrix[0, 0] * getMat3From(0, 0).determinant() -
                    matrix[0, 1] * getMat3From(0, 1).determinant() +
                    matrix[0, 2] * getMat3From(0, 2).determinant() -
                    matrix[0, 3] * getMat3From(0, 3).determinant();
        }
        //inverse

        //isIdentity

        //rotation

        //transpose

        //print

        //operadors

        //Retorna una matriu 3x3 sense la fila i la columna que li passem
        //Si col o row són més grans que 3, donarà error
        public Matrix3 getMat3From(byte row, byte col)
        {
            Matrix3 mat3 = new Matrix3();
            int nr = 0;
            int nc = 0; //new row i new colum (index de la Matrix3)

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (r!= row && c != col)
                    {
                        mat3.matrix[nr, nc] = matrix[r,c];

                    }
                    if (c != col)
                    {
                        nc++;
                    }
                }

                if (r != row)
                {
                    nr++;
                }
                nc = 0;
            }

            return mat3;
        }

    }

    
}
