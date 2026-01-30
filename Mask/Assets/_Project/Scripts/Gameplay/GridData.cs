using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Block[,] _gridCells;
    private Dictionary<Vector2Int, Block> _positionLookup;
    private int _columns;
    private int _rows;

    public int Columns => _columns;
    public int Rows => _rows;

    public GridData(int columns, int rows)
    {
        _columns = columns;
        _rows = rows;
        _gridCells = new Block[columns, rows];
        _positionLookup = new Dictionary<Vector2Int, Block>();
    }

    // Check if a position is within grid bounds
    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < _columns && y >= 0 && y < _rows;
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        return IsValidPosition(pos.x, pos.y);
    }

    // Check if a position is occupied by a block
    public bool IsPositionOccupied(int x, int y)
    {
        if (!IsValidPosition(x, y))
            return false;

        return _gridCells[x, y] != null;
    }

    public bool IsPositionOccupied(Vector2Int pos)
    {
        return IsPositionOccupied(pos.x, pos.y);
    }

    // Get block at specific position
    public Block GetBlockAt(int x, int y)
    {
        if (!IsValidPosition(x, y))
            return null;

        return _gridCells[x, y];
    }

    public Block GetBlockAt(Vector2Int pos)
    {
        return GetBlockAt(pos.x, pos.y);
    }

    // Set block at specific position
    public void SetBlockAt(int x, int y, Block block)
    {
        if (!IsValidPosition(x, y))
        {
            Debug.LogWarning($"Attempted to set block at invalid position: ({x}, {y})");
            return;
        }

        // Remove old block from lookup if exists
        if (_gridCells[x, y] != null)
        {
            _positionLookup.Remove(new Vector2Int(x, y));
        }

        _gridCells[x, y] = block;

        // Add new block to lookup if not null
        if (block != null)
        {
            _positionLookup[new Vector2Int(x, y)] = block;
            block.GridPosition = new Vector2Int(x, y);
        }
    }

    public void SetBlockAt(Vector2Int pos, Block block)
    {
        SetBlockAt(pos.x, pos.y, block);
    }

    // Remove block at specific position
    public void RemoveBlockAt(int x, int y)
    {
        SetBlockAt(x, y, null);
    }

    public void RemoveBlockAt(Vector2Int pos)
    {
        RemoveBlockAt(pos.x, pos.y);
    }

    // Get neighboring blocks (4-directional)
    public List<Block> GetNeighbors(int x, int y, bool includeDiagonals = false)
    {
        List<Block> neighbors = new List<Block>();

        // Cardinal directions
        Vector2Int[] cardinalOffsets = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // Up
            new Vector2Int(1, 0),   // Right
            new Vector2Int(0, -1),  // Down
            new Vector2Int(-1, 0)   // Left
        };

        foreach (var offset in cardinalOffsets)
        {
            int nx = x + offset.x;
            int ny = y + offset.y;
            Block neighbor = GetBlockAt(nx, ny);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        // Diagonal directions
        if (includeDiagonals)
        {
            Vector2Int[] diagonalOffsets = new Vector2Int[]
            {
                new Vector2Int(1, 1),    // Up-Right
                new Vector2Int(1, -1),   // Down-Right
                new Vector2Int(-1, -1),  // Down-Left
                new Vector2Int(-1, 1)    // Up-Left
            };

            foreach (var offset in diagonalOffsets)
            {
                int nx = x + offset.x;
                int ny = y + offset.y;
                Block neighbor = GetBlockAt(nx, ny);
                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    public List<Block> GetNeighbors(Vector2Int pos, bool includeDiagonals = false)
    {
        return GetNeighbors(pos.x, pos.y, includeDiagonals);
    }

    // Check if a full row can be placed at a specific height
    public bool CanPlaceRowAt(int row)
    {
        if (!IsValidPosition(0, row))
            return false;

        // Check if all positions in the row are empty
        for (int x = 0; x < _columns; x++)
        {
            if (IsPositionOccupied(x, row))
                return false;
        }

        return true;
    }

    // Check if any position in a row is occupied
    public bool IsRowOccupied(int row)
    {
        if (!IsValidPosition(0, row))
            return false;

        for (int x = 0; x < _columns; x++)
        {
            if (IsPositionOccupied(x, row))
                return true;
        }

        return false;
    }

    // Clear entire grid
    public void Clear()
    {
        _gridCells = new Block[_columns, _rows];
        _positionLookup.Clear();
    }

    // Get all blocks in the grid
    public List<Block> GetAllBlocks()
    {
        return new List<Block>(_positionLookup.Values);
    }
}
