using System.Collections;
using System;

namespace myClasses
{

    public class myVector3{
        public float x, y, z;

        //Constructors
        public myVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public myVector3() : this(0, 0, 0) { }
        public myVector3(float n) : this(n, n, n) { }

        //Methods
        public float modulus()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
        public void normalize()
        {
            float magnitude = modulus();
            this.x /= magnitude;
            this.y /= magnitude;
            this.z /= magnitude;
        }
        public void scale(float s)
        {
            this.x *= s;
            this.y *= s;
            this.z *= s;
        }
        //Dot product
        public static float Dot(myVector3 a, myVector3 b)
        {
            return (a.x * b.x + a.y * b.y + a.z * b.z);
        }
        //Cross Product
        public static myVector3 Cross(myVector3 a, myVector3 b)
        {
            return new myVector3( (a.y*b.z-a.z*b.y), (a.z*b.x-a.x*b.z), (a.x*b.y-a.y*b.x) );
        }
        //Conversion to myQuaternion (with w=0)
        public MyQuaternion toMyQuaternion()
        {
            return new MyQuaternion(x,y,z,0);
        }

        public void printValues()
        {
            UnityEngine.MonoBehaviour.print("X: " + x + " Y: " + y + " Z: " + z);
        }
        public String getString()
        {
            
            return ("X: " + String.Format("{0:0.00}", x) + " Y: " + String.Format("{0:0.00}", y) + " Z: " + String.Format("{0:0.00}", z));
        }
        //Per anar fent proves si necessitem un vector3 de Unity
        public UnityEngine.Vector3 toUnityVector3()
        {
            return new UnityEngine.Vector3(x,y,z);
        }

        //Si el index és mes gran que 1 o més petit que 0 -> sempre tornara z
        public float getByIndex(int index)
        {
            if (index==0)
            {
                return x;
            }
            else if (index == 1)
            {
                return y;
            }
            else
            {
                return z;
            }
        }
        //Si el index és mes gran que 1 o més petit que 0 -> sempre es setejarà z
        public void setByIndex(int index, float value)
        {
            if (index == 0)
            {
                x = value;
            }
            else if (index == 1)
            {
                y = value;
            }
            else
            {
                z = value;
            }
        }

        public static myVector3 unityToMyVec(UnityEngine.Vector3 unityVec)
        {
            return new myVector3(unityVec.x, unityVec.y, unityVec.z);
        }

        //Operators
        public static myVector3 operator +(myVector3 a, myVector3 b)
        {
            return new myVector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static myVector3 operator -(myVector3 a, myVector3 b)
        {
            return new myVector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static myVector3 operator *(myVector3 a, float b)
        {
            return new myVector3(a.x * b, a.y * b, a.z * b);
        }
        public static myVector3 operator *(float b, myVector3 a)
        {
            return new myVector3(a.x * b, a.y * b, a.z * b);
        }
        public static myVector3 operator /(myVector3 a, float b)
        {
            return new myVector3(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(myVector3 a, myVector3 b)    //TODO: potser seria més correcte fer un .Equals()...
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;          //han de ser tots iguals
        }
        public static bool operator !=(myVector3 a, myVector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;          //només que hi hagi un de diferent, els vectors seran diferents
        }
    }

}