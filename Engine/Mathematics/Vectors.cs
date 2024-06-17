using System.Diagnostics.CodeAnalysis;

namespace IBX_Engine.Mathematics
{
    /// <summary>
    /// A 2D vector
    /// </summary>
    /// <param name="x">The X component</param>
    /// <param name="y">The Y component</param>
    public struct Vec2(float x, float y) : IEquatable<Vec2>
    {
        public float X = x, Y = y;

        // Operator overloads for vector on scalar operations
        public static Vec2 operator +(Vec2 left, float right) => new(left.X + right, left.Y + right);
        public static Vec2 operator +(float left, Vec2 right) => new(left + right.X, left + right.Y);
        public static Vec2 operator -(Vec2 left, float right) => new(left.X - right, left.Y - right);
        public static Vec2 operator -(float left, Vec2 right) => new(left - right.X, left - right.Y);
        public static Vec2 operator *(Vec2 left, float right) => new(left.X * right, left.Y * right);
        public static Vec2 operator *(float left, Vec2 right) => new(left * right.X, left * right.Y);
        public static Vec2 operator /(Vec2 left, float right) => new(left.X / right, left.Y / right);
        public static Vec2 operator /(float left, Vec2 right) => new(left / right.X, left / right.Y);
        public static Vec2 operator %(Vec2 left, float right) => new(left.X % right, left.Y % right);
        public static Vec2 operator %(float left, Vec2 right) => new(left % right.X, left % right.Y);

        // Operator overloads for vector on vector operations
        public static Vec2 operator +(Vec2 left, Vec2 right) => new(left.X + right.X, left.Y + right.Y);
        public static Vec2 operator -(Vec2 left, Vec2 right) => new(left.X - right.X, left.Y - right.Y);
        public static Vec2 operator *(Vec2 left, Vec2 right) => new(left.X * right.X, left.Y * right.Y);
        public static Vec2 operator /(Vec2 left, Vec2 right) => new(left.X / right.X, left.Y / right.Y);
        public static Vec2 operator %(Vec2 left, Vec2 right) => new(left.X % right.X, left.Y % right.Y);

        // Operator overloads for vector on vector comparison, only equality checks are possible
        public static bool operator ==(Vec2 left, Vec2 right) => left.Equals(right);
        public static bool operator !=(Vec2 left, Vec2 right) => !left.Equals(right);

        // Predefined vectors
        public static Vec2 Zero => new(0, 0);
        public static Vec2 One => new(1, 1);
        public static Vec2 Right => new(1, 0);
        public static Vec2 Up => new(0, 1);

        // Magnitude and normalization
        public readonly float Magnitude => MathF.Sqrt((X * X) + (Y * Y));
        public readonly Vec2 Normalized => this / Magnitude;

        // Overrides to stop the compiler from complaining
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);
        public override readonly int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Check if two vectors are equal
        /// </summary>
        /// <param name="other">The other vector to check against</param>
        /// <returns>Returns true if the vectors are equal</returns>
        public readonly bool Equals(Vec2 other) => X == other.X && Y == other.Y;

        /// <summary>
        /// Retrieve a OpenTK compatible vector
        /// </summary>
        /// <returns>An OpenTK compatible vector</returns>
        internal readonly OpenTK.Mathematics.Vector2 AsOpenTKVector() => new(X, Y);

        /// <summary>
        /// Retrieve a System.Numerics compatible vector
        /// </summary>
        /// <returns>A System.Numerics compatible vector</returns>
        internal readonly System.Numerics.Vector2 AsSystemVector() => new(X, Y);
    }

    /// <summary>
    /// A 3D vector
    /// </summary>
    /// <param name="x">The X component</param>
    /// <param name="y">The Y component</param>
    /// <param name="z">The Z component</param>
    public struct Vec3(float x, float y, float z) : IEquatable<Vec3>
    {
        public float X = x, Y = y, Z = z;

        // Operator overloads for vector on scalar operations
        public static Vec3 operator +(Vec3 left, float right) => new(left.X + right, left.Y + right, left.Z + right);
        public static Vec3 operator +(float left, Vec3 right) => new(left + right.X, left + right.Y, left + right.Z);
        public static Vec3 operator -(Vec3 left, float right) => new(left.X - right, left.Y - right, left.Z - right);
        public static Vec3 operator -(float left, Vec3 right) => new(left - right.X, left - right.Y, left - right.Z);
        public static Vec3 operator *(Vec3 left, float right) => new(left.X * right, left.Y * right, left.Z * right);
        public static Vec3 operator *(float left, Vec3 right) => new(left * right.X, left * right.Y, left * right.Z);
        public static Vec3 operator /(Vec3 left, float right) => new(left.X / right, left.Y / right, left.Z / right);
        public static Vec3 operator /(float left, Vec3 right) => new(left / right.X, left / right.Y, left / right.Z);
        public static Vec3 operator %(Vec3 left, float right) => new(left.X % right, left.Y % right, left.Z % right);
        public static Vec3 operator %(float left, Vec3 right) => new(left % right.X, left % right.Y, left % right.Z);

        // Operator overloads for vector on vector operations
        public static Vec3 operator +(Vec3 left, Vec3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        public static Vec3 operator -(Vec3 left, Vec3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        public static Vec3 operator *(Vec3 left, Vec3 right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        public static Vec3 operator /(Vec3 left, Vec3 right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        public static Vec3 operator %(Vec3 left, Vec3 right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);

        // Operator overloads for vector on vector comparison, only equality checks are possible
        public static bool operator ==(Vec3 left, Vec3 right) => left.Equals(right);
        public static bool operator !=(Vec3 left, Vec3 right) => !left.Equals(right);

        // Predefined vectors
        public static Vec3 Zero => new(0, 0, 0);
        public static Vec3 One => new(1, 1, 1);
        public static Vec3 Right => new(1, 0, 0);
        public static Vec3 Up => new(0, 1, 0);
        public static Vec3 Forward => new(0, 0, 1);

        // Magnitude and normalization
        public readonly float Magnitude => MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
        public readonly Vec3 Normalized => this / Magnitude;

        // Overrides to stop the compiler from complaining
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);
        public override readonly int GetHashCode() => base.GetHashCode();

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

    /// <summary>
    /// A 4D vector
    /// </summary>
    /// <param name="x">The X component</param>
    /// <param name="y">The Y component</param>
    /// <param name="z">The Z component</param>
    /// <param name="w">The W component</param>
    public struct Vec4(float x, float y, float z, float w) : IEquatable<Vec4>
    {
        public float X = x, Y = y, Z = z, W = w;

        // Operator overloads for vector on scalar operations
        public static Vec4 operator +(Vec4 left, float right) => new(left.X + right, left.Y + right, left.Z + right, left.W + right);
        public static Vec4 operator +(float left, Vec4 right) => new(left + right.X, left + right.Y, left + right.Z, left + right.W);
        public static Vec4 operator -(Vec4 left, float right) => new(left.X - right, left.Y - right, left.Z - right, left.W - right);
        public static Vec4 operator -(float left, Vec4 right) => new(left - right.X, left - right.Y, left - right.Z, left - right.W);
        public static Vec4 operator *(Vec4 left, float right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
        public static Vec4 operator *(float left, Vec4 right) => new(left * right.X, left * right.Y, left * right.Z, left * right.W);
        public static Vec4 operator /(Vec4 left, float right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);
        public static Vec4 operator /(float left, Vec4 right) => new(left / right.X, left / right.Y, left / right.Z, left / right.W);
        public static Vec4 operator %(Vec4 left, float right) => new(left.X % right, left.Y % right, left.Z % right, left.W % right);
        public static Vec4 operator %(float left, Vec4 right) => new(left % right.X, left % right.Y, left % right.Z, left % right.W);

        // Operator overloads for vector on vector operations
        public static Vec4 operator +(Vec4 left, Vec4 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        public static Vec4 operator -(Vec4 left, Vec4 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        public static Vec4 operator *(Vec4 left, Vec4 right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
        public static Vec4 operator /(Vec4 left, Vec4 right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
        public static Vec4 operator %(Vec4 left, Vec4 right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z, left.W % right.W);

        // Operator overloads for vector on vector comparison, only equality checks are possible
        public static bool operator ==(Vec4 left, Vec4 right) => left.Equals(right);
        public static bool operator !=(Vec4 left, Vec4 right) => !left.Equals(right);


        // Predefined vectors
        public static Vec4 Zero => new(0, 0, 0, 0);
        public static Vec4 One => new(1, 1, 1, 1);
        public static Vec4 Right => new(1, 0, 0, 0);
        public static Vec4 Up => new(0, 1, 0, 0);
        public static Vec4 Forward => new(0, 0, 1, 0);
        public static Vec4 ForwardW => new(0, 0, 0, 1);

        // Magnitude and normalization
        public readonly float Magnitude => MathF.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        public readonly Vec4 Normalized => this / Magnitude;

        // Overrides to stop the compiler from complaining
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);
        public override readonly int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Check if two vectors are equal
        /// </summary>
        /// <param name="other">The other vector to check against</param>
        /// <returns>Returns true if the vectors are equal</returns>
        public readonly bool Equals(Vec4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        /// <summary>
        /// Retrieve a OpenTK compatible vector
        /// </summary>
        /// <returns>An OpenTK compatible vector</returns>
        internal readonly OpenTK.Mathematics.Vector4 AsOpenTKVector() => new(X, Y, Z, W);

        /// <summary>
        /// Retrieve a System.Numerics compatible vector
        /// </summary>
        /// <returns>A System.Numerics compatible vector</returns>
        internal readonly System.Numerics.Vector4 AsSystemVector() => new(X, Y, Z, W);
    }
}
