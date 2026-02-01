namespace _Project.Scripts.Gameplay.Spawning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FallingBlocks;
    using Input.Blocks;
    using Input.Score;
    using UnityEngine;

    public class FallingBlocksModel : MonoBehaviour
    {
        public event Action<FallingBlock> OnBlockAdded;
        public event Action<FallingBlock> OnBlockBreak;
        
        public static FallingBlocksModel Instance { get; private set; }

        private List<FallingBlock> _fallingBlocks = new();
        public List<FallingBlock> FallingBlocks => _fallingBlocks;

        private ResetFallingBlocksSystem _resetBlocksSystem;
        private DisableArrowDownWhenBlockBelowIsDestroyedSystem _disableArrowSystem;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _resetBlocksSystem = new ResetFallingBlocksSystem(LevelManager.Instance, this);
            _disableArrowSystem = new DisableArrowDownWhenBlockBelowIsDestroyedSystem(this);
        }

        private void OnDestroy()
        {
            _resetBlocksSystem.Dispose();
            _disableArrowSystem.Disable();
        }

        public void AddBlock(FallingBlock block)
        {
            _fallingBlocks.Add(block);
            OnBlockAdded?.Invoke(block);
        }

        public FallingBlock GetTopBlock()
        {
            if (!_fallingBlocks.Any())
            {
                return null;
            }

            var maxY = .0f;
            FallingBlock block = null;
            foreach (var b in _fallingBlocks)
            {
                if (b.transform.position.y > maxY)
                {
                    maxY = b.transform.position.y;
                    block = b;
                }
            }

            return block;
        }

        public FallingBlock GetBottomBlock()
        {
            if (!_fallingBlocks.Any())
            {
                return null;
            }

            var minY = float.MaxValue;
            FallingBlock block = null;
            foreach (var b in _fallingBlocks)
            {
                if (b.transform.position.y < minY)
                {
                    minY = b.transform.position.y;
                    block = b;
                }
            }
            
            return block;
        }

        public List<FallingBlock> GetAllBlocksAtSameLine(float linePosition)
        {
            var  list = new List<FallingBlock>();
            foreach (var fallingBlock in _fallingBlocks)
            {
                var distance = Mathf.Abs(linePosition - fallingBlock.transform.position.y);
                if (distance < 0.4f)
                {
                    list.Add(fallingBlock);
                }
            }
            
            return list;
        }
        
        public List<FallingBlock> GetAllBlocksAtNextLine(float linePosition)
        {
            var  list = new List<FallingBlock>();
            var linePos = linePosition + _fallingBlocks.First().VerticalGap;
            foreach (var fallingBlock in _fallingBlocks)
            {
                var distance = Mathf.Abs(linePos - fallingBlock.transform.position.y);
                if (distance < 0.4f)
                {
                    list.Add(fallingBlock);
                }
            }
            
            return list;
        }
        
        public void CheckForPassDown(float linePos)
        {
            var nextLineBlocks = GetAllBlocksAtNextLine(linePos);
            foreach (var block in nextLineBlocks)
            {
                if (block.BlockMechanicType == BlockMechanicType.DownPass
                    || block.BlockMechanicType == BlockMechanicType.DownInverted)

                {
                    block.SetMechanic(BlockMechanicType.None);
                }
            }
        }

        public void CheckForSidePass(float linePos, float blockX)
        {
            var thisRowBlock = GetAllBlocksAtSameLine(linePos);

            foreach (var block in thisRowBlock)
            {
                if (block.transform.position.x < blockX &&
                    (block.BlockMechanicType == BlockMechanicType.RightPass
                    || block.BlockMechanicType == BlockMechanicType.RightInverted))
                {
                    block.SetMechanic(BlockMechanicType.None);
                }
                else if (block.transform.position.x > blockX
                         && (block.BlockMechanicType == BlockMechanicType.LeftPass
                             || block.BlockMechanicType == BlockMechanicType.LeftInverted))
                {
                    block.SetMechanic(BlockMechanicType.None);
                }
            }
        }

        public void BreakBlock(FallingBlock block)
        {
            OnBlockBreak?.Invoke(block);
            block.ShowScore(ScoreManager.Instance.GetBreakBlockPoints());
            block.DoDestroyEffect();
            DestroyBlock(block);
        }
        
        private void DestroyBlock(FallingBlock block)
        {
            if (!_fallingBlocks.Contains(block))
            {
                Debug.LogError("The block is not on the list");
            }
            else
            {
                _fallingBlocks.Remove(block);
            }

            Destroy(block.gameObject);
        }

        public void ClearBlocks()
        {
            var list = _fallingBlocks.ToList();

            foreach (var block in list)
            {
                DestroyBlock(block);
            }
            
            _fallingBlocks.Clear();
        }
    }
}