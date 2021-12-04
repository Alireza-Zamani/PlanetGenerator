using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    private NoiseSettings.RigidNoiseSettings _noiseSettings;
    private Noise _noise = new Noise();

    public RidgidNoiseFilter(NoiseSettings.RigidNoiseSettings noiseSettings)
    {
        this._noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _noiseSettings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < _noiseSettings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs( _noise.Evaluate(point * frequency + _noiseSettings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01( v * _noiseSettings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency += _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistance;
        }

        noiseValue = noiseValue - _noiseSettings.minValue;
        return noiseValue * _noiseSettings.strength;
    }
}
