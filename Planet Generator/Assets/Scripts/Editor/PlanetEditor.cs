using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    [SerializeField] Planet planet;

    private Editor _shapeEditor;
    private Editor _colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.ShapeSetting, planet.OnShapeSettingsUpdate, ref planet.shapeSettingsFoldout, ref _shapeEditor);
        DrawSettingsEditor(planet.ColorSetting, planet.OnColorSettingUpdate, ref planet.colorSettingsFoldout, ref _colorEditor);
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings == null)
            return;


        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

        using (var check = new EditorGUI.ChangeCheckScope())
        {
            if (foldout)
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if (check.changed)
                {
                    if (onSettingsUpdated != null)
                    {
                        onSettingsUpdated();
                    }

                }
            }


        }

    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }

}
