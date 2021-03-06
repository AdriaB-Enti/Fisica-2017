﻿using System.Collections;
using myClasses;

public class testing : UnityEngine.MonoBehaviour
{
    
	//Per comprovar ràpidament que el Vector 3 no té errors
	void Start () {
        myVector3 prova = new myVector3(0);
        UnityEngine.Vector3 vectorUnity = new UnityEngine.Vector3();
        float dot1 = UnityEngine.Vector3.Dot(new UnityEngine.Vector3(1, 2, 3), new UnityEngine.Vector3(8, 9, 7));
        float dot2 = myVector3.Dot(new myVector3(1, 2, 3), new myVector3(8, 9, 7));

        if (dot1 == dot2)
        {
            print("Dot is alright");
        }

        prova = myVector3.Cross(new myVector3(1, 2, 3), new myVector3(8, 9, 7));
        vectorUnity = UnityEngine.Vector3.Cross(new UnityEngine.Vector3(1, 2, 3), new UnityEngine.Vector3(8, 9, 7));
        if (prova.x==vectorUnity.x && prova.y == vectorUnity.y && prova.z == vectorUnity.z)
        {
            print("Cross is alright");
        }

        float mag1 = prova.modulus();
        float mag2 = vectorUnity.magnitude;

        if (mag1 == mag2)
        {
            print("Magnitude is alright");
        }

        prova.normalize();
        print("Modul..."+prova.modulus());  //....no és del tot 1 (floating point error) no sé si es pot evitar... pot donar problemes
        if (prova.modulus()==1f)
        {
            print("Normalize is alright");

        }

        prova = myVector3.Cross(new myVector3(0, 1, 0), new myVector3(0, 0, 1));
        vectorUnity = UnityEngine.Vector3.Cross(new UnityEngine.Vector3(0, 1, 0), new UnityEngine.Vector3(0, 0, 1));
        if (prova.x == vectorUnity.x && prova.y == vectorUnity.y && prova.z == vectorUnity.z)
        {
            print("Cross (2) is alright");
        }
        prova.printValues();

        //testing del Matrix3
        Matrix3 mat3 = new Matrix3();
        mat3.matrix = new float[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, -8, -9 } };
        mat3.print();
        //myVector3.unityVec3ToMyVec3(mat3 * new myVector3(88, 2, 9)).printValues();
        //(mat3 * mat3.getTransposed()).print();
        print(mat3.determinant() == -42 ? "Determinant Mat3x3 correcte" : "Determinant Mat3x3 incorrecte");

        //Test inversa i cofactors ---- Funciona bé
        //Matrix3 mat33 = new Matrix3();
        //mat33.matrix = new float[,] { { -1, -2, 2 }, { 2, 1, 1 }, { 3, 4, 5 } };
        //mat33.cofactorMat().print();
        //mat33.getInverse().print();


        //Testing del Matrix4
        Matrix4 mat = new Matrix4();
        mat.matrix = new float[,] { { 1, 2, 3 ,4}, 
                                    { 5, 6, 7, -8}, 
                                    { 9, 10, -11, 12 }, 
                                    { 13, 14, -15, -16} };
        print(mat.determinant() == -2432 ? "Determinant Mat4x4 correcte" : "Determinant Mat4x4 incorrecte");
        mat.print();

        //Testing de MyQuaternion
        MyQuaternion quaternion1 = new MyQuaternion(5,8,1,3);
        MyQuaternion quaternion2 = new MyQuaternion(6,6,1,1);
        MyQuaternion quatResult = quaternion1*quaternion2;
        UnityEngine.Quaternion unityQuat = new UnityEngine.Quaternion(5, 8, 1, 3);
        UnityEngine.Quaternion unityQuat2 = new UnityEngine.Quaternion(6, 6, 1, 1);
        UnityEngine.Quaternion unityResult = unityQuat2 * unityQuat;
        print(quatResult.toUnityQuat().Equals(unityResult) ? "MyQuaternion product correcte" : "MyQuaternion product incorrecte");
        //print("X: " + quatResult.x + " Y: " + quatResult.y + " Z: " + quatResult.z + " W: " + quatResult.w);
        //print("X: " + unityResult.x + " Y: " + unityResult.y + " Z: " + unityResult.z + " W: " + unityResult.w);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
