using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterController))]
public class MonsterController_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        MonsterController script = (MonsterController)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Connect Hingejoints"))
        {
            script.TransferReferencesBetweenJoints();
        }
    }
}
