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

        public bool isIdentity()
        {
            bool identity = true;
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if ((r == c && matrix[r, c] != 1) || (r !=c && matrix[r,c]!=0))
                    {
                        identity = false;
                    }
                }
            }
            return identity;
        }

        public void print()
        {
            string text = "";
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    text += matrix[r, c];
                    if (c < 3)
                    {
                        text += ", ";
                    }
                }
                text += "\n";
            }
            UnityEngine.MonoBehaviour.print(text);
        }

        //inverse
        
        //rotation

        //transpose

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


        public UnityEngine.Matrix4x4 toUnityMatrix()
        {
            UnityEngine.Matrix4x4 result = new UnityEngine.Matrix4x4();
            result.SetRow(0, new Vector4(matrix[0, 0], matrix[0, 1], matrix[0, 2], matrix[0, 3]));
            result.SetRow(1, new Vector4(matrix[1, 0], matrix[1, 1], matrix[1, 2], matrix[1, 3]));
            result.SetRow(2, new Vector4(matrix[2, 0], matrix[2, 1], matrix[2, 2], matrix[2, 3]));
            result.SetRow(3, new Vector4(matrix[3, 0], matrix[3, 1], matrix[3, 2], matrix[3, 3]));

            return result;
        }

    }

    
}
