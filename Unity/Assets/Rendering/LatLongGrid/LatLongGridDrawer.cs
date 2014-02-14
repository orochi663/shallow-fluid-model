﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Rendering.LatLongGrid
{
    public static class LatLongGridDrawer
    {
        public static int NumberOfPointsPerLatitude = 100;
        public static int NumberOfPointsPerLongitude = 50;

        public static float ScaleFactor = 0.99f;

        public static void DrawGrid(float radius)
        {
            var gridObject = new GameObject("LatLong Grid");

            foreach (var colatitude in Enumerable.Range(1, 17).Select(i => i*Mathf.PI/18))
            {
                var latitudeObject = DrawLatitude(radius, colatitude);
                latitudeObject.transform.parent = gridObject.transform;
            }

            foreach (var azimuth in Enumerable.Range(0, 36).Select(i => i * 2*Mathf.PI / 36))
            {
                var longitudeObject = DrawLongitude(radius, azimuth);
                longitudeObject.transform.parent = gridObject.transform;
            }

            foreach (var colatitude in Enumerable.Range(1, 17).Select(i => i*Mathf.PI/18))
            {
                var labelObject = DrawLabelsAtColatitude(radius, colatitude);
                labelObject.transform.parent = gridObject.transform;
            }
        }

        private static GameObject DrawLatitude(float radius, float colatitude)
        {
            var azimuths = AnglesInRange(0, 2*Mathf.PI, NumberOfPointsPerLatitude);

            var vertices = 
                azimuths.Select(
                azimuth => radius*CreateVectorAt(colatitude, azimuth))
                .ToArray();

            var latitudeObject = 
                CreateLineObject(
                "Colatitude " + Mathf.Rad2Deg*colatitude, 
                vertices,
                "Materials/LatLongGrid/Boundaries");

            return latitudeObject;
        }


        private static GameObject DrawLongitude(float radius, float azimuth)
        {
            var azimuths = AnglesInRange(0, Mathf.PI, NumberOfPointsPerLongitude);

            var vertices = 
                azimuths.Select(
                colatitude => radius*CreateVectorAt(colatitude, azimuth))
                .ToArray();

            var longitudeObject = 
                CreateLineObject(
                "Longitude " + Mathf.Rad2Deg*azimuth,
                vertices,
                "Materials/LatLongGrid/Boundaries");

            return longitudeObject;
        }

        private static GameObject DrawLabelsAtColatitude(float scaleFactor, float colatitude)
        {
            var azimuths = AnglesInRange(0, 2 * Mathf.PI, 36);

            var labels = azimuths.Take(36).Select(azimuth => DrawLabel(scaleFactor, colatitude, azimuth));

            var parentObject = new GameObject("Latitude Labels " + Mathf.Rad2Deg * colatitude);
            foreach (var label in labels)
            {
                label.transform.parent = parentObject.transform;
            }

            return parentObject;
        }

        private static GameObject DrawLabel(float scaleFactor, float colatitude, float azimuth)
        {
            var text = String.Format("{0,3:N0}  {1,3:N0}", Mathf.Rad2Deg*colatitude, Mathf.Rad2Deg*azimuth);

            var labelObject = new GameObject("Label " + text);

            var normal = CreateVectorAt(colatitude, azimuth);
            var localEast = Vector3.Cross(normal, new Vector3(0, 0, 1));
            var localNorth = Vector3.Cross(localEast, normal);
            labelObject.transform.position = scaleFactor*normal;
            labelObject.transform.rotation = Quaternion.LookRotation(-normal, localNorth);

            var textMesh = labelObject.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.font = Resources.Load("Materials/LatLongGrid/ARIAL", typeof (Font)) as Font;
            textMesh.renderer.material = Resources.Load("Materials/LatLongGrid/OneSidedMaterial", typeof(Material)) as Material;
            textMesh.characterSize = scaleFactor*0.005f;
            textMesh.anchor = TextAnchor.UpperCenter;

            return labelObject;
        }

        private static IEnumerable<float> AnglesInRange(float leftAzimuth, float rightAzimuth, int pointsPerRange)
        {
            float distance;
            if (rightAzimuth >= leftAzimuth)
            {
                distance = rightAzimuth - leftAzimuth;
            }
            else
            {
                distance = 2 * Mathf.PI - (leftAzimuth - rightAzimuth);
            }
            var azimuths = Enumerable.Range(0, pointsPerRange + 1).Select(i => leftAzimuth + i * distance / pointsPerRange);

            return azimuths;
        }

        private static GameObject CreateLineObject(String name, Vector3[] points, String materialName)
        {
            var gameObject = new GameObject(name);
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = Resources.Load(materialName, typeof(Material)) as Material;

            var mesh = meshFilter.mesh;
            mesh.vertices = points;
            mesh.SetIndices(Enumerable.Range(0, points.Count()).ToArray(), MeshTopology.LineStrip, 0);
            mesh.normals = Enumerable.Repeat(new Vector3(), points.Count()).ToArray();
            mesh.uv = Enumerable.Repeat(new Vector2(0, 0), points.Count()).ToArray();

            return gameObject;
        }

        private static Vector3 CreateVectorAt(float colatitude, float azimuth)
        {
            var x = Mathf.Sin(colatitude) * Mathf.Cos(azimuth);
            var y = -Mathf.Sin(colatitude) * Mathf.Sin(azimuth);
            var z = Mathf.Cos(colatitude);

            return new Vector3(x, y, z);
        }
    }
}
