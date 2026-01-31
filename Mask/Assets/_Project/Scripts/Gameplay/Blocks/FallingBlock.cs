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