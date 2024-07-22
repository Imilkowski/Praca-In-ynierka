using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/VoxelModel")]
public class VoxelModel : ScriptableObject
{
    public bool[,,] voxelData;
    public Vector3Int voxelDataSize;
}
