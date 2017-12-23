using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuaternion : MonoBehaviour {

    public float w, x, y, z;

    public MyQuaternion(float w1,float x1,float y1,float z1)
    {
        w = w1;
        x = x1;
        y = y1;
        z = z1;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    MyQuaternion Invert(MyQuaternion q)
    {
        float sum = Mathf.Pow(q.w, 2) + Mathf.Pow(q.x, 2) + 
            Mathf.Pow(q.y, 2) + Mathf.Pow(q.z, 2);
        MyQuaternion q2 = new MyQuaternion(q.w/sum, -q.x/sum, -q.y/sum, -q.z/sum);
        return q2;
    }
        
    static MyQuaternion Multiply(MyQuaternion q1, MyQuaternion q2)
    {
        MyQuaternion result = new MyQuaternion(q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z,
                                               q1.w * q2.x + q1.x * q2.w - q1.y * q2.z + q1.z * q2.y,
                                               q1.w * q2.y + q1.x * q2.y + q1.y * q2.w - q1.z * q2.x,
                                               q1.w * q2.z - q1.x * q2.y + q1.y * q2.x + q1.z * q2.w
                                               );
        return result;     
    }

    Vector3 ConvertToAxisAngle(MyQuaternion q,Vector3 axis)
    {
        float angle = 2 * Mathf.Acos(q.w);

        axis.x = q.x / Mathf.Sqrt(1 - q.w * q.w);
        axis.y = q.y / Mathf.Sqrt(1 - q.w * q.w);
        axis.z = q.z / Mathf.Sqrt(1 - q.w * q.w);

        return axis;

    }

    //MyQuaternion setQuaternion(MyQuaternion q)
    //{

    //}

    public float Length()
    {
        float l = Mathf.Sqrt(Mathf.Pow(this.w, 2) + Mathf.Pow(this.x, 2) +
        Mathf.Pow(this.y, 2) + Mathf.Pow(this.z, 2));
    
        return l;
    }

    public MyQuaternion Normalize()
    {
        float magnitud = this.Length();
        MyQuaternion result1 = new MyQuaternion(this.w/magnitud, this.x/magnitud,
            this.y/magnitud, this.z/magnitud);

        return result1;
    }



    //static string valores()
    //{
    //    return *"" +w+" "+x+" "+y+" "+z(;

    //}
}
