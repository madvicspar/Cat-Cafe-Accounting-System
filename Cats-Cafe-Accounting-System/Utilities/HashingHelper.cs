using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security;
using System.Text;

public static class HashingHelper
{
    public static byte[] ComputeHash(SecureString secureString, byte[] salt)
    {
        string password = SecureStringToString(secureString); // Метод для конвертации SecureString в строку

        using (var hmac = new HMACSHA256(salt))
        {
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public static byte[] ComputeHash(byte[] password, byte[] salt)
    {
        using (var hmac = new HMACSHA256(salt))
        {
            return hmac.ComputeHash(password);
        }
    }

    public static bool CompareByteArrays(byte[] array1, byte[] array2)
    {
        return StructuralComparisons.StructuralEqualityComparer.Equals(array1, array2);
    }

    private static string SecureStringToString(SecureString secureString)
    {
        IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
        try
        {
            return Marshal.PtrToStringBSTR(ptr);
        }
        finally
        {
            Marshal.ZeroFreeBSTR(ptr);
        }
    }

    public static byte[] GenerateSalt()
    {
        byte[] byteArray = new byte[32];
        new Random().NextBytes(byteArray);
        return byteArray;
    }
}