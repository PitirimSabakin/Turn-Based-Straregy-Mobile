using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class MyCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GameManager gm = (GameManager)target;

        if (GUILayout.Button("Create cells"))
        {
            gm.SpawnCells();
        }

        if(GUILayout.Button("Destroy cells"))
        {
            gm.DestroyCells();
        }
    }
}
