using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner
{
    private GridConfig _config;
    private GridManager _gridManager;
    private Transform _parentTransform;

    public BlockSpawner(GridConfig config, GridManager gridManager, Transform parentTransform)
    {
        _config = config;
        _gridManager = gridManager;
        _parentTransform = parentTransform;
    }

    // Spawn a single row of blocks
    public BlockRow SpawnRow(Vector3 spawnPosition, int rowIndex)
    {
        // Create row container
        GameObject rowObject = new GameObject($"BlockRow_{rowIndex}");
        rowObject.transform.position = spawnPosition;
        rowObject.transform.SetParent(_parentTransform);

        BlockRow blockRow = rowObject.AddComponent<BlockRow>();

        // Create blocks for the row
        List<Block> blocks = new List<Block>();
        for (int col = 0; col < _config.columnCount; col++)
        {
            Block block = CreateBlock(col, rowIndex, rowObject.transform);
            blocks.Add(block);
        }

        // Initialize the row
        blockRow.Initialize(_config, _gridManager, blocks, rowIndex);

        return blockRow;
    }

    // Spawn multiple rows at once
    public List<BlockRow> SpawnMultipleRows(int count, int startRowIndex)
    {
        List<BlockRow> spawnedRows = new List<BlockRow>();

        for (int i = 0; i < count; i++)
        {
            // Calculate spawn position for each row (spawn consecutively with no gaps)
            int spawnRowIndex = startRowIndex + i;
            Vector3 spawnPosition = CalculateSpawnPosition(spawnRowIndex);

            BlockRow row = SpawnRow(spawnPosition, spawnRowIndex);
            spawnedRows.Add(row);
        }

        return spawnedRows;
    }

    // Create a single block
    private Block CreateBlock(int column, int row, Transform rowTransform)
    {
        GameObject blockObject;

        // Instantiate from prefab if available, otherwise create new
        if (_config.blockPrefab != null)
        {
            blockObject = Object.Instantiate(_config.blockPrefab);
        }
        else
        {
            blockObject = new GameObject($"Block_{column}_{row}");
            blockObject.AddComponent<SpriteRenderer>();
        }

        blockObject.name = $"Block_{column}_{row}";
        blockObject.transform.SetParent(rowTransform);

        // Position block relative to row
        Vector3 localPosition = new Vector3(
            column * _config.CellSize,
            0,
            0
        );
        blockObject.transform.localPosition = localPosition;

        // Get or add Block component
        Block block = blockObject.GetComponent<Block>();
        if (block == null)
        {
            block = blockObject.AddComponent<Block>();
        }

        // Randomize block type and color
        int blockType = Random.Range(0, _config.blockColors.Length);
        Color blockColor = _config.blockColors[blockType];

        block.Initialize(blockType, blockColor);

        return block;
    }

    // Calculate spawn position for a row (above the visible area)
    private Vector3 CalculateSpawnPosition(int rowIndex)
    {
        // Get the grid origin position
        Vector3 gridOrigin = _gridManager.GetWorldPosition(0, 0);

        // Calculate Y position for this row
        float yPosition = gridOrigin.y + rowIndex * _config.CellSize;

        // X position should align with the left edge of the grid
        float xPosition = gridOrigin.x;

        return new Vector3(xPosition, yPosition, 0);
    }

    // Calculate the spawn height (how high above screen to spawn)
    public float GetSpawnHeight()
    {
        return _config.TotalRows * _config.CellSize;
    }
}
