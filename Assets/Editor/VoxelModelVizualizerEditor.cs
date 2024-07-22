using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoxelModelVizualizer))]
public class VoxelModelVizualizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (VoxelModelVizualizer)target;

        if (GUILayout.Button("Visualize Voxel Model", GUILayout.Height(30)))
        {
            script.VisualizeVoxelModel();
        }
    }
}
