using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/VoxelModel")]
public class VoxelModel : ScriptableObject
{
    public List<Voxel> voxelData;
    public Vector3Int voxelModelSize;
    public float resolution;
}
