﻿using System.IO;
using System.Runtime.Serialization.Json;

namespace Implant
{
    public static class Extensions
    {
        public static byte[] Serialise<T>(this T data)
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serialiser.WriteObject(ms, data);
                return ms.ToArray();
            }
        }
        public static T Deserialize<T>(this byte[] data)
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(data))
            {
                return (T)serialiser.ReadObject(ms);
            }
        }
    }
}
