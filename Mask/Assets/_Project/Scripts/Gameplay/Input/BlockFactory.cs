namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

    [DefaultExecutionOrder(-200)]
    public class BlockFactory : MonoBehaviour
    {
        [SerializeField] private Block _blockPrefab;
        
        public static BlockFactory Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        
        public Block CreateBlock()
        {
            return Instantiate(_blockPrefab);
        }
    }
}