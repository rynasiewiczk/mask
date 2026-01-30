using _Project.Scripts.Gameplay.Input;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private BlockView _view;
    private BlockType _blockType;
    
    private void Awake()
    {
        SetType(BlockType.One);
    }
        
    public void SetSelected(bool selected)
    {
        _view.SetSelected(selected);
    }

    public void Change()
    {
        if (_blockType == BlockType.One) SetType(BlockType.Zero);
        else if (_blockType == BlockType.Zero) SetType(BlockType.One);
    }

    public void SetType(BlockType blockType)
    {
        _blockType = blockType;
        _view.SetBlockType(_blockType);
        
    }

    public Vector2 GetSize()
    {
        return Vector2.one;
    }

    public void Fall(float distance)
    {
        transform.position += Vector3.down * distance;
    }
}

public enum BlockType
{
    One,
    Zero,
    Unknown,
}
