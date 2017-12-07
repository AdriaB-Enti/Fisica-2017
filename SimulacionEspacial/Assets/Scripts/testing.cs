using System.Collections;
using myClasses;

public class testing : UnityEngine.MonoBehaviour
{
    
	//Per comprovar ràpidament que el Vector 3 no té errors
	void Start () {
        Vector3 prova = new Vector3(0);
        UnityEngine.Vector3 vectorUnity = new UnityEngine.Vector3();
        float dot1 = UnityEngine.Vector3.Dot(new UnityEngine.Vector3(1, 2, 3), new UnityEngine.Vector3(8, 9, 7));
        float dot2 = Vector3.Dot(new Vector3(1, 2, 3), new Vector3(8, 9, 7));

        if (dot1 == dot2)
        {
            print("Dot is alright");
        }

        prova = Vector3.Cross(new Vector3(1, 2, 3), new Vector3(8, 9, 7));
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

        prova = Vector3.Cross(new Vector3(0, 1, 0), new Vector3(0, 0, 1));
        vectorUnity = UnityEngine.Vector3.Cross(new UnityEngine.Vector3(0, 1, 0), new UnityEngine.Vector3(0, 0, 1));
        if (prova.x == vectorUnity.x && prova.y == vectorUnity.y && prova.z == vectorUnity.z)
        {
            print("Cross (2) is alright");
        }
        prova.printValues();

        //testing del Matrix4
        Matrix4 mat = new Matrix4();
        print(mat.matrix[0, 0]);



    }

    // Update is called once per frame
    void Update () {
	
	}
}
