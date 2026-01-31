using _Project.Scripts.Gameplay.Input;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private BlockView _view;
    public BlockType BlockType { get; private set; }
    
    public Transform TopBlockPosition => _view.TopBlockPosition;
    public Transform BottomBlockPosition => _view.BottomBlockPosition;

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
    
    public void SetUnknown(bool isUnknown) => _view.SetUnknown(isUnknown);
}

public enum BlockType
{
    One,
    Zero,
    Unknown,
}
