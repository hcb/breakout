using System;
//using System.Collections.Generic;

struct Index2
{
    public int X, Y;

    public static readonly Index2 Zero = new Index2(0, 0);

    /// <summary>
    /// Creates a new 2D index .
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    public Index2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", X, Y);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }

    /// <summary>
    /// Returns the length of this vector.
    /// </summary>
    public int Length()
    {
        return (int)Math.Sqrt(X * X + Y * Y);
    }

    public static Index2 operator -(Index2 a)
    {
        return new Index2(-a.X, -a.Y);
    }

    public static Index2 operator +(Index2 a, Index2 b)
    {
        return new Index2(a.X + b.X, a.Y + b.Y);
    }

    public static Index2 operator -(Index2 a, Index2 b)
    {
        return new Index2(a.X - b.X, a.Y - b.Y);
    }

    public static Index2 operator *(int s, Index2 v)
    {
        return new Index2(s * v.X, s * v.Y);
    }

    public static Index2 operator *(Index2 v, int s)
    {
        return new Index2(s * v.X, s * v.Y);
    }

    //public static Index2 operator *(int s, Index2 v)
    //{
    //    return new Index2(s * v.X, s * v.Y);
    //}

    //public static Index2 operator *(Index2 v, int s)
    //{
    //    return new Index2(s * v.X, s * v.Y);
    //}

    public static Index2 operator /(Index2 v, int s)
    {
        return new Index2(v.X / s, v.Y / s);
    }

    public static bool operator ==(Index2 a, Index2 b)
    {
        return a.X.Equals(b.X) && a.Y.Equals(b.Y);
    }

    public static bool operator !=(Index2 a, Index2 b)
    {
        return !(a.X.Equals(b.X) && a.Y.Equals(b.Y));
    }

    //public static Index2 operator /(Index2 v, int s)
    //{
    //    return new Index2(v.X / s, v.Y / s);
    //}
}
