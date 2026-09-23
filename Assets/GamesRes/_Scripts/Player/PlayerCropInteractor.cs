using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCropInteractor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Material[] materials;

    private void Update()
    {
        SetPlayerPostionShaderGraph();
    }
    private void SetPlayerPostionShaderGraph()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetVector("_PlayerPosition", transform.position);
        }
    }
}
