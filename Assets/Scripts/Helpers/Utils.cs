using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Helpers
{
    public static class Utils { }

    [Serializable]
    public class Vector3Serializable
    {
        [JsonRequired] internal float X;
        [JsonRequired] internal float Y;
        [JsonRequired] internal float Z;

        public Vector3Serializable(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }
        
        internal Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
