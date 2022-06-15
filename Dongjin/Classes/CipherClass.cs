using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows;

namespace Dongjin.Classes
{
    class CipherClass
    {
        public static Stream CreateEncryptor(Stream source, string password)
        {
            byte[] SaltBytes = new byte[16];
            RandomNumberGenerator.Fill(SaltBytes); //RandomNumberGenerator is used for .Net Core 3

            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, SaltBytes, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);

            byte[] IVBytes = new byte[aes.BlockSize / 8];
            RandomNumberGenerator.Fill(IVBytes); //RandomNumberGenerator is used for .Net Core 3
            aes.IV = IVBytes;

            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);

            //Store/Send the Salt and IV - this can be shared. It's more important that it's very random, than being private.
            source.WriteByte((byte)SaltBytes.Length);
            source.Write(SaltBytes, 0, SaltBytes.Length);
            source.WriteByte((byte)IVBytes.Length);
            source.Write(IVBytes, 0, IVBytes.Length);
            source.Flush();

            var cryptoStream = new CryptoStream(source, transform, CryptoStreamMode.Write);
            return cryptoStream;
        }

        public static Stream CreateDecryptor(Stream source, string password)
        {
            var ArrayLength = source.ReadByte();
            if (ArrayLength == -1) throw new Exception("Salt length not found");
            byte[] SaltBytes = new byte[ArrayLength];
            var readBytes = source.Read(SaltBytes, 0, ArrayLength);
            if (readBytes != ArrayLength) throw new Exception("No support for multiple reads");

            ArrayLength = source.ReadByte();
            if (ArrayLength == -1) throw new Exception("Salt length not found");
            byte[] IVBytes = new byte[ArrayLength];
            readBytes = source.Read(IVBytes, 0, ArrayLength);
            if (readBytes != ArrayLength) throw new Exception("No support for multiple reads");

            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.IV = IVBytes;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, SaltBytes, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);

            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);

            var cryptoStream = new CryptoStream(source, transform, CryptoStreamMode.Read);
            return cryptoStream;
        }

        public const int iterations = 1042; // Recommendation is >= 1000.
    }
}
