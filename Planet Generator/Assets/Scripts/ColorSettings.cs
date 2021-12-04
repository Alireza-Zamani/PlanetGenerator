using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomColorSettings biomColorSettings;
    public Gradient oceanColor;



    [System.Serializable]
    public class BiomColorSettings
    {
        public Biom[] bioms;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)] public float BlendAmount;


        [System.Serializable]
        public class Biom 
        {
            public Gradient gradient;
            public Color tint;
            [Range(0, 1)] public float tintPercent;

            [Range(0,1)] public float startHeight;

        }

    }
}
