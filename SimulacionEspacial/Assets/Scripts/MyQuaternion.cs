using System.Collections;
using System.Collections.Generic;

namespace myClasses
{

    public struct AxisAngle
    {
        public Vector3 axis;
        public float angle;
    }

    public class MyQuaternion{

        public float x, y, z, w;

        public MyQuaternion(float x1,float y1,float z1,float w1)
        {
            x = x1;
            y = y1;
            z = z1;
            w = w1;
        }

        //Inversa = quaternió normalitzat i trasposat
        public MyQuaternion Invert(MyQuaternion q)
        {
            float sum = q.x*q.x + q.y*q.y + q.z*q.z + q.w*q.w;
            return new MyQuaternion(-q.x/sum, -q.y/sum, -q.z/sum, q.w / sum);
        }
        
        /*
         * TODO: TENIR EN COMPTE ALFA=0 (INFINIT) I ALFA=180 
         */
        public AxisAngle ConvertToAxisAngle()
        {
            AxisAngle result;
            result.angle = 2 * UnityEngine.Mathf.Acos(w);

            Vector3 axis = new Vector3();
            axis.x = x / UnityEngine.Mathf.Sqrt(1 - w * w);
            axis.y = y / UnityEngine.Mathf.Sqrt(1 - w * w);
            axis.z = z / UnityEngine.Mathf.Sqrt(1 - w * w);
            result.axis = axis;

            return result;
        }

        //TODO: axis angle to quaternion

        public float Length()
        {
            return UnityEngine.Mathf.Sqrt(this.w * this.w + this.x * this.x + this.y * this.y
                + this.z * this.z);
        }

        public MyQuaternion Normalize()
        {
            float magnitud = this.Length();
            MyQuaternion result1 = new MyQuaternion(this.x/magnitud, this.y/magnitud, 
                this.z/magnitud, this.w / magnitud);

            return result1;
        }

        //Retorna q* (o lo que és el mateix, el quaternió trasposat)
        public MyQuaternion conjugate()
        {
            return new MyQuaternion(-x,-y,-z,w);
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

        //Retorna el quaternió identitat
        public static MyQuaternion QuatIdentity()
        {
            return new MyQuaternion(0,0,0,1);
        }

        //Operators
        public static MyQuaternion operator *(MyQuaternion q1, MyQuaternion q2)
        {
            return new MyQuaternion(q1.w * q2.x + q1.x * q2.w - q1.y * q2.z + q1.z * q2.y,
                                                   q1.w * q2.y + q1.x * q2.z + q1.y * q2.w - q1.z * q2.x,
                                                   q1.w * q2.z - q1.x * q2.y + q1.y * q2.x + q1.z * q2.w,
                                                   q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
                                                   );
        }

        public static bool operator ==(MyQuaternion a, MyQuaternion b)  //seria més correcte posar-ho en un equals...
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }
        public static bool operator !=(MyQuaternion a, MyQuaternion b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }
    }

}