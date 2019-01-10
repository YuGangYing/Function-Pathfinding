using System;
using UnityEngine;

namespace BlueNoah.Math.FixedPoint
{
    /// <summary>
    /// A vector structure.
    /// </summary>
    [Serializable]
    public struct FixedPointVector3
    {

        private static FixedPointInt64 ZeroEpsilonSq = FixedPointMath.Epsilon;
        internal static FixedPointVector3 InternalZero;
        internal static FixedPointVector3 Arbitrary;

        /// <summary>The X component of the vector.</summary>
        public FixedPointInt64 x;
        /// <summary>The Y component of the vector.</summary>
        public FixedPointInt64 y;
        /// <summary>The Z component of the vector.</summary>
        public FixedPointInt64 z;

        #region Static readonly variables
        /// <summary>
        /// A vector with components (0,0,0);
        /// </summary>
        public static readonly FixedPointVector3 zero;
        /// <summary>
        /// A vector with components (-1,0,0);
        /// </summary>
        public static readonly FixedPointVector3 left;
        /// <summary>
        /// A vector with components (1,0,0);
        /// </summary>
        public static readonly FixedPointVector3 right;
        /// <summary>
        /// A vector with components (0,1,0);
        /// </summary>
        public static readonly FixedPointVector3 up;
        /// <summary>
        /// A vector with components (0,-1,0);
        /// </summary>
        public static readonly FixedPointVector3 down;
        /// <summary>
        /// A vector with components (0,0,-1);
        /// </summary>
        public static readonly FixedPointVector3 back;
        /// <summary>
        /// A vector with components (0,0,1);
        /// </summary>
        public static readonly FixedPointVector3 forward;
        /// <summary>
        /// A vector with components (1,1,1);
        /// </summary>
        public static readonly FixedPointVector3 one;
        /// <summary>
        /// A vector with components 
        /// (FP.MinValue,FP.MinValue,FP.MinValue);
        /// </summary>
        public static readonly FixedPointVector3 MinValue;
        /// <summary>
        /// A vector with components 
        /// (FP.MaxValue,FP.MaxValue,FP.MaxValue);
        /// </summary>
        public static readonly FixedPointVector3 MaxValue;
        #endregion

        #region Private static constructor
        static FixedPointVector3()
        {
            one = new FixedPointVector3(1, 1, 1);
            zero = new FixedPointVector3(0, 0, 0);
            left = new FixedPointVector3(-1, 0, 0);
            right = new FixedPointVector3(1, 0, 0);
            up = new FixedPointVector3(0, 1, 0);
            down = new FixedPointVector3(0, -1, 0);
            back = new FixedPointVector3(0, 0, -1);
            forward = new FixedPointVector3(0, 0, 1);
            MinValue = new FixedPointVector3(FixedPointInt64.MinValue);
            MaxValue = new FixedPointVector3(FixedPointInt64.MaxValue);
            Arbitrary = new FixedPointVector3(1, 1, 1);
            InternalZero = zero;
        }
        #endregion

        public static FixedPointVector3 Abs(FixedPointVector3 other) {
            return new FixedPointVector3(FixedPointInt64.Abs(other.x), FixedPointInt64.Abs(other.y), FixedPointInt64.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        public FixedPointInt64 sqrMagnitude {
            get { 
                return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z));
            }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        public FixedPointInt64 magnitude {
            get {
                FixedPointInt64 num = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
                return FixedPointInt64.Sqrt(num);
            }
        }

        public static FixedPointVector3 ClampMagnitude(FixedPointVector3 vector, FixedPointInt64 maxLength) {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        public FixedPointVector3 normalized {
            get {
                FixedPointVector3 result = new FixedPointVector3(this.x, this.y, this.z);
                result.Normalize();

                return result;
            }
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>

        public FixedPointVector3(int x,int y,int z)
		{
			this.x = (FixedPointInt64)x;
			this.y = (FixedPointInt64)y;
			this.z = (FixedPointInt64)z;
		}

		public FixedPointVector3(FixedPointInt64 x, FixedPointInt64 y, FixedPointInt64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(FixedPointVector3 other) {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public void Set(FixedPointInt64 x, FixedPointInt64 y, FixedPointInt64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public FixedPointVector3(FixedPointInt64 xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }

		public static FixedPointVector3 Lerp(FixedPointVector3 from, FixedPointVector3 to, FixedPointInt64 percent) {
			return from + (to - from) * percent;
		}

        /// <summary>
        /// Builds a string from the JVector.
        /// </summary>
        /// <returns>A string containing all three components.</returns>
        #region public override string ToString()
        public override string ToString() {
            return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
        }
        #endregion

        /// <summary>
        /// Tests if an object is equal to this vector.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if they are euqal, otherwise false.</returns>
        #region public override bool Equals(object obj)
        public override bool Equals(object obj)
        {
            if (!(obj is FixedPointVector3)) return false;
            FixedPointVector3 other = (FixedPointVector3)obj;

            return (((x == other.x) && (y == other.y)) && (z == other.z));
        }
        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public static FixedPointVector3 Scale(FixedPointVector3 vecA, FixedPointVector3 vecB) {
            FixedPointVector3 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;

            return result;
        }

        /// <summary>
        /// Tests if two JVector are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>
        #region public static bool operator ==(JVector value1, JVector value2)
        public static bool operator ==(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }
        #endregion

        /// <summary>
        /// Tests if two JVector are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>
        #region public static bool operator !=(JVector value1, JVector value2)
        public static bool operator !=(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>
        #region public static JVector Min(JVector value1, JVector value2)

        public static FixedPointVector3 Min(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result;
            FixedPointVector3.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
        public static void Min(ref FixedPointVector3 value1, ref FixedPointVector3 value2, out FixedPointVector3 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>
        #region public static JVector Max(JVector value1, JVector value2)
        public static FixedPointVector3 Max(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result;
            FixedPointVector3.Max(ref value1, ref value2, out result);
            return result;
        }
		
		public static FixedPointInt64 Distance(FixedPointVector3 v1, FixedPointVector3 v2) {
			return FixedPointInt64.Sqrt ((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
		}

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
        public static void Max(ref FixedPointVector3 value1, ref FixedPointVector3 value2, out FixedPointVector3 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Sets the length of the vector to zero.
        /// </summary>
        #region public void MakeZero()
        public void MakeZero()
        {
            x = FixedPointInt64.Zero;
            y = FixedPointInt64.Zero;
            z = FixedPointInt64.Zero;
        }
        #endregion

        /// <summary>
        /// Checks if the length of the vector is zero.
        /// </summary>
        /// <returns>Returns true if the vector is zero, otherwise false.</returns>
        #region public bool IsZero()
        public bool IsZero()
        {
            return (this.sqrMagnitude == FixedPointInt64.Zero);
        }

        /// <summary>
        /// Checks if the length of the vector is nearly zero.
        /// </summary>
        /// <returns>Returns true if the vector is nearly zero, otherwise false.</returns>
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }
        #endregion

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed vector.</returns>
        #region public static JVector Transform(JVector position, JMatrix matrix)
        public static FixedPointVector3 Transform(FixedPointVector3 position, FixedPointMatrix matrix)
        {
            FixedPointVector3 result;
            FixedPointVector3.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void Transform(ref FixedPointVector3 position, ref FixedPointMatrix matrix, out FixedPointVector3 result)
        {
            FixedPointInt64 num0 = ((position.x * matrix.M11) + (position.y * matrix.M21)) + (position.z * matrix.M31);
            FixedPointInt64 num1 = ((position.x * matrix.M12) + (position.y * matrix.M22)) + (position.z * matrix.M32);
            FixedPointInt64 num2 = ((position.x * matrix.M13) + (position.y * matrix.M23)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Transforms a vector by the transposed of the given Matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransposedTransform(ref FixedPointVector3 position, ref FixedPointMatrix matrix, out FixedPointVector3 result)
        {
            FixedPointInt64 num0 = ((position.x * matrix.M11) + (position.y * matrix.M12)) + (position.z * matrix.M13);
            FixedPointInt64 num1 = ((position.x * matrix.M21) + (position.y * matrix.M22)) + (position.z * matrix.M23);
            FixedPointInt64 num2 = ((position.x * matrix.M31) + (position.y * matrix.M32)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        #region public static FP Dot(JVector vector1, JVector vector2)
        public static FixedPointInt64 Dot(FixedPointVector3 vector1, FixedPointVector3 vector2)
        {
            return FixedPointVector3.Dot(ref vector1, ref vector2);
        }


        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static FixedPointInt64 Dot(ref FixedPointVector3 vector1, ref FixedPointVector3 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
        }
        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static void Add(JVector value1, JVector value2)
        public static FixedPointVector3 Add(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result;
            FixedPointVector3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
        public static void Add(ref FixedPointVector3 value1, ref FixedPointVector3 value2, out FixedPointVector3 result)
        {
            FixedPointInt64 num0 = value1.x + value2.x;
            FixedPointInt64 num1 = value1.y + value2.y;
            FixedPointInt64 num2 = value1.z + value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixedPointVector3 Divide(FixedPointVector3 value1, FixedPointInt64 scaleFactor) {
            FixedPointVector3 result;
            FixedPointVector3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref FixedPointVector3 value1, FixedPointInt64 scaleFactor, out FixedPointVector3 result) {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector Subtract(JVector value1, JVector value2)
        public static FixedPointVector3 Subtract(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result;
            FixedPointVector3.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
        public static void Subtract(ref FixedPointVector3 value1, ref FixedPointVector3 value2, out FixedPointVector3 result)
        {
            FixedPointInt64 num0 = value1.x - value2.x;
            FixedPointInt64 num1 = value1.y - value2.y;
            FixedPointInt64 num2 = value1.z - value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of both vectors.</returns>
        #region public static JVector Cross(JVector vector1, JVector vector2)
        public static FixedPointVector3 Cross(FixedPointVector3 vector1, FixedPointVector3 vector2)
        {
            FixedPointVector3 result;
            FixedPointVector3.Cross(ref vector1, ref vector2, out result);
            return result;
        }

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of both vectors.</param>
        public static void Cross(ref FixedPointVector3 vector1, ref FixedPointVector3 vector2, out FixedPointVector3 result)
        {
            FixedPointInt64 num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            FixedPointInt64 num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            FixedPointInt64 num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }
        #endregion

        /// <summary>
        /// Gets the hashcode of the vector.
        /// </summary>
        /// <returns>Returns the hashcode of the vector.</returns>
        #region public override int GetHashCode()
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        #endregion

        /// <summary>
        /// Inverses the direction of the vector.
        /// </summary>
        #region public static JVector Negate(JVector value)
        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static FixedPointVector3 Negate(FixedPointVector3 value)
        {
            FixedPointVector3 result;
            FixedPointVector3.Negate(ref value,out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
        public static void Negate(ref FixedPointVector3 value, out FixedPointVector3 result)
        {
            FixedPointInt64 num0 = -value.x;
            FixedPointInt64 num1 = -value.y;
            FixedPointInt64 num2 = -value.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>
        #region public static JVector Normalize(JVector value)
        public static FixedPointVector3 Normalize(FixedPointVector3 value)
        {
            FixedPointVector3 result;
            FixedPointVector3.Normalize(ref value, out result);
            return result;
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            FixedPointInt64 num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
            FixedPointInt64 num = FixedPointInt64.One / FixedPointInt64.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
        public static void Normalize(ref FixedPointVector3 value, out FixedPointVector3 result)
        {
            FixedPointInt64 num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z);
            FixedPointInt64 num = FixedPointInt64.One / FixedPointInt64.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
        }
        #endregion

        #region public static void Swap(ref JVector vector1, ref JVector vector2)

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
        public static void Swap(ref FixedPointVector3 vector1, ref FixedPointVector3 vector2)
        {
            FixedPointInt64 temp;

            temp = vector1.x;
            vector1.x = vector2.x;
            vector2.x = temp;

            temp = vector1.y;
            vector1.y = vector2.y;
            vector2.y = temp;

            temp = vector1.z;
            vector1.z = vector2.z;
            vector2.z = temp;
        }
        #endregion

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>
        #region public static JVector Multiply(JVector value1, FP scaleFactor)
        public static FixedPointVector3 Multiply(FixedPointVector3 value1, FixedPointInt64 scaleFactor)
        {
            FixedPointVector3 result;
            FixedPointVector3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
        public static void Multiply(ref FixedPointVector3 value1, FixedPointInt64 scaleFactor, out FixedPointVector3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }
        #endregion

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the cross product of both.</returns>
        #region public static JVector operator %(JVector value1, JVector value2)
        public static FixedPointVector3 operator %(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result; FixedPointVector3.Cross(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>
        #region public static FP operator *(JVector value1, JVector value2)
        public static FixedPointInt64 operator *(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            return FixedPointVector3.Dot(ref value1, ref value2);
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(JVector value1, FP value2)
        public static FixedPointVector3 operator *(FixedPointVector3 value1, FixedPointInt64 value2)
        {
            FixedPointVector3 result;
            FixedPointVector3.Multiply(ref value1, value2,out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(FP value1, JVector value2)
        public static FixedPointVector3 operator *(FixedPointInt64 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result;
            FixedPointVector3.Multiply(ref value2, value1, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector operator -(JVector value1, JVector value2)
        public static FixedPointVector3 operator -(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result; FixedPointVector3.Subtract(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static JVector operator +(JVector value1, JVector value2)
        public static FixedPointVector3 operator +(FixedPointVector3 value1, FixedPointVector3 value2)
        {
            FixedPointVector3 result; FixedPointVector3.Add(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixedPointVector3 operator /(FixedPointVector3 value1, FixedPointInt64 value2) {
            FixedPointVector3 result;
            FixedPointVector3.Divide(ref value1, value2, out result);
            return result;
        }

        public static FixedPointInt64 Angle(FixedPointVector3 a, FixedPointVector3 b) {
            return FixedPointInt64.Acos(a.normalized * b.normalized) * FixedPointInt64.Rad2Deg;
        }

        public FixedPointVector2 ToTSVector2() {
            return new FixedPointVector2(this.x, this.y);
        }

    }

}