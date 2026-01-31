namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using System;
    using UnityEngine;

    public class FallingBlock : Block
    {
        public Guid ChainGuid { get; private set; }
        
        public void Fall(float distance)
        {
            transform.position += Vector3.down * distance;
        }

        public void HandleMissmatchedUserBlock()
        {
            var effect = Instantiate(_missmatchParticles, transform.position, Quaternion.identity);
            effect.Setup(BlockType == BlockType.One ? _oneColor : _zeroColor);
            _view.ShowMissplay();
        }

        public bool CanBeDestroyedAsChain { get; set; }

        public FallingBlock WithChainGuid(Guid chainGuid)
        {
            this.ChainGuid = chainGuid;
            return this;
        }
    }
}