namespace _Project.Scripts.Gameplay.Input
{
    using Spawning;
    using UnityEngine;

    public class BlocksFallSystem : MonoBehaviour
    {
        public static BlocksFallSystem Instance { get; private set; }

        [SerializeField] private Transform _speedupHeight;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private FallingBlocksModel _model;
        [SerializeField] private BlocksSpawner _spawner;

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

            if (_isPaused)
            {
                return;
            }

            if (!LevelManager.Instance.StartFalling)
            {
                return;
            }

            var fallSpeed = GetFallSpeed();
            var blocks = _model.FallingBlocks;

            foreach (var block in blocks)
            {
                block.Fall(fallSpeed);
            }
        }

        private float GetFallSpeed()
        {
            var difficultyFallSpeed = _gridConfig.GetFallSpeed(_spawner.SpawnsCount);
            if (ShouldApplySpeedup())
            {
                var fastFall = _gridConfig.FastSpeed;
                var bottomBlock = _model.GetBottomBlock();
                var blockHeightLerp = Mathf.InverseLerp(_spawner.SpawnPosition.y, _speedupHeight.position.y,
                    bottomBlock.transform.position.y);

                var lerp = Mathf.Lerp(fastFall, difficultyFallSpeed, blockHeightLerp);
                
                return lerp * Time.deltaTime;
            }

            return difficultyFallSpeed  * Time.deltaTime;
        }

        private bool ShouldApplySpeedup()
        {
            var bottomBlock = _model.GetBottomBlock();
            return bottomBlock.transform.position.y > _speedupHeight.position.y;
        }
    }
}