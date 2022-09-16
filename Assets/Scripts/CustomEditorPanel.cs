using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PanelOfSpells))]
public class CustomEditorPanel : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PanelOfSpells panelOfSpells = (PanelOfSpells)target;

        if (GUILayout.Button("Create cells"))
        {
            panelOfSpells.SpawnCells();
        }
    }

    
}
