namespace _Project.Scripts.Gameplay.Spawning
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class BlocksModel : MonoBehaviour
    {
        private List<Block> _blocks = new();
        public List<Block> Blocks => _blocks;

        public void AddBlock(Block block) => _blocks.Add(block);

        public Block GetTopBlock()
        {
            if (!_blocks.Any())
            {
                return null;
            }

            var maxY = .0f;
            Block block = null; 
            foreach (var b in _blocks)
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