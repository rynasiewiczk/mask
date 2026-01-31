namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using System;
    using Spawning;
    using UnityEngine;

    public class UserBlock : Block
    {
        [SerializeField] private float _moveSpeed = 4f;

        public FallingBlock TargetBlock;
        private bool _canDestroy;
        public event Action OnJoinedFall;
        public event Action OnMissmatched;
        public event Action<UserBlock> OnDestroying;

        public void SetTargetBlock(FallingBlock targetBlock)
        {
            TargetBlock = targetBlock;
        }

        public void SetCanDestroyTarget(bool canDestroyTarget)
        {
            _canDestroy = canDestroyTarget;
        }

        public bool CanDestroyTarget()
        {
            return TargetBlock.BlockType == BlockType;
        }
        
        private void FixedUpdate()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                Destroy(gameObject);
                return;
            }

            if (TargetBlock == null)
            {
                return;
            }

            transform.Translate(Vector3.up * (_moveSpeed * Time.deltaTime));

            if (TargetBlock.BottomBlockPosition.position.y <= transform.position.y)
            {
                if (TargetBlock.BlockType == BlockType && TargetBlock.BlockMechanicType.IsChainPart() && TargetBlock.CanBeDestroyedIfChain)
                {
                    ClearHittedBlock(TargetBlock);
                }
                else
                {
                    TargetBlock.CanBeDestroyedIfChain = false;
                    if (TargetBlock.BlockMechanicType == BlockMechanicType.Unknown)
                    {
                        TargetBlock.SetMechanic(BlockMechanicType.None);
                    }
                    // var newFallingBlock = BlockFactory.Instance.CreateFallingBlock();
                    // newFallingBlock.SetType(BlockType);
                    // newFallingBlock.SetMechanic(BlockMechanicType.None);
                    // newFallingBlock.transform.position = _targetBlock.BottomBlockPosition.position;
                    // FallingBlocksModel.Instance.AddBlock(newFallingBlock);
                    // OnJoinedFall?.Invoke();
                    
                    OnMissmatched?.Invoke();
                    TargetBlock.HandleMissmatchedUserBlock();
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