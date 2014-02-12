﻿using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Rendering
{
    public static class GraphicsUtilities
    {
        /// <summary>
        /// Converts a MathNet.Numerics vector to a Unity Vector3.
        /// </summary>
        public static Vector3 Vector3(Vector v)
        {
            var x =  (float) v[0];
            var y = -(float) v[1];
            var z =  (float) v[2];

            return new Vector3(x, y, z);
        }

        public static Vector3 Vector3(float colatitude, float azimuth, float radius)
        {
            var x = radius * Mathf.Sin(colatitude) * Mathf.Cos(azimuth);
            var y = -radius * Mathf.Sin(colatitude) * Mathf.Sin(azimuth);
            var z = radius * Mathf.Cos(colatitude);

            return new Vector3(x, y, z);
        }
    }
}
