using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private NoiseSettings.SimpleNoiseSettings _noiseSettings;
    private Noise _noise = new Noise();

    public SimpleNoiseFilter (NoiseSettings.SimpleNoiseSettings noiseSettings)
    {
        this._noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _noiseSettings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < _noiseSettings.numLayers; i++)
        {
            float v = _noise.Evaluate(point * frequency + _noiseSettings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency += _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistance;
        }

        noiseValue = noiseValue - _noiseSettings.minValue;
        return noiseValue * _noiseSettings.strength;
    }
}
