namespace _Project.Scripts.Gameplay.Spawning
{
    using System.Collections.Generic;
    using System.Linq;
    using Input.Blocks;
    using UnityEngine;

    public class BlocksModel : MonoBehaviour
    {
        private List<FallingBlock> _fallingBlocks = new();
        public List<FallingBlock> FallingBlocks => _fallingBlocks;

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
    }
}