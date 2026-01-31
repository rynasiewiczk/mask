using _Project.Scripts.Gameplay.Input;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private BlockView _view;
    public BlockType BlockType { get; private set; }
    public BlockMechanicType BlockMechanicType { get; private set; }
    
    public Transform NextBlockPosition => _view.NextBlockPosition;

    private void Awake()
    {
        SetType(BlockType.One);
    }

    public void Change()
    {
        if (BlockType == BlockType.One) SetType(BlockType.Zero);
        else if (BlockType == BlockType.Zero) SetType(BlockType.One);
    }

    public void SetType(BlockType blockType)
    {
        BlockType = blockType;
        _view.SetBlockType(BlockType);
    }
    
    public Vector2 GetSize()
    {
        return Vector2.one;
    }

    public void SetMechanic(BlockMechanicType blockMechanic)
    {
        BlockMechanicType = blockMechanic;
        _view.SetMechanic(BlockMechanicType);
    }
}

public enum BlockType
{
    One,
    Zero,
}

public enum BlockMechanicType
{
    None,
    LeftPass,
    RightPass,
    UpPass,
    DownPass,
    Unknown,
    Inverted,
}

