namespace _Project.Scripts.Gameplay.Input
{
    using Blocks;
    using UnityEngine;

    [DefaultExecutionOrder(-200)]
    public class BlockFactory : MonoBehaviour
    {
        [SerializeField] private UserBlock _userBlockPrefab;
        [SerializeField] private FallingBlock _fallingBlockPrefab;
        
        public static BlockFactory Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        
        public UserBlock CreateUserBlock()
        {
            return Instantiate(_userBlockPrefab);
        }
        
        public FallingBlock CreateFallingBlock()
        {
            return Instantiate(_fallingBlockPrefab);
        }
    }
}