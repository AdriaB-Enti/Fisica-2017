using System.Collections;
using System;

namespace myClasses
{

    public class Vector3{
        public float x, y, z;

        //Constructors
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3() : this(0, 0, 0) { }
        public Vector3(float n) : this(n, n, n) { }

        private static Vector3 up()
        {
            return new Vector3(0, 1, 0);
        }
        private static Vector3 left()
        {
            return new Vector3(0, 0, 1);
        }
        private static Vector3 front()
        {
            return new Vector3(1, 0, 0);
        }

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
        public static float Dot(Vector3 a, Vector3 b)
        {
            return (a.x * b.x + a.y * b.y + a.z * b.z);
        }
        //Cross Product
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3( (a.y*b.z-a.z*b.y), (a.z*b.x-a.x*b.z), (a.x*b.y-a.y*b.x) );
        }

        //Operators
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }
        public static Vector3 operator /(Vector3 a, float b)
        {
            return new Vector3(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;      //han de ser tots iguals
        }
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;      //només que hi hagi un de diferent, els vectors seran diferents
        }
    }

}