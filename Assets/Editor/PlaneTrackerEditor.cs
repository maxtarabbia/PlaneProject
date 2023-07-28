using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaneTracker))]
public class MapGeneratoreditor : Editor
{
    public override void OnInspectorGUI()
    {

        PlaneTracker Planetracker = (PlaneTracker)target;


        if (DrawDefaultInspector())
            Planetracker.UpdateCamPosition();

        if (GUILayout.Button("Update"))
        {
            Planetracker.UpdateCamPosition();
        }
    }
}