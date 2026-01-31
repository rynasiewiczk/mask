namespace _Project.Scripts.Gameplay.Input
{
    using Spawning;
    using UnityEngine;

    public class BlocksFallSystem : MonoBehaviour
    {
        public static BlocksFallSystem Instance { get; private set; } 
        
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private FallingBlocksModel _model;

        private bool _isPaused;
        
        public bool IsPaused => _isPaused;
        
        private void Awake()
        {
            Instance = this;
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }
        
        private void LateUpdate()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }
            
            if(_isPaused) { return; } 

            var blocks = _model.FallingBlocks;
            foreach (var block in blocks)
            {
                block.Fall(_gridConfig.FallSpeed * Time.deltaTime);
            }
        }
    }
}