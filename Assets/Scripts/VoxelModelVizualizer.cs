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
        //for (int j = 0; j < 10; j++)
        //{
        //    Debug.Log(voxelModel.voxelData[Random.Range(0, voxelModel.voxelData.GetLength(0)), Random.Range(0, voxelModel.voxelData.GetLength(1)), Random.Range(0, voxelModel.voxelData.GetLength(2))]);
        //}

        instancedPopulation = 0;
        for (int z = 0; z < voxelModel.voxelData.GetLength(2); z++)
        {
            for (int y = 0; y < voxelModel.voxelData.GetLength(1); y++)
            {
                for (int x = 0; x < voxelModel.voxelData.GetLength(0); x++)
                {
                    if (voxelModel.voxelData[x, y, z])
                        instancedPopulation += 1;
                }
            }
        }

        matrices = new Matrix4x4[instancedPopulation];

        int i = 0;
        for (int z = 0; z < voxelModel.voxelData.GetLength(2); z++)
        {
            for (int y = 0; y < voxelModel.voxelData.GetLength(1); y++)
            {
                for (int x = 0; x < voxelModel.voxelData.GetLength(0); x++)
                {
                    if (voxelModel.voxelData[x, y, z])
                    {
                        Vector3 position = new Vector3(x, y, z) / 10;
                        Quaternion rotation = Quaternion.identity;
                        Vector3 scale = Vector3.one / 11;

                        Matrix4x4 mat = Matrix4x4.TRS(position + transform.position, rotation, scale);

                        matrices[i] = mat;

                        i++;
                    }
                }
            }
        }

        visualized = true;

        Debug.Log("Vizualizing");
    }

    void Update()
    {
        if (visualized)
            Graphics.DrawMeshInstanced(mesh, 0, material, matrices, instancedPopulation);
    }
}
