namespace _Project.Scripts.Gameplay.FallingBlocks
{
    using System.Linq;
    using Input.Blocks;
    using Spawning;
    using UnityEngine;

    public class DisableArrowDownWhenBlockBelowIsDestroyedSystem
    {
        private FallingBlocksModel _fallingBlocks;

        public DisableArrowDownWhenBlockBelowIsDestroyedSystem(FallingBlocksModel fallingBlocks)
        {
            _fallingBlocks = fallingBlocks;

            _fallingBlocks.OnBlockBreak += HandleBlockBreak;
        }

        public void Disable()
        {
            _fallingBlocks.OnBlockBreak -= HandleBlockBreak;
        }

        private void HandleBlockBreak(FallingBlock breakBlock)
        {
            var topBlockPosition = breakBlock.TopBlockPosition.position;
            var topBlockList = _fallingBlocks.FallingBlocks.Where(b => Vector3.Distance(b.transform.position, topBlockPosition) < .25f).ToList();
            if (topBlockList.Count == 0)
            {
                return;
            }
            
            var topBlock = topBlockList.First();
            if (topBlock.BlockMechanicType == BlockMechanicType.DownPass ||
                topBlock.BlockMechanicType == BlockMechanicType.DownInverted)
            {
                topBlock.HidePassArrows();
            }
        }
    }
}