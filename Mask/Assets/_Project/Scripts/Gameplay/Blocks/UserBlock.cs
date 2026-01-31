namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using System;
    using Spawning;
    using UnityEngine;

    public class UserBlock : Block
    {
        [SerializeField] private float _moveSpeed = 4f;

        private FallingBlock _targetBlock;
        public event Action OnJoinedFall;
        public event Action<UserBlock> OnDestroying;

        public void SetTargetBlock(FallingBlock targetBlock)
        {
            _targetBlock = targetBlock;
        }
        
        private void FixedUpdate()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                Destroy(gameObject);
                return;
            }

            if (_targetBlock == null)
            {
                return;
            }

            transform.Translate(Vector3.up * (_moveSpeed * Time.deltaTime));

            if (_targetBlock.BottomBlockPosition.position.y <= transform.position.y)
            {
                if (_targetBlock.BlockType == BlockType)
                {
                    ClearHittedBlock(_targetBlock);
                }
                else
                {
                    if (_targetBlock.BlockMechanicType == BlockMechanicType.Unknown)
                    {
                        _targetBlock.SetMechanic(BlockMechanicType.None);
                    }
                    var newFallingBlock = BlockFactory.Instance.CreateFallingBlock();
                    newFallingBlock.SetType(BlockType);
                    newFallingBlock.transform.position = _targetBlock.BottomBlockPosition.position;
                    FallingBlocksModel.Instance.AddBlock(newFallingBlock);
                    OnJoinedFall?.Invoke();
                }
                
                OnDestroying?.Invoke(this);
                Destroy(gameObject);
            }
        }

        private void ClearHittedBlock(FallingBlock otherBlock)
        {
            FallingBlocksModel.Instance.BreakBlock(otherBlock);
        }
    }
}