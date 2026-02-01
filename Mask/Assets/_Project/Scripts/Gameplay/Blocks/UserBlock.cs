namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using System;
    using Spawning;
    using UnityEngine;

    public class UserBlock : Block
    {
        [SerializeField] private float _moveSpeed = 4f;

        public FallingBlock TargetBlock;
        public event Action OnJoinedFall;
        public event Action OnMissmatched;
        public event Action<UserBlock> OnDestroying;

        public void SetTargetBlock(FallingBlock targetBlock)
        {
            TargetBlock = targetBlock;
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
                Destroy(gameObject);
                return;
            }

            transform.Translate(Vector3.up * (_moveSpeed * Time.deltaTime));

            if (TargetBlock.BottomBlockPosition.position.y + TargetBlock.VerticalGap * .5f <= transform.position.y)
            {
                if (TargetBlock.BlockType == BlockType && NoChainOrCanDestroyChain())
                {
                    CameraView.Instance.DoShake(0.3f, 0.3f, .01f);
                    FallingBlocksModel.Instance.FallingBlocks.ForEach(b => b.DoHit());
                    ClearHittedBlock(TargetBlock);
                    
                    FallingBlocksModel.Instance.CheckForPassDown(TargetBlock.transform.position.y);
                    FallingBlocksModel.Instance.CheckForSidePass(TargetBlock.transform.position.y, TargetBlock.transform.position.x);
                }
                else
                {
                    TargetBlock.CanBeDestroyedAsChain = false;
                    if (TargetBlock.BlockMechanicType == BlockMechanicType.Unknown)
                    {
                        TargetBlock.SetMechanic(BlockMechanicType.None);
                    }

                    AudioManager.Instance.PlayMismatchedBlocksSfx();
                    
                    OnMissmatched?.Invoke();
                    TargetBlock.HandleMissmatchedUserBlock();
                }
                
                OnDestroying?.Invoke(this);
                Destroy(gameObject);
            }
        }

        

        private bool NoChainOrCanDestroyChain()
        {
            return !TargetBlock.BlockMechanicType.IsChainPart() || TargetBlock.CanBeDestroyedAsChain;
        }

        private void ClearHittedBlock(FallingBlock otherBlock)
        {
            FallingBlocksModel.Instance.BreakBlock(otherBlock);
        }
    }
}