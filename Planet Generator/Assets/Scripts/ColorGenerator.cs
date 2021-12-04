using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    private ColorSettings _settings;
    private Texture2D _texture;
    private const int TextureResolution = 50;
    private INoiseFilter _biomNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        _settings = settings;

        if (_texture == null || _texture.height != _settings.biomColorSettings.bioms.Length)
        {
            _texture = new Texture2D(TextureResolution * 2, _settings.biomColorSettings.bioms.Length, TextureFormat.RGBA32, false);
        }

        _biomNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(_settings.biomColorSettings.noise);
    }
     
    public void UpdateElevation (MinMax elevationMinMax)
    {
        Debug.Log( "Min is : " + elevationMinMax.Min);
        Debug.Log( "Max is : " + elevationMinMax.Max);

        _settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomPercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = ((pointOnUnitSphere.y + 1) / 2);
        heightPercent += (_biomNoiseFilter.Evaluate(pointOnUnitSphere) - _settings.biomColorSettings.noiseOffset) * _settings.biomColorSettings.noiseStrength;

        float biomIndex = 0;
        float blendRange = _settings.biomColorSettings.BlendAmount / 2 + 0.001f;

        int numberOfBioms = _settings.biomColorSettings.bioms.Length;

        for (int i = 0; i < numberOfBioms; i++)
        {
            float distance = heightPercent - _settings.biomColorSettings.bioms[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomIndex *= (1 - weight);
            biomIndex += i * weight;

        }
            return biomIndex / Mathf.Max(1, numberOfBioms - 1);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[_texture.width * _texture.height];


        int colorIndex = 0;
        foreach (var biom in _settings.biomColorSettings.bioms)
        {
            for (int i = 0; i < TextureResolution * 2; i++)
            {
                Color gradiantColor;
                if (i < TextureResolution)
                {
                    gradiantColor = _settings.oceanColor.Evaluate(i / (TextureResolution - 1f));
                }
                else
                {
                    gradiantColor = biom.gradient.Evaluate((i - TextureResolution) / (TextureResolution - 1f));
                }

                

                Color tintColor = biom.tint;
                colors[colorIndex] = gradiantColor * (1 - biom.tintPercent) + tintColor * biom.tintPercent;
                colorIndex++;
            }

        }


        _texture.SetPixels(colors);
        _texture.Apply();
        _settings.planetMaterial.SetTexture("_texture", _texture);


    }

}
