using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject leftWall;

    public void Configuration(int configuration)
    {
        frontWall.SetActive(IsKthBitSet(configuration, 0));
        rightWall.SetActive(IsKthBitSet(configuration, 1));
        backWall.SetActive(IsKthBitSet(configuration, 2));
        leftWall.SetActive(IsKthBitSet(configuration, 3));
    }
    public bool IsKthBitSet(int configuration, int bit)
    {
        if ((configuration & (1 << bit)) > 0)
            return false;
        else
            return true;
    }
}
