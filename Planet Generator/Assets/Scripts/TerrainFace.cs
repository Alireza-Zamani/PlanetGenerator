using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private Mesh _mesh;
    private int _resolution;
    private Vector3 _localUp;

    private Vector3 _axisA;
    private Vector3 _axisB;

    private ShapeGenerator _shapeGenerator;

    public TerrainFace (ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this._shapeGenerator = shapeGenerator;
        this._mesh = mesh;
        this._resolution = resolution;
        this._localUp = localUp;

        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 2 * 3];

        Vector2[] uv = ( _mesh.uv.Length == vertices.Length)? _mesh.uv : new Vector2[vertices.Length];

        int triangleIndex = 0;
        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int index = x + (y * _resolution);

                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                float unscaledElevation = _shapeGenerator.CalculateUnscaledElevastion(pointOnUnitSphere);
                vertices[index] = pointOnUnitSphere * _shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[index].y = unscaledElevation;

                if (x != _resolution -1 && y != _resolution - 1)
                {
                    triangles[triangleIndex] = index;
                    triangles[triangleIndex + 1] = index + _resolution + 1;
                    triangles[triangleIndex + 2] = index + _resolution;

                    triangles[triangleIndex + 3] = index;
                    triangles[triangleIndex + 4] = index + 1;
                    triangles[triangleIndex + 5] = index + _resolution + 1;

                    triangleIndex += 6;
                }

            }

            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
            _mesh.uv = uv;
        }
    }

    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = _mesh.uv;

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int index = x + (y * _resolution);

                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[index].x = colorGenerator.BiomPercentFromPoint(pointOnUnitSphere);
            }
        }

        _mesh.uv = uv;

    }    
}
