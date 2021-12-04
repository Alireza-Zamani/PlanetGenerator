using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back};
    [SerializeField] private FaceRenderMask faceRenderMask;

    [SerializeField] private bool autoUpdate = true;

    [SerializeField, Range(2, 256)] private int resolution = 2;

    [SerializeField, HideInInspector] MeshFilter[] meshFilters;
    TerrainFace[] _terrainFaces;

    [SerializeField] ShapeSettings shapeSetting;
    [SerializeField] ColorSettings colorSetting;

    [HideInInspector] public bool colorSettingsFoldout = false;
    [HideInInspector] public bool shapeSettingsFoldout = false;


    public ShapeSettings ShapeSetting { get => shapeSetting; }
    public ColorSettings ColorSetting { get => colorSetting; }

    private ShapeGenerator _shapeGenerator = new ShapeGenerator();
    private ColorGenerator _colorGenerator = new ColorGenerator();

    private void Start()
    {
        GeneratePlanet();
    }

    private void Initialize()
    {
        _shapeGenerator.UpdateSettings(shapeSetting);
        _colorGenerator.UpdateSettings(colorSetting);

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        if (meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];


        _terrainFaces = new TerrainFace[6];

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("Mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSetting.planetMaterial;


            _terrainFaces[i] = new TerrainFace(_shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);

        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    private void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                _terrainFaces[i].ConstructMesh();
            }
        }

        _colorGenerator.UpdateElevation(_shapeGenerator.elevationMinMax);
    }

    private void GenerateColors()
    {
        _colorGenerator.UpdateColors();

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                _terrainFaces[i].UpdateUVs(_colorGenerator);
            }
        }

    }

}
