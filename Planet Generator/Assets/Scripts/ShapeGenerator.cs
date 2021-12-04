using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings _settings;
    private INoiseFilter[] _noiseFilters;

    public MinMax elevationMinMax;

    public void UpdateSettings (ShapeSettings settings)
    {
        this.elevationMinMax = new MinMax();

        this._settings = settings;
        _noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            _noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    public float CalculateUnscaledElevastion(Vector3 pointOnUnitSphere)
    {
        float fisrtLayerValue = 0;
        float elevation = 0;

        if (_noiseFilters.Length > 0)
        {
            fisrtLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_settings.noiseLayers[0].enabled)
            {
                elevation = fisrtLayerValue;
            }
        }

        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            if (_settings.noiseLayers[i].enabled)
            {
                float mask = (_settings.noiseLayers[i].useFirstLayerAsMask) ? fisrtLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask; 
            }
        }
        elevationMinMax.AddValue(elevation);

        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0, unscaledElevation);
        elevation = _settings.planetRadius * (1 + elevation);
        return elevation;

    }
} 
