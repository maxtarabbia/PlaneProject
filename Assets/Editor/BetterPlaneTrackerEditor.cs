using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BetterPlaneTracker))]
public class BetterPlaneTrackereditor : Editor
{
    public override void OnInspectorGUI()
    {

        BetterPlaneTracker BetterPlaneTracker = (BetterPlaneTracker)target;


        if (DrawDefaultInspector())
            BetterPlaneTracker.UpdateCam();

        if (GUILayout.Button("Update"))
        {
            BetterPlaneTracker.UpdateCam();
        }
    }
}