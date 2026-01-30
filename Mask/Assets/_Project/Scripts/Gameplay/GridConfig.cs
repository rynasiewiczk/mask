using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "Gameplay/Grid Config")]
public class GridConfig : ScriptableObject
{
    [Header("Grid Dimensions")]
    [Tooltip("Number of columns in the grid")]
    public int columnCount = 7;

    [Tooltip("Number of visible rows on screen")]
    public int visibleRows = 10;

    [Tooltip("Number of buffer rows above visible area")]
    public int bufferRows = 3;

    [Header("Block Settings")]
    [Tooltip("Size of each block in world units")]
    public float blockSize = 1.0f;

    [Tooltip("Spacing between blocks")]
    public float blockSpacing = 0.1f;

    [Tooltip("Prefab for individual blocks")]
    public GameObject blockPrefab;

    [Header("Spawn Settings")]
    [Tooltip("Number of rows to spawn at once")]
    public int rowsPerSpawn = 2;

    [Tooltip("Time interval between spawns (seconds)")]
    public float spawnInterval = 3.0f;

    [Tooltip("Automatically spawn rows at intervals")]
    public bool autoSpawn = true;

    [Header("Fall Settings")]
    [Tooltip("Speed at which rows fall (units per second)")]
    public float fallSpeed = 2.0f;

    [Tooltip("Speed at which blocks snap to grid positions")]
    public float snapSpeed = 10.0f;

    [Tooltip("Animation curve for falling motion")]
    public AnimationCurve fallCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Visual Settings")]
    [Tooltip("Colors for different block types")]
    public Color[] blockColors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };

    // Calculated properties
    public int TotalRows => visibleRows + bufferRows;
    public float CellSize => blockSize + blockSpacing;
    public float GridWidth => columnCount * CellSize - blockSpacing;
    public float GridHeight => TotalRows * CellSize - blockSpacing;

    private void OnValidate()
    {
        // Ensure valid values
        columnCount = Mathf.Max(1, columnCount);
        visibleRows = Mathf.Max(1, visibleRows);
        bufferRows = Mathf.Max(1, bufferRows);
        rowsPerSpawn = Mathf.Max(1, rowsPerSpawn);
        blockSize = Mathf.Max(0.1f, blockSize);
        blockSpacing = Mathf.Max(0.0f, blockSpacing);
        spawnInterval = Mathf.Max(0.1f, spawnInterval);
        fallSpeed = Mathf.Max(0.1f, fallSpeed);
        snapSpeed = Mathf.Max(1.0f, snapSpeed);

        if (blockColors == null || blockColors.Length == 0)
        {
            blockColors = new Color[] { Color.white };
        }
    }
}
