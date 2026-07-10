using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SwiftlyS2.Shared.Natives;

/// <summary>
/// 3-Dimensional vector at world scale (?) for source 2.
/// 
/// No more cssharp chaos.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
public struct VectorWS
{
    public float X;
    public float Y;
    public float Z;
    private const float Rad2Deg = 180.0f / MathF.PI;

    public VectorWS( float x, float y, float z )
    {
        X = x;
        Y = y;
        Z = z;
    }

    public VectorWS( VectorWS other )
    {
        X = other.X;
        Y = other.Y;
        Z = other.Z;
    }

    public readonly Vector3 ToBuiltin()
    {
        return new(X, Y, Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Length() => (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float LengthSquared() => (X * X) + (Y * Y) + (Z * Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Distance( VectorWS other ) => (this - other).Length();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float DistanceSquared( VectorWS other ) => (this - other).LengthSquared();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly VectorWS Cross( VectorWS other ) => new((Y * other.Z) - (Z * other.Y), (Z * other.X) - (X * other.Z), (X * other.Y) - (Y * other.X));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Dot( VectorWS other ) => Dot(this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Normalize()
    {
        var len = Length();
        if (len > 0f)
        {
            var inv = 1.0f / len;
            X *= inv;
            Y *= inv;
            Z *= inv;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly VectorWS Normalized()
    {
        var len = Length();
        if (len > 0f)
        {
            var inv = 1.0f / len;
            return new(X * inv, Y * inv, Z * inv);
        }
        return Zero;
    }

    public readonly void Deconstruct( out float x, out float y, out float z )
    {
        x = X;
        y = Y;
        z = Z;
    }

    /// <summary>
    /// Converts this forward vector into Euler QAngles (pitch, yaw, roll).
    /// Usage: <c>forward.ToQAngles(out var angles);</c>
    /// </summary>
    /// <returns>Resulting <see cref="QAngle"/>.</returns>
    public readonly QAngle ToQAngles()
    {
        float yaw;
        float pitch;

        if (X == 0f && Y == 0f)
        {
            yaw = 0f;
            pitch = Z > 0f ? 270f : 90f;
        }
        else
        {
            yaw = MathF.Atan2(Y, X) * Rad2Deg;
            if (yaw < 0f)
            {
                yaw += 360f;
            }

            var tmp = MathF.Sqrt((X * X) + (Y * Y));
            pitch = MathF.Atan2(-Z, tmp) * Rad2Deg;
            if (pitch < 0f)
            {
                pitch += 360f;
            }
        }

        return new QAngle(pitch, yaw, 0f);
    }

    public readonly Vector ToVector() => new(X, Y, Z);

    public override readonly bool Equals( object? obj ) => obj is VectorWS vector && this == vector;
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override readonly string ToString() => $"Vector({X}, {Y}, {Z})";

    public static VectorWS FromBuiltin( Vector3 vector ) => new(vector.X, vector.Y, vector.Z);
    public static VectorWS Zero => new(0, 0, 0);
    public static VectorWS One => new(1, 1, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot( VectorWS a, VectorWS b ) => (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator +( VectorWS a, VectorWS b ) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator -( VectorWS a, VectorWS b ) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator *( VectorWS a, VectorWS b ) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator /( VectorWS a, VectorWS b ) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator *( VectorWS a, float b ) => new(a.X * b, a.Y * b, a.Z * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator *( float b, VectorWS a ) => new(a.X * b, a.Y * b, a.Z * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator /( VectorWS a, float b )
    {
        if (b == 0)
        {
            throw new DivideByZeroException();
        }
        var oofl = 1.0f / b;
        return new(a.X * oofl, a.Y * oofl, a.Z * oofl);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorWS operator -( VectorWS a ) => new(-a.X, -a.Y, -a.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==( VectorWS a, VectorWS b ) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=( VectorWS a, VectorWS b ) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;

    /// <summary>
    /// Serializes the vector to a string.
    /// Example return: "100 200 300"
    /// </summary>
    /// <param name="formatProvider">Format provider to use for the string. Null for default provider.</param>
    /// <returns>Serialized vector in string.</returns>
    public readonly string Serialize( IFormatProvider? formatProvider = null )
    {
        return $"{X.ToString(formatProvider)} {Y.ToString(formatProvider)} {Z.ToString(formatProvider)}";
    }

    /// <summary>
    /// Deserializes the vector from a string.
    /// Example input: "100 200 300"
    /// </summary>
    /// <exception cref="FormatException">Thrown when the input string is not in the correct format.</exception>
    /// <param name="input">Serialized vector in string.</param>
    /// <param name="formatProvider">Format provider to use for the string. Null for default provider.</param>
    /// <returns>Deserialized vector.</returns>
    public static VectorWS Deserialize( string input, IFormatProvider? formatProvider = null )
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new FormatException("Input string is null or whitespace.");

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
        {
            throw new FormatException("Invalid vector string format. Expected format: 'x y z'");
        }
        return new VectorWS(float.Parse(parts[0], formatProvider), float.Parse(parts[1], formatProvider), float.Parse(parts[2], formatProvider));
    }

    /// <summary>
    /// Tries to deserialize the vector from a string.
    /// Example input: "100 200 300"
    /// </summary>
    /// <param name="input">Serialized vector in string.</param>
    /// <param name="vector">Deserialized vector.</param>
    /// <returns>True if the deserialization was successful, false otherwise.</returns>
    public static bool TryDeserialize( [NotNullWhen(true)] string? input, out VectorWS vector )
    {
        return TryDeserialize(input, null, out vector);
    }

    /// <summary>
    /// Tries to deserialize the vector from a string.
    /// Example input: "100 200 300"
    /// </summary>
    /// <param name="input">Serialized vector in string.</param>
    /// <param name="formatProvider">Format provider to use for the string. Null for default provider.</param>
    /// <param name="vector">Deserialized vector.</param>
    /// <returns>True if the deserialization was successful, false otherwise.</returns>
    public static bool TryDeserialize( [NotNullWhen(true)] string? input, IFormatProvider? formatProvider, out VectorWS vector )
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            vector = Zero;
            return false;
        }
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
        {
            vector = Zero;
            return false;
        }
        if (float.TryParse(parts[0], formatProvider, out var x) && float.TryParse(parts[1], formatProvider, out var y) && float.TryParse(parts[2], formatProvider, out var z))
        {
            vector = new VectorWS(x, y, z);
            return true;
        }
        vector = Zero;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Distance2D( VectorWS other )
    {
        return MathF.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Distance2DSquared( VectorWS other )
    {
        return (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Length2D()
    {
        return MathF.Sqrt(X * X + Y * Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float Length2DSquared()
    {
        return X * X + Y * Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Normalize2D()
    {
        var len = Length2D();
        if (len > 0f)
        {
            var inv = 1.0f / len;
            X *= inv;
            Y *= inv;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly VectorWS Normalized2D()
    {
        var len = Length2D();
        if (len > 0f)
        {
            var inv = 1.0f / len;
            return new VectorWS(X * inv, Y * inv, Z);
        }
        return new VectorWS(0f, 0f, Z);
    }

}