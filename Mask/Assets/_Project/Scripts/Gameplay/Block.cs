using System;
using _Project.Scripts.Gameplay.Input;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private BlockView _view;
    public BlockType BlockType { get; private set; }
    
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

    private void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, Vector3.up, 20f, LayerMask.GetMask("Default"));
        if (hit.collider && hit.transform.TryGetComponent(out Block otherBlock)
            && hit.distance < Mathf.Abs(NextBlockPosition.localPosition.y))
        {
            // Debug.Log("Block hit");
            // transform.position = otherBlock.NextBlockPosition.position;
            // _moveSpeed = 0f;
            //add to move system
        }
    }
}

public enum BlockType
{
    One,
    Zero,
    Unknown,
}
