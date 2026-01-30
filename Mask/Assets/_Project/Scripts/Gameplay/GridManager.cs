using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GridConfig config;

    // Core systems
    private GridData _gridData;
    private BlockSpawner _spawner;
    private List<BlockRow> _activeRows;

    // Grid positioning
    private Vector3 _gridOrigin;
    private Camera _mainCamera;

    // Spawning
    private int _nextSpawnRowIndex;

    // Singleton
    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize
        _mainCamera = Camera.main;
        _activeRows = new List<BlockRow>();

        if (config == null)
        {
            Debug.LogError("GridManager: No GridConfig assigned!");
            enabled = false;
            return;
        }

        InitializeGrid();
    }

    private void Start()
    {
        // Spawn initial rows if auto-spawn is enabled
        if (config.autoSpawn)
        {
            SpawnRows();
        }
    }

    private void Update()
    {
        // Check if we should spawn new rows based on position
        if (config.autoSpawn)
        {
            CheckAndSpawnRows();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitializeGrid()
    {
        // Create grid data structure
        _gridData = new GridData(config.columnCount, config.TotalRows);

        // Calculate grid origin (centered horizontally, bottom at screen bottom)
        CalculateGridOrigin();

        // Create spawner
        _spawner = new BlockSpawner(config, this, transform);

        // Initialize spawn position to start above the visible area
        _nextSpawnRowIndex = config.TotalRows;

        Debug.Log($"Grid initialized: {config.columnCount}x{config.TotalRows} at origin {_gridOrigin}");
    }

    private void CalculateGridOrigin()
    {
        // Calculate grid width
        float gridWidth = config.GridWidth;

        // Center the grid horizontally around the camera
        float gridLeft = -gridWidth / 2f;

        // Position grid vertically
        // Bottom of grid should be at bottom of visible area
        float cameraHeight = _mainCamera.orthographicSize * 2f;
        float gridBottom = -cameraHeight / 2f;

        _gridOrigin = new Vector3(gridLeft, gridBottom, 0);
    }

    // Coordinate conversion methods
    public Vector3 GetWorldPosition(int x, int y)
    {
        float worldX = _gridOrigin.x + x * config.CellSize;
        float worldY = _gridOrigin.y + y * config.CellSize;
        return new Vector3(worldX, worldY, 0);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return GetWorldPosition(gridPos.x, gridPos.y);
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - _gridOrigin.x) / config.CellSize);
        int y = Mathf.RoundToInt((worldPos.y - _gridOrigin.y) / config.CellSize);
        return new Vector2Int(x, y);
    }

    public int WorldToGridY(float worldY)
    {
        return Mathf.RoundToInt((worldY - _gridOrigin.y) / config.CellSize);
    }

    // Grid state queries
    public bool IsPositionOccupied(int x, int y)
    {
        return _gridData.IsPositionOccupied(x, y);
    }

    public bool IsPositionOccupied(Vector2Int pos)
    {
        return _gridData.IsPositionOccupied(pos);
    }

    public bool IsRowOccupied(int row)
    {
        return _gridData.IsRowOccupied(row);
    }

    public Block GetBlockAt(int x, int y)
    {
        return _gridData.GetBlockAt(x, y);
    }

    public Block GetBlockAt(Vector2Int pos)
    {
        return _gridData.GetBlockAt(pos);
    }

    // Register a settled block in the grid
    public void RegisterBlock(Block block, Vector2Int gridPos)
    {
        _gridData.SetBlockAt(gridPos, block);
    }

    // Spawning methods
    private void CheckAndSpawnRows()
    {
        // If no rows are active, spawn the first batch
        if (_activeRows.Count == 0)
        {
            SpawnRows();
            return;
        }

        // Find the topmost active row (highest Y position)
        float topmostY = float.MinValue;
        foreach (BlockRow row in _activeRows)
        {
            if (row != null && row.transform.position.y > topmostY)
            {
                topmostY = row.transform.position.y;
            }
        }

        // Calculate where the next spawn would occur
        Vector3 nextSpawnPosition = GetWorldPosition(0, _nextSpawnRowIndex);
        float nextSpawnY = nextSpawnPosition.y;

        // Calculate how much space there is between the topmost row and where we'd spawn next
        float availableSpace = nextSpawnY - topmostY;

        // Calculate the minimum space needed (height of the rows we're about to spawn)
        float requiredSpace = config.rowsPerSpawn * config.CellSize;

        // Spawn new rows if there's enough room above the topmost row
        // This ensures new rows spawn as soon as the previous ones have moved down enough
        if (availableSpace >= requiredSpace)
        {
            SpawnRows();
        }
    }

    public void SpawnRows()
    {
        List<BlockRow> newRows = _spawner.SpawnMultipleRows(config.rowsPerSpawn, _nextSpawnRowIndex);
        _activeRows.AddRange(newRows);

        // Update next spawn position (spawn next batch directly above this one)
        _nextSpawnRowIndex += config.rowsPerSpawn;

        Debug.Log($"Spawned {newRows.Count} rows at index {_nextSpawnRowIndex - config.rowsPerSpawn}. Active rows: {_activeRows.Count}");
    }

    // Called when a row has settled
    public void OnRowSettled(BlockRow row)
    {
        _activeRows.Remove(row);
        Debug.Log($"Row settled at Y={row.RowIndex}. Active rows: {_activeRows.Count}");
    }

    // Debug visualization
    private void OnDrawGizmos()
    {
        if (config == null || _gridData == null)
            return;

        // Draw grid bounds
        Gizmos.color = Color.white;
        Vector3 bottomLeft = _gridOrigin;
        Vector3 bottomRight = _gridOrigin + Vector3.right * config.GridWidth;
        Vector3 topLeft = _gridOrigin + Vector3.up * config.GridHeight;
        Vector3 topRight = _gridOrigin + new Vector3(config.GridWidth, config.GridHeight, 0);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);

        // Draw visible area separator
        Gizmos.color = Color.yellow;
        float visibleHeight = config.visibleRows * config.CellSize;
        Vector3 visibleTop = _gridOrigin + Vector3.up * visibleHeight;
        Gizmos.DrawLine(visibleTop, visibleTop + Vector3.right * config.GridWidth);

        // Draw grid cells
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        for (int y = 0; y < config.TotalRows; y++)
        {
            for (int x = 0; x < config.columnCount; x++)
            {
                Vector3 cellCenter = GetWorldPosition(x, y);
                float cellSize = config.blockSize;
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, cellSize, 0.1f));

                // Highlight occupied cells
                if (_gridData != null && _gridData.IsPositionOccupied(x, y))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize * 0.9f, cellSize * 0.9f, 0.1f));
                    Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
                }
            }
        }
    }

    // Public accessors
    public GridConfig Config => config;
    public GridData GridData => _gridData;
    public List<BlockRow> ActiveRows => _activeRows;
}
