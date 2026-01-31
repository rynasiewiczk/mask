namespace _Project.Scripts.Gameplay.Spawning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Input.Blocks;
    using UnityEngine;

    public class FallingBlocksModel : MonoBehaviour
    {
        public static FallingBlocksModel Instance { get; private set; }

        private List<FallingBlock> _fallingBlocks = new();
        public List<FallingBlock> FallingBlocks => _fallingBlocks;

        private void Awake()
        {
            Instance = this;
        }

        public void AddBlock(FallingBlock block) => _fallingBlocks.Add(block);

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

        public void DestroyBlock(FallingBlock block)
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
    }
}