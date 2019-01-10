using System;

namespace BlueNoah.Math.FixedPoint {

    [Serializable]
    public struct FixedPointVector2 : IEquatable<FixedPointVector2>
    {
#region Private Fields

        private static FixedPointVector2 zeroVector = new FixedPointVector2(0, 0);
        private static FixedPointVector2 oneVector = new FixedPointVector2(1, 1);

        private static FixedPointVector2 rightVector = new FixedPointVector2(1, 0);
        private static FixedPointVector2 leftVector = new FixedPointVector2(-1, 0);

        private static FixedPointVector2 upVector = new FixedPointVector2(0, 1);
        private static FixedPointVector2 downVector = new FixedPointVector2(0, -1);

        #endregion Private Fields

        #region Public Fields

        public FixedPointInt64 x;
        public FixedPointInt64 y;

        #endregion Public Fields

#region Properties

        public static FixedPointVector2 zero
        {
            get { return zeroVector; }
        }

        public static FixedPointVector2 one
        {
            get { return oneVector; }
        }

        public static FixedPointVector2 right
        {
            get { return rightVector; }
        }

        public static FixedPointVector2 left {
            get { return leftVector; }
        }

        public static FixedPointVector2 up
        {
            get { return upVector; }
        }

        public static FixedPointVector2 down {
            get { return downVector; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor foe standard 2D vector.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Single"/>
        /// </param>
        public FixedPointVector2(FixedPointInt64 x, FixedPointInt64 y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for "square" vector.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/>
        /// </param>
        public FixedPointVector2(FixedPointInt64 value)
        {
            x = value;
            y = value;
        }

        public void Set(FixedPointInt64 x, FixedPointInt64 y) {
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Public Methods

        public static void Reflect(ref FixedPointVector2 vector, ref FixedPointVector2 normal, out FixedPointVector2 result)
        {
            FixedPointInt64 dot = Dot(vector, normal);
            result.x = vector.x - ((2f*dot)*normal.x);
            result.y = vector.y - ((2f*dot)*normal.y);
        }

        public static FixedPointVector2 Reflect(FixedPointVector2 vector, FixedPointVector2 normal)
        {
            FixedPointVector2 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static FixedPointVector2 Add(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static void Add(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointVector2 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
        }

        public static FixedPointVector2 Barycentric(FixedPointVector2 value1, FixedPointVector2 value2, FixedPointVector2 value3, FixedPointInt64 amount1, FixedPointInt64 amount2)
        {
            return new FixedPointVector2(
                FixedPointMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixedPointMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static void Barycentric(ref FixedPointVector2 value1, ref FixedPointVector2 value2, ref FixedPointVector2 value3, FixedPointInt64 amount1,
                                       FixedPointInt64 amount2, out FixedPointVector2 result)
        {
            result = new FixedPointVector2(
                FixedPointMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixedPointMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static FixedPointVector2 CatmullRom(FixedPointVector2 value1, FixedPointVector2 value2, FixedPointVector2 value3, FixedPointVector2 value4, FixedPointInt64 amount)
        {
            return new FixedPointVector2(
                FixedPointMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixedPointMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static void CatmullRom(ref FixedPointVector2 value1, ref FixedPointVector2 value2, ref FixedPointVector2 value3, ref FixedPointVector2 value4,
                                      FixedPointInt64 amount, out FixedPointVector2 result)
        {
            result = new FixedPointVector2(
                FixedPointMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixedPointMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static FixedPointVector2 Clamp(FixedPointVector2 value1, FixedPointVector2 min, FixedPointVector2 max)
        {
            return new FixedPointVector2(
                FixedPointMath.Clamp(value1.x, min.x, max.x),
                FixedPointMath.Clamp(value1.y, min.y, max.y));
        }

        public static void Clamp(ref FixedPointVector2 value1, ref FixedPointVector2 min, ref FixedPointVector2 max, out FixedPointVector2 result)
        {
            result = new FixedPointVector2(
                FixedPointMath.Clamp(value1.x, min.x, max.x),
                FixedPointMath.Clamp(value1.y, min.y, max.y));
        }

        /// <summary>
        /// Returns FP precison distanve between two vectors
        /// </summary>
        /// <param name="value1">
        /// A <see cref="FixedPointVector2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="FixedPointVector2"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public static FixedPointInt64 Distance(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            FixedPointInt64 result;
            DistanceSquared(ref value1, ref value2, out result);
            return (FixedPointInt64) FixedPointInt64.Sqrt(result);
        }


        public static void Distance(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointInt64 result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = (FixedPointInt64) FixedPointInt64.Sqrt(result);
        }

        public static FixedPointInt64 DistanceSquared(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            FixedPointInt64 result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointInt64 result)
        {
            result = (value1.x - value2.x)*(value1.x - value2.x) + (value1.y - value2.y)*(value1.y - value2.y);
        }

        /// <summary>
        /// Devide first vector with the secund vector
        /// </summary>
        /// <param name="value1">
        /// A <see cref="FixedPointVector2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="FixedPointVector2"/>
        /// </param>
        /// <returns>
        /// A <see cref="FixedPointVector2"/>
        /// </returns>
        public static FixedPointVector2 Divide(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }

        public static void Divide(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointVector2 result)
        {
            result.x = value1.x/value2.x;
            result.y = value1.y/value2.y;
        }

        public static FixedPointVector2 Divide(FixedPointVector2 value1, FixedPointInt64 divider)
        {
            FixedPointInt64 factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        public static void Divide(ref FixedPointVector2 value1, FixedPointInt64 divider, out FixedPointVector2 result)
        {
            FixedPointInt64 factor = 1/divider;
            result.x = value1.x*factor;
            result.y = value1.y*factor;
        }

        public static FixedPointInt64 Dot(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            return value1.x*value2.x + value1.y*value2.y;
        }

        public static void Dot(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointInt64 result)
        {
            result = value1.x*value2.x + value1.y*value2.y;
        }

        public override bool Equals(object obj)
        {
            return (obj is FixedPointVector2) ? this == ((FixedPointVector2) obj) : false;
        }

        public bool Equals(FixedPointVector2 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int) (x + y);
        }

        public static FixedPointVector2 Hermite(FixedPointVector2 value1, FixedPointVector2 tangent1, FixedPointVector2 value2, FixedPointVector2 tangent2, FixedPointInt64 amount)
        {
            FixedPointVector2 result = new FixedPointVector2();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        public static void Hermite(ref FixedPointVector2 value1, ref FixedPointVector2 tangent1, ref FixedPointVector2 value2, ref FixedPointVector2 tangent2,
                                   FixedPointInt64 amount, out FixedPointVector2 result)
        {
            result.x = FixedPointMath.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
            result.y = FixedPointMath.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
        }

        public FixedPointInt64 magnitude {
            get {
                FixedPointInt64 result;
                DistanceSquared(ref this, ref zeroVector, out result);
                return FixedPointInt64.Sqrt(result);
            }
        }

        public static FixedPointVector2 ClampMagnitude(FixedPointVector2 vector, FixedPointInt64 maxLength) {
            return Normalize(vector) * maxLength;
        }

        public FixedPointInt64 LengthSquared()
        {
            FixedPointInt64 result;
            DistanceSquared(ref this, ref zeroVector, out result);
            return result;
        }

        public static FixedPointVector2 Lerp(FixedPointVector2 value1, FixedPointVector2 value2, FixedPointInt64 amount) {
            amount = FixedPointMath.Clamp(amount, 0, 1);

            return new FixedPointVector2(
                FixedPointMath.Lerp(value1.x, value2.x, amount),
                FixedPointMath.Lerp(value1.y, value2.y, amount));
        }

        public static FixedPointVector2 LerpUnclamped(FixedPointVector2 value1, FixedPointVector2 value2, FixedPointInt64 amount)
        {
            return new FixedPointVector2(
                FixedPointMath.Lerp(value1.x, value2.x, amount),
                FixedPointMath.Lerp(value1.y, value2.y, amount));
        }

        public static void LerpUnclamped(ref FixedPointVector2 value1, ref FixedPointVector2 value2, FixedPointInt64 amount, out FixedPointVector2 result)
        {
            result = new FixedPointVector2(
                FixedPointMath.Lerp(value1.x, value2.x, amount),
                FixedPointMath.Lerp(value1.y, value2.y, amount));
        }

        public static FixedPointVector2 Max(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            return new FixedPointVector2(
                FixedPointMath.Max(value1.x, value2.x),
                FixedPointMath.Max(value1.y, value2.y));
        }

        public static void Max(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointVector2 result)
        {
            result.x = FixedPointMath.Max(value1.x, value2.x);
            result.y = FixedPointMath.Max(value1.y, value2.y);
        }

        public static FixedPointVector2 Min(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            return new FixedPointVector2(
                FixedPointMath.Min(value1.x, value2.x),
                FixedPointMath.Min(value1.y, value2.y));
        }

        public static void Min(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointVector2 result)
        {
            result.x = FixedPointMath.Min(value1.x, value2.x);
            result.y = FixedPointMath.Min(value1.y, value2.y);
        }

        public void Scale(FixedPointVector2 other) {
            this.x = x * other.x;
            this.y = y * other.y;
        }

        public static FixedPointVector2 Scale(FixedPointVector2 value1, FixedPointVector2 value2) {
            FixedPointVector2 result;
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;

            return result;
        }

        public static FixedPointVector2 Multiply(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static FixedPointVector2 Multiply(FixedPointVector2 value1, FixedPointInt64 scaleFactor)
        {
            value1.x *= scaleFactor;
            value1.y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref FixedPointVector2 value1, FixedPointInt64 scaleFactor, out FixedPointVector2 result)
        {
            result.x = value1.x*scaleFactor;
            result.y = value1.y*scaleFactor;
        }

        public static void Multiply(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointVector2 result)
        {
            result.x = value1.x*value2.x;
            result.y = value1.y*value2.y;
        }

        public static FixedPointVector2 Negate(FixedPointVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static void Negate(ref FixedPointVector2 value, out FixedPointVector2 result)
        {
            result.x = -value.x;
            result.y = -value.y;
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public static FixedPointVector2 Normalize(FixedPointVector2 value)
        {
            Normalize(ref value, out value);
            return value;
        }

        public FixedPointVector2 normalized {
            get {
                FixedPointVector2 result;
                FixedPointVector2.Normalize(ref this, out result);

                return result;
            }
        }

        public static void Normalize(ref FixedPointVector2 value, out FixedPointVector2 result)
        {
            FixedPointInt64 factor;
            DistanceSquared(ref value, ref zeroVector, out factor);
            factor = 1f/(FixedPointInt64) FixedPointInt64.Sqrt(factor);
            result.x = value.x*factor;
            result.y = value.y*factor;
        }

        public static FixedPointVector2 SmoothStep(FixedPointVector2 value1, FixedPointVector2 value2, FixedPointInt64 amount)
        {
            return new FixedPointVector2(
                FixedPointMath.SmoothStep(value1.x, value2.x, amount),
                FixedPointMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static void SmoothStep(ref FixedPointVector2 value1, ref FixedPointVector2 value2, FixedPointInt64 amount, out FixedPointVector2 result)
        {
            result = new FixedPointVector2(
                FixedPointMath.SmoothStep(value1.x, value2.x, amount),
                FixedPointMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static FixedPointVector2 Subtract(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static void Subtract(ref FixedPointVector2 value1, ref FixedPointVector2 value2, out FixedPointVector2 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
        }

        public static FixedPointInt64 Angle(FixedPointVector2 a, FixedPointVector2 b) {
            return FixedPointInt64.Acos(a.normalized * b.normalized) * FixedPointInt64.Rad2Deg;
        }

        public FixedPointVector3 ToTSVector() {
            return new FixedPointVector3(this.x, this.y, 0);
        }

        public override string ToString() {
            return string.Format("({0:f1}, {1:f1})", x.AsFloat(), y.AsFloat());
        }

        #endregion Public Methods

#region Operators

        public static FixedPointVector2 operator -(FixedPointVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }


        public static bool operator ==(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static FixedPointVector2 operator +(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }


        public static FixedPointVector2 operator -(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }


        public static FixedPointInt64 operator *(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            return FixedPointVector2.Dot(value1, value2);
        }


        public static FixedPointVector2 operator *(FixedPointVector2 value, FixedPointInt64 scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FixedPointVector2 operator *(FixedPointInt64 scaleFactor, FixedPointVector2 value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FixedPointVector2 operator /(FixedPointVector2 value1, FixedPointVector2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }


        public static FixedPointVector2 operator /(FixedPointVector2 value1, FixedPointInt64 divider)
        {
            FixedPointInt64 factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        #endregion Operators
    }
}