using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VoxelizeOnStart : MonoBehaviour
{
    public static VoxelizeOnStart Instance;

    public List<RayVoxelizer> voxelizers;
    public int currentVoxelizer;

    public Slider slider;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        VoxelizeNextModel();
    }

    public void VoxelizeNextModel()
    {
        foreach (RayVoxelizer rayVoxelizer in voxelizers)
        {
            rayVoxelizer.transform.parent.gameObject.SetActive(false);
        }

        currentVoxelizer += 1;

        if (currentVoxelizer > voxelizers.Count - 1)
        {
            SceneManager.LoadScene("Showcase");
            return;
        }

        voxelizers[currentVoxelizer].transform.parent.gameObject.SetActive(true);
        voxelizers[currentVoxelizer].GenerateVoxelData();
    }

    public void UpdateProgressBar(float progress)
    {
        slider.normalizedValue = progress / 100;
    }
}
