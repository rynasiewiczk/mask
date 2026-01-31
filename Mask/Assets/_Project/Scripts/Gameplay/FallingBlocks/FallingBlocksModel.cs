namespace _Project.Scripts.Gameplay.Spawning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Input.Blocks;
    using UnityEngine;

    public class FallingBlocksModel : MonoBehaviour
    {
        public event Action<FallingBlock> OnBlockAdded;
        public event Action<FallingBlock> OnBlockBreak;
        
        public static FallingBlocksModel Instance { get; private set; }

        private List<FallingBlock> _fallingBlocks = new();
        public List<FallingBlock> FallingBlocks => _fallingBlocks;

        private ResetFallingBlocksSystem _resetBlocksSystem;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _resetBlocksSystem = new ResetFallingBlocksSystem(LevelManager.Instance, this);
        }

        private void OnDestroy()
        {
            _resetBlocksSystem.Dispose();
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

        public void BreakBlock(FallingBlock block)
        {
            OnBlockBreak?.Invoke(block);
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