using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Voxelizer))]
public class VoxelizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (Voxelizer)target;

        if (GUILayout.Button("Generate Voxel Data", GUILayout.Height(30)))
        {
            script.GenerateVoxelData();
        }
    }
}
