using System;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header("Actions")]
    public static Action<CropField> OnFullySown;
    public static Action<CropField> OnFullyWatered;
    public static Action<CropField> OnFullyHarvested;

    [Header("Elements")]
    [SerializeField] private Transform tilesParent;
    private List<CropTile> cropTiles = new List<CropTile>();

    [Header("Settings")]
    [SerializeField] private CropData cropData;
    private int tilesSown;
    private int tilesWatered;
    private int tilesHarvested;
    private TileFieldState state;

    private void Start()
    {
        state = TileFieldState.Empty;
        StoreTiles();
    }
    private void StoreTiles()
    {
        for (int i = 0; i < tilesParent.childCount; i++)
        {
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTile>());
        }
    }
    #region Sow Things
    public void SeedsCollidedCallback(Vector3[] seedPositions)
    {
        for (int i = 0; i < seedPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(seedPositions[i]);

            if (closestCropTile == null) continue;

            if (!closestCropTile.IsEmpty()) continue;

            Sow(closestCropTile);
        }
    }
    private void Sow(CropTile cropTile)
    {
        cropTile.Sow(cropData);

        tilesSown++;

        if (tilesSown == cropTiles.Count)
            FieldFullySown();
    }
    private void FieldFullySown()
    {
        state = TileFieldState.Sown;

        OnFullySown?.Invoke(this);
    }
    [NaughtyAttributes.Button]
    private void InstantlySowTiles()
    {
        for (int i = 0; i < cropTiles.Count; i++)
            Sow(cropTiles[i]);
    }
    #endregion
    #region Water Things
    public void WaterCollidedCallback(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(waterPositions[i]);

            if (closestCropTile == null) continue;

            if (!closestCropTile.IsSown()) continue;

            Water(closestCropTile);
        }
    }
    private void Water(CropTile cropTile)
    {
        cropTile.Water();

        tilesWatered++;

        if (tilesWatered == cropTiles.Count)
            FieldFullyWatered();
    }
    private void FieldFullyWatered()
    {
        state = TileFieldState.Watered;

        OnFullyWatered?.Invoke(this);
    }
    [NaughtyAttributes.Button]
    private void InstantlyWaterTiles()
    {
        for (int i = 0; i < cropTiles.Count; i++)
            Water(cropTiles[i]);
    }
    #endregion
    #region Harvest Things
    public void Havest(Transform harvestSphere)
    {
        float sphereRadius = harvestSphere.localScale.x;

        for (int i = 0; i < cropTiles.Count; i++)
        {
            if( cropTiles[i].IsEmpty()) continue;

            float distanceCropTileSphere  = Vector3.Distance(harvestSphere.position, cropTiles[i].transform.position);

            if(distanceCropTileSphere <= sphereRadius)
                HavestTile(cropTiles[i]);
        }
    }
    private void HavestTile(CropTile cropTile)
    {
        cropTile.Harvest();

        tilesHarvested++;

        if(tilesHarvested == cropTiles.Count)
            FieldFullyHarvested();
    }
    private void FieldFullyHarvested()
    {
        tilesSown = 0;
        tilesWatered = 0;
        tilesHarvested = 0;
        
        state = TileFieldState.Empty;

        OnFullyHarvested?.Invoke(this);
    }
    #endregion
    private CropTile GetClosestCropTile(Vector3 seedPosition)
    {
        float minDistance = 1000;
        int closestCropTileIndex = -1;

        for (int i = 0; i < cropTiles.Count; i++)
        {
            CropTile cropTile = cropTiles[i];
            float distanceBetweenTileAndSeed = Vector3.Distance(cropTile.transform.position, seedPosition);

            if (distanceBetweenTileAndSeed < minDistance)
            {
                minDistance = distanceBetweenTileAndSeed;
                closestCropTileIndex = i;
            }
        }

        if (closestCropTileIndex == -1)
            return null;

        return cropTiles[closestCropTileIndex];
    }
    public bool IsEmpty() => state == TileFieldState.Empty;
    public bool IsSown() => state == TileFieldState.Sown;
    public bool IsWatered() => state == TileFieldState.Watered;
}
