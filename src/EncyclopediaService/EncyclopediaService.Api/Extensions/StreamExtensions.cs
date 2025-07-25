﻿using System.IO;
using System.Security.Cryptography;

namespace EncyclopediaService.Api.Extensions
{
    public static class StreamExtensions
    {
        public static string ToBase64(this Stream target)
        {
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, new ToBase64Transform(), CryptoStreamMode.Write))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = target.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
                cryptoStream.FlushFinalBlock();

                return System.Text.Encoding.Default.GetString(memoryStream.ToArray());
            }

    }
}
}
