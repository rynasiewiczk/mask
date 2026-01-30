using System.Collections.Generic;
using UnityEngine;

public class BlockRow : MonoBehaviour
{
    public enum RowState
    {
        SPAWNING,
        FALLING,
        LANDING,
        SETTLED
    }

    private List<Block> _blocks;
    private RowState _state;
    private GridConfig _config;
    private GridManager _gridManager;
    private float _targetY;
    private float _snapProgress;

    public RowState State => _state;
    public List<Block> Blocks => _blocks;
    public int RowIndex { get; private set; }

    public void Initialize(GridConfig config, GridManager gridManager, List<Block> blocks, int rowIndex)
    {
        _config = config;
        _gridManager = gridManager;
        _blocks = blocks;
        RowIndex = rowIndex;
        _state = RowState.SPAWNING;

        // Parent all blocks to this row
        foreach (var block in _blocks)
        {
            block.transform.SetParent(transform);
        }

        // Transition to falling immediately
        _state = RowState.FALLING;
    }

    private void Update()
    {
        switch (_state)
        {
            case RowState.FALLING:
                UpdateFalling();
                break;
            case RowState.LANDING:
                UpdateLanding();
                break;
        }
    }

    private void UpdateFalling()
    {
        // Move row downward
        float fallDistance = _config.fallSpeed * Time.deltaTime;
        transform.position += Vector3.down * fallDistance;

        // Check if we should start landing
        if (ShouldLand())
        {
            StartLanding();
        }
    }

    private bool ShouldLand()
    {
        // Check if we've reached the bottom of the grid
        float bottomY = _gridManager.GetWorldPosition(0, 0).y;
        if (transform.position.y <= bottomY)
        {
            return true;
        }

        // Check if we're about to collide with settled blocks
        int currentRow = _gridManager.WorldToGridY(transform.position.y);

        // Check the row below us
        int rowBelow = currentRow - 1;
        if (rowBelow >= 0 && _gridManager.IsRowOccupied(rowBelow))
        {
            // We're about to hit settled blocks
            return true;
        }

        // If we're very close to settled blocks, start landing
        if (rowBelow >= 0)
        {
            float rowBelowY = _gridManager.GetWorldPosition(0, rowBelow).y;
            float distanceToBelow = transform.position.y - rowBelowY;
            if (distanceToBelow < _config.CellSize * 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    private void StartLanding()
    {
        _state = RowState.LANDING;

        // Calculate the target Y position (snap to grid)
        int targetRow = _gridManager.WorldToGridY(transform.position.y);

        // Make sure we land on an empty row
        while (targetRow >= 0 && _gridManager.IsRowOccupied(targetRow))
        {
            targetRow++;
        }

        // Ensure we don't go below row 0
        targetRow = Mathf.Max(0, targetRow);

        _targetY = _gridManager.GetWorldPosition(0, targetRow).y;
        RowIndex = targetRow;
        _snapProgress = 0f;
    }

    private void UpdateLanding()
    {
        // Smoothly snap to grid position
        _snapProgress += _config.snapSpeed * Time.deltaTime;
        float t = Mathf.Clamp01(_snapProgress);

        Vector3 currentPos = transform.position;
        currentPos.y = Mathf.Lerp(currentPos.y, _targetY, t);
        transform.position = currentPos;

        // Check if we've reached the target
        if (t >= 1.0f || Mathf.Abs(currentPos.y - _targetY) < 0.01f)
        {
            // Snap exactly to target
            currentPos.y = _targetY;
            transform.position = currentPos;

            // Settle the row
            SettleRow();
        }
    }

    private void SettleRow()
    {
        _state = RowState.SETTLED;

        // Transfer blocks to grid
        for (int i = 0; i < _blocks.Count; i++)
        {
            Block block = _blocks[i];
            Vector2Int gridPos = new Vector2Int(i, RowIndex);

            // Unparent block from row
            block.transform.SetParent(_gridManager.transform);

            // Set block position to exact grid position
            block.transform.position = _gridManager.GetWorldPosition(gridPos.x, gridPos.y);

            // Register block in grid
            _gridManager.RegisterBlock(block, gridPos);
        }

        // Notify grid manager that row is settled
        _gridManager.OnRowSettled(this);

        // Destroy this row container
        Destroy(gameObject);
    }

    // Debug visualization
    private void OnDrawGizmos()
    {
        if (_blocks == null || _blocks.Count == 0)
            return;

        // Draw state indicator
        switch (_state)
        {
            case RowState.SPAWNING:
                Gizmos.color = Color.yellow;
                break;
            case RowState.FALLING:
                Gizmos.color = Color.cyan;
                break;
            case RowState.LANDING:
                Gizmos.color = Color.green;
                break;
            case RowState.SETTLED:
                Gizmos.color = Color.gray;
                break;
        }

        // Draw a line representing the row
        if (_blocks.Count > 0)
        {
            Vector3 start = _blocks[0].transform.position;
            Vector3 end = _blocks[_blocks.Count - 1].transform.position;
            Gizmos.DrawLine(start, end);
        }
    }
}
