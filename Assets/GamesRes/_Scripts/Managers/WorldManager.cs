using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform world;
    Chunk[,] grid;

    [Header("Settings")]
    [SerializeField] private int gridSize = 25;
    [SerializeField] private int gridScale;

    [Header("Datas")]
    private WorldData worldData;
    private string dataPath;
    private bool shouldSave;

    [Header("Chunk Meshes")]
    [SerializeField] private Mesh[] chunkShapes;

    private void Awake()
    {
        Chunk.OnUnlocked += ChunkUnlockedCallback;
        Chunk.OnPriceChanged += ChunkPriceChangedCallback;
    }
    private void Start()
    {
        dataPath = Application.persistentDataPath + "/WorldData.txt";
        LoadWorld();
        Initialize();

        InvokeRepeating(nameof(TrySaveGame), 1, 1);
    }
    private void OnDestroy()
    {
        Chunk.OnPriceChanged -= ChunkPriceChangedCallback;
        Chunk.OnUnlocked -= ChunkUnlockedCallback;
    }
    private void ChunkPriceChangedCallback() => shouldSave = true;
    private void ChunkUnlockedCallback()
    {
        UpdateChunkWalls();
        UpdateGridRenderers();

        SaveWorld();
    }
    private void TrySaveGame()
    {
        if (shouldSave)
        {
            SaveWorld();
            shouldSave = false;
        }
    }
    private void Initialize()
    {
        for (int i = 0; i < world.childCount; i++)
        {
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);
        }

        InitializeGrid();

        UpdateChunkWalls();
        UpdateGridRenderers();
    }
    private void InitializeGrid()
    {
        grid = new Chunk[this.gridSize, this.gridSize];

        for (int i = 0; i < world.childCount; i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale,
                (int)chunk.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);

            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }
    }
    private void UpdateChunkWalls()
    {
        // Loop thought all the x axis in the 2 dimensions Grid
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            // Loop thought all the z axis in the 2 dimensions Grid
            for (int z = 0; z < grid.GetLength(1); z++)
            {
                Chunk chunk = grid[x, z];

                if (chunk == null) continue;

                Chunk frontChunk = IsValidGridPosition(x, z + 1) ? grid[x, z + 1] : null;
                Chunk rightChunk = IsValidGridPosition(x + 1, z) ? grid[x + 1, z] : null;
                Chunk backChunk = IsValidGridPosition(x, z - 1) ? grid[x, z - 1] : null;
                Chunk leftChunk = IsValidGridPosition(x - 1, z) ? grid[x - 1, z] : null;

                int configuration = 0;

                if (frontChunk != null && frontChunk.IsUnlocked())
                    configuration += 1;

                if (rightChunk != null && rightChunk.IsUnlocked())
                    configuration += 2;

                if (backChunk != null && backChunk.IsUnlocked())
                    configuration += 4;

                if (leftChunk != null && leftChunk.IsUnlocked())
                    configuration += 8;

                // We know the configuration of the chunk

                chunk.UpdateWalls(configuration);
                SetChunkRenderer(chunk, configuration);
            }
        }
    }
    private enum ChunkShape { None, TopRight, BottomRight, BottomLeft, TopLeft, Top, Right, Bottom, Left, Four }
    private void SetChunkRenderer(Chunk chunk, int configuration)
    {
        switch (configuration)
        {
            case 0:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Four]); break;
            case 1:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]); break;
            case 2:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Left]); break;
            case 3:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomLeft]); break;
            case 4:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Top]); break;
            case 5:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 6:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopLeft]); break;
            case 7:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 8:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Right]); break;
            case 9:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomRight]); break;
            case 10:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 11:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 12:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopRight]); break;
            case 13:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 14:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 15:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
        }
    }
    private void UpdateGridRenderers()
    {
        // Loop thought all the x axis in the 2 dimensions Grid
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            // Loop thought all the z axis in the 2 dimensions Grid
            for (int z = 0; z < grid.GetLength(1); z++)
            {
                Chunk chunk = grid[x, z];

                if (chunk == null) continue;
                if (chunk.IsUnlocked()) continue;

                Chunk frontChunk = IsValidGridPosition(x, z + 1) ? grid[x, z + 1] : null;
                Chunk rightChunk = IsValidGridPosition(x + 1, z) ? grid[x + 1, z] : null;
                Chunk backChunk = IsValidGridPosition(x, z - 1) ? grid[x, z - 1] : null;
                Chunk leftChunk = IsValidGridPosition(x - 1, z) ? grid[x - 1, z] : null;

                if (frontChunk != null && frontChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (rightChunk != null && rightChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (backChunk != null && backChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (leftChunk != null && leftChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
            }
        }
    }
    private bool IsValidGridPosition(int x, int z)
    {
        if (x < 0 || x >= gridSize || z < 0 || z >= gridSize) return false;

        return true;
    }
    private void LoadWorld()
    {
        string data = "";

        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);

            worldData = new WorldData();

            for (int i = 0; i < world.childCount; i++)
            {
                int chunkInitialPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                worldData.chunkPrices.Add(chunkInitialPrice);
            }

            string worldDataString = JsonUtility.ToJson(worldData, true);

            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);

            fs.Write(worldDataBytes);

            fs.Close();
        }
        else
        {
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if (worldData.chunkPrices.Count < world.childCount)
                UpdateData();
        }
    }
    private void UpdateData()
    {
        int missingData = world.childCount - worldData.chunkPrices.Count;

        for (int i = 0; i < missingData; i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            worldData.chunkPrices.Add(chunkPrice);
        }
    }
    private void SaveWorld()
    {
        if (worldData.chunkPrices.Count != world.childCount)
            worldData = new WorldData();

        for (int i = 0; i < world.childCount; i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();

            if (worldData.chunkPrices.Count > i)
                worldData.chunkPrices[i] = chunkCurrentPrice;
            else
                worldData.chunkPrices.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(worldData, true);

        File.WriteAllText(dataPath, data);

        Debug.LogWarning("Data Saved!");
    }
}
