using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelModelVizualizer : MonoBehaviour
{
    public VoxelModel voxelModel;

    [Header("Assignables")]

    public Material material;
    public Mesh mesh;

    private Matrix4x4[] matrices;
    private int instancedPopulation;

    private bool visualized;

    void Start()
    {
        VisualizeVoxelModel();
    }

    public void VisualizeVoxelModel()
    {
        ClearVisualization();
        Invoke("CreateVizualization", 0.1f);
    }

    private void ClearVisualization()
    {
        visualized = true;
    }

    private void CreateVizualization()
    {
        instancedPopulation = voxelModel.voxelData.Count;

        matrices = new Matrix4x4[instancedPopulation];

        int i = 0;
        foreach (Voxel voxel in voxelModel.voxelData)
        {
            Vector3 position = voxel.pos * voxelModel.resolution;
            Quaternion rotation = Quaternion.identity;
            Vector3 scale = Vector3.one * voxelModel.resolution * 0.9f;

            Matrix4x4 mat = Matrix4x4.TRS(position + transform.position, rotation, scale);

            matrices[i] = mat;

            i++;
        }

        Invoke("StartVisualization", 0.1f);
    }

    private void StartVisualization()
    {
        visualized = true;

        Debug.Log("Visualizing");
    }

    void Update()
    {
        if (visualized)
            Graphics.DrawMeshInstanced(mesh, 0, material, matrices, instancedPopulation);
    }
}
