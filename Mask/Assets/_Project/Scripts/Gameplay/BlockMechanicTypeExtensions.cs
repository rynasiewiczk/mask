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
            case BlockMechanicType.LeftInverted: 
                return Vector2Int.left;
            case BlockMechanicType.RightPass:
            case BlockMechanicType.RightInverted:
                return  Vector2Int.right;
            case BlockMechanicType.UpPass:
            case BlockMechanicType.UpInverted:
                return  Vector2Int.up;
            case BlockMechanicType.DownPass:
            case BlockMechanicType.DownInverted:
                return  Vector2Int.down;
            default:
                Debug.LogError($"BlockMechanicType {blockMechanicType} is not a pass type");
                return Vector2Int.zero;
        }
    }
    
    public static Vector2Int GetChainDirectionVector(this BlockMechanicType blockMechanicType)
    {
        switch (blockMechanicType)
        {
            case BlockMechanicType.ChainLeft:
                return Vector2Int.left;
            case BlockMechanicType.ChainRight:
                return  Vector2Int.right;
            default:
                Debug.LogError($"BlockMechanicType {blockMechanicType} is not a chain type");
                return Vector2Int.zero;
        }
    }
    
    public static bool IsChainPart(this BlockMechanicType blockMechanicType)
    {
        return blockMechanicType == BlockMechanicType.ChainLeft 
               || blockMechanicType == BlockMechanicType.ChainRight 
               || blockMechanicType == BlockMechanicType.ChainBoth
               || blockMechanicType == BlockMechanicType.ChainEnd;
    }
}