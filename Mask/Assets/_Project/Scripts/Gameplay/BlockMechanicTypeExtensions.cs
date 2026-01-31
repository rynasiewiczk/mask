using UnityEngine;

public static class BlockMechanicTypeExtensions
{
    public static bool IsPass(this BlockMechanicType blockMechanicType)
    {
        return blockMechanicType == BlockMechanicType.LeftPass || blockMechanicType == BlockMechanicType.RightPass || blockMechanicType == BlockMechanicType.UpPass || blockMechanicType == BlockMechanicType.DownPass;
    }

    public static Vector2Int GetDirectionVector(this BlockMechanicType blockMechanicType)
    {
        switch (blockMechanicType)
        {
            case BlockMechanicType.LeftPass:
                return Vector2Int.left;
            case BlockMechanicType.RightPass:
                return  Vector2Int.right;
            case BlockMechanicType.UpPass:
                return  Vector2Int.up;
            case BlockMechanicType.DownPass:
                return  Vector2Int.down;
            default:
                Debug.LogError($"BlockMechanicType {blockMechanicType} is not a pass type");
                return Vector2Int.zero;
        }
    }
}