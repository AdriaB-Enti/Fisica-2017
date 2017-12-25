using System.Collections;
using System.Collections.Generic;

namespace myClasses
{

    public class MyQuaternion{

        public float w, x, y, z;

        public MyQuaternion(float w1,float x1,float y1,float z1)
        {
            w = w1;
            x = x1;
            y = y1;
            z = z1;
        }


        MyQuaternion Invert(MyQuaternion q)
        {
            float sum = q.w*q.w + q.x*q.x + q.y*q.y + q.z*q.z;
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
            float angle = 2 * UnityEngine.Mathf.Acos(q.w);

            axis.x = q.x / UnityEngine.Mathf.Sqrt(1 - q.w * q.w);
            axis.y = q.y / UnityEngine.Mathf.Sqrt(1 - q.w * q.w);
            axis.z = q.z / UnityEngine.Mathf.Sqrt(1 - q.w * q.w);

            return axis;

        }

        //MyQuaternion setQuaternion(MyQuaternion q)
        //{

        //}

        public float Length()
        {
            return UnityEngine.Mathf.Sqrt(this.w * this.w + this.x * this.x + this.y * this.y
                + this.z * this.z);
        }

        public MyQuaternion Normalize()
        {
            float magnitud = this.Length();
            MyQuaternion result1 = new MyQuaternion(this.w/magnitud, this.x/magnitud,
                this.y/magnitud, this.z/magnitud);

            return result1;
        }

        //retorna un Quaternion de unity amb els mateixos valors - ideal per fer proves
        public UnityEngine.Quaternion toUnityQuat()
        {
            return new UnityEngine.Quaternion(x,y,z,w);
        }

        public void printValues()
        {
            UnityEngine.MonoBehaviour.print("X: " + x + " Y: " + y + " Z: " + z + " W: " + w);
        }

        public static bool operator ==(MyQuaternion a, MyQuaternion b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }
        public static bool operator !=(MyQuaternion a, MyQuaternion b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }
    }

}