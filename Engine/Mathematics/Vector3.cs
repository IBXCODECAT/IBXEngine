namespace IBX_Engine.Mathematics
{
    public struct Vec3(float x, float y, float z) : IEquatable<Vec3>
    {
        public float X = x, Y = y, Z = z;

        // Operator overloads for vector on scalar operations
        public static Vec3 operator + (Vec3 left, float right) => new Vec3(left.X + right, left.Y + right, left.Z + right);
        public static Vec3 operator + (float left, Vec3 right) => new Vec3(left + right.X, left + right.Y, left + right.Z);
        public static Vec3 operator - (Vec3 left, float right) => new Vec3(left.X - right, left.Y - right, left.Z - right);
        public static Vec3 operator - (float left, Vec3 right) => new Vec3(left - right.X, left - right.Y, left - right.Z);
        public static Vec3 operator * (Vec3 left, float right) => new Vec3(left.X * right, left.Y * right, left.Z * right);
        public static Vec3 operator * (float left, Vec3 right) => new Vec3(left * right.X, left * right.Y, left * right.Z);
        public static Vec3 operator / (Vec3 left, float right) => new Vec3(left.X / right, left.Y / right, left.Z / right);
        public static Vec3 operator / (float left, Vec3 right) => new Vec3(left / right.X, left / right.Y, left / right.Z);
        public static Vec3 operator % (Vec3 left, float right) => new Vec3(left.X % right, left.Y % right, left.Z % right);
        public static Vec3 operator % (float left, Vec3 right) => new Vec3(left % right.X, left % right.Y, left % right.Z);

        // Operator overloads for vector on vector operations
        public static Vec3 operator + (Vec3 left, Vec3 right) => new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        public static Vec3 operator - (Vec3 left, Vec3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        public static Vec3 operator * (Vec3 left, Vec3 right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        public static Vec3 operator / (Vec3 left, Vec3 right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        public static Vec3 operator % (Vec3 left, Vec3 right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);

        // Operator overloads for vector on vector comparison, only equality checks are possible
        public static bool operator ==(Vec3 left, Vec3 right) => left.Equals(right);
        public static bool operator !=(Vec3 left, Vec3 right) => !left.Equals(right);

        public static Vec3 Zero => new(0, 0, 0);
        public static Vec3 One => new(1, 1, 1);
        public static Vec3 Up => new(0, 1, 0);
        public static Vec3 Right => new(1, 0, 0);
        public static Vec3 Forward => new(0, 0, 1);

        public readonly float Magnitude => MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
        public readonly Vec3 Normalized => this / Magnitude;

        /// <summary>
        /// Check if two vectors are equal
        /// </summary>
        /// <param name="other">The other vector to check against</param>
        /// <returns>Returns true if the vectors are equal</returns>
        public readonly bool Equals(Vec3 other) => X == other.X && Y == other.Y && Z == other.Z;

        /// <summary>
        /// Retrieve a OpenTK compatible vector
        /// </summary>
        /// <returns>An OpenTK compatible vector</returns>
        internal readonly OpenTK.Mathematics.Vector3 AsOpenTKVector() => new(X, Y, Z);

        /// <summary>
        /// Retrieve a System.Numerics compatible vector
        /// </summary>
        /// <returns>A System.Numerics compatible vector</returns>
        internal readonly System.Numerics.Vector3 AsSystemVector() => new(X, Y, Z);
    }
}