namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using System;
    using System.Collections.Generic;

    public class UserBlocksSequence
    {
        private List<UserBlock> _blocks;
        public IReadOnlyList<UserBlock> Blocks => _blocks;

        public bool DidAnyJoinFalling { get; private set; }

        public event Action<UserBlocksSequence> OnAllDestroyed;
        
        public UserBlocksSequence(List<UserBlock> blocks)
        {
            _blocks = blocks;

            foreach (var userBlock in _blocks)
            {
                userBlock.OnDestroying += HandleDestroy;
                userBlock.OnJoinedFall += HandleFallJoin;
            }
        }

        public void HandleDestroy(UserBlock block)
        {
            _blocks.Remove(block);
            if (_blocks.Count == 0)
            {
                OnAllDestroyed?.Invoke(this);
            }
        }

        public void HandleFallJoin()
        {
            DidAnyJoinFalling = true;
        }
    }
}