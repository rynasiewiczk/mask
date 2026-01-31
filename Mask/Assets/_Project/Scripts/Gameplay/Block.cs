using System;
using _Project.Scripts.Gameplay.Input;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] protected BlockView _view;
    public BlockType BlockType { get; private set; }
    public BlockMechanicType BlockMechanicType { get; private set; }
    
    public Transform TopBlockPosition => _view.TopBlockPosition;
    public Transform BottomBlockPosition => _view.BottomBlockPosition;
    public float VerticalGap => Mathf.Abs(BottomBlockPosition.localPosition.y); 

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

public enum MechanicType
{
    None,
    Pass,
    Inverted,
    Chain,
    InvertedPass,
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
    ChainLeft,
    ChainRight,
    ChainBoth,
    ChainEnd,
    LeftInverted,
    RightInverted,
    UpInverted,
    DownInverted,
}