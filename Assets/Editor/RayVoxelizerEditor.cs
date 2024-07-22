using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RayVoxelizer))]
public class RayVoxelizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (RayVoxelizer)target;

        if (GUILayout.Button("Generate Voxel Data", GUILayout.Height(30)))
        {
            script.GenerateVoxelData();
        }
    }
}
