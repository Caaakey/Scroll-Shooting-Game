using System;

public class BitUtility
{
    public static bool IsSet<T>(T dest, T source) where T : struct
    {
        int destValue = BitConvert.Enum32ToInt<T>(dest);
        int sourceValue = BitConvert.Enum32ToInt<T>(source);

        return (destValue & sourceValue) != 0;
    }

    public static void Set<T>(ref T dest, T source) where T : struct
    {
        int destValue = BitConvert.Enum32ToInt<T>(dest);
        int sourceValue = BitConvert.Enum32ToInt<T>(source);

        dest = BitConvert.IntToEnum32<T>(destValue | sourceValue);
    }

    public static void UnSet<T>(ref T dest, T source) where T : struct
    {
        int destValue = BitConvert.Enum32ToInt<T>(dest);
        int sourceValue = BitConvert.Enum32ToInt<T>(source);

        dest = BitConvert.IntToEnum32<T>(destValue & (~sourceValue));
    }

}
