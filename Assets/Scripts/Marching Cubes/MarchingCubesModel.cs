using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesModel : MonoBehaviour
{
    [Header("Properties")]

    public VoxelModel voxelModel;
    public Material modelMaterial;

    public int voxelsInPart;

    public bool showVoxelPoints;

    [Header("Debug")]

    [SerializeField] private Vector3Int divisionFactor;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    [HideInInspector] public bool[,,] voxelDataArray;
    [HideInInspector] public List<Vector3Int> partsPos;
    private List<GameObject> modelParts;

    void Awake()
    {
        ConvertVoxelDataToArray();
        InitializeModel();

        for (int i = 0; i < modelParts.Count; i++)
        {
            CalculateModel(i);
        }
    }

    public int GetIdPyPos(Vector3Int partPos)
    {
        for (int i = 0; i < modelParts.Count; i++)
        {
            if (partsPos[i] == partPos)
                return i;
        }

        return 0;
    }

    private void InitializeModel()
    {
        divisionFactor.x = Mathf.CeilToInt(voxelModel.voxelModelSize.x / (float)voxelsInPart);
        divisionFactor.y = Mathf.CeilToInt(voxelModel.voxelModelSize.y / (float)voxelsInPart);
        divisionFactor.z = Mathf.CeilToInt(voxelModel.voxelModelSize.z / (float)voxelsInPart);

        partsPos = new List<Vector3Int>();
        for (int x = 0; x < divisionFactor.x; x++)
        {
            for (int y = 0; y < divisionFactor.y; y++)
            {
                for (int z = 0; z < divisionFactor.z; z++)
                {
                    partsPos.Add(new Vector3Int(x, y, z));
                }
            }
        }

        modelParts = new List<GameObject>();
        foreach (Vector3Int part in partsPos)
        {
            GameObject spawnedPart = new GameObject("Part " + part.ToString());
            spawnedPart.transform.parent = transform;
            spawnedPart.transform.localPosition = Vector3.zero;

            spawnedPart.AddComponent<MeshFilter>();
            spawnedPart.AddComponent<MeshCollider>();
            spawnedPart.AddComponent<MeshRenderer>().material = modelMaterial;

            if (GetType() == typeof(DestructionModel))
                spawnedPart.AddComponent<DestructionPart>().partPos = part;

            modelParts.Add(spawnedPart);
        }
    }

    public void CalculateModel(int partId)
    {
        MarchCubes(partId);
        SetMesh(partId);
    }

    private void SetMesh(int partId)
    {
        GameObject part = modelParts[partId];

        Mesh mesh = new Mesh();
        //mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        part.GetComponent<MeshFilter>().mesh = mesh;
        part.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void MarchCubes(int partId)
    {
        Vector3Int partPos = partsPos[partId];

        vertices.Clear();
        triangles.Clear();

        int xSize = voxelModel.voxelModelSize.x / divisionFactor.x;
        int ySize = voxelModel.voxelModelSize.y / divisionFactor.y;
        int zSize = voxelModel.voxelModelSize.z / divisionFactor.z;

        for (int x = xSize * partPos.x; x < xSize * (partPos.x + 1); x++)
        {
            for (int y = ySize * partPos.y; y < ySize * (partPos.y + 1); y++)
            {
                for (int z = zSize * partPos.z; z < zSize * (partPos.z + 1); z++)
                {
                    try
                    {
                        bool[] cubeCorners = new bool[8];

                        for (int i = 0; i < 8; i++)
                        {
                            Vector3Int corner = new Vector3Int(x, y, z) + MarchingTable.Corners[i];
                            cubeCorners[i] = !voxelDataArray[corner.x, corner.y, corner.z]; //why does it need to be inverted?
                        }

                        MarchCube(new Vector3Int(x, y, z), GetConfigurationIndex(cubeCorners));
                    }
                    catch (System.Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }

    private void MarchCube(Vector3Int position, int configIndex)
    {
        if (configIndex == 0 || configIndex == 255)
            return;

        int edgeIndex = 0;
        for (int t = 0; t < 5; t++)
        {
            for (int v = 0; v < 3; v++)
            {
                int triTableValue = MarchingTable.Triangles[configIndex, edgeIndex];

                if (triTableValue == -1)
                    return;

                Vector3 edgeStart = position + MarchingTable.Edges[triTableValue, 0];
                Vector3 edgeEnd = position + MarchingTable.Edges[triTableValue, 1];

                Vector3 vertex = (edgeStart + edgeEnd) / 2 * voxelModel.resolution;

                vertices.Add(vertex);
                triangles.Add(vertices.Count - 1);

                edgeIndex++;
            }
        }
    }

    private int GetConfigurationIndex(bool[] cubeCorners)
    {
        int configId = 0;

        for (int i = 0; i < 8; i++)
        {
            if(cubeCorners[i])
                configId |= 1 << i;
        }

        return configId;
    }

    private void ConvertVoxelDataToArray()
    {
        voxelDataArray = new bool[voxelModel.voxelModelSize.x, voxelModel.voxelModelSize.y, voxelModel.voxelModelSize.z];

        foreach (Voxel voxel in voxelModel.voxelData)
        {
            voxelDataArray[(int)voxel.pos.x, (int)voxel.pos.y, (int)voxel.pos.z] = true;
        }
    }

    void OnDrawGizmos()
    {
        if (!showVoxelPoints)
            return;

        if (voxelDataArray == null)
            return; 

        for (int x = 0; x < voxelModel.voxelModelSize.x; x++)
        {
            for (int y = 0; y < voxelModel.voxelModelSize.y; y++)
            {
                for (int z = 0; z < voxelModel.voxelModelSize.z; z++)
                {
                    if(voxelDataArray[x, y, z])
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(transform.position + (new Vector3(x, y, z) * voxelModel.resolution), 0.01f);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(transform.position + (new Vector3(x, y, z) * voxelModel.resolution), 0.01f);
                    }
                }
            }
        }
    }
}
