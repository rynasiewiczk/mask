namespace _Project.Scripts.Gameplay.Spawning
{
    using System.Linq;
    using Input;
    using Input.Blocks;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class BlocksSpawner : MonoBehaviour
    {
        [SerializeField] private FallingBlocksModel _model;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private float _heightGap = .5f;

        [FormerlySerializedAs("_prefab")] [SerializeField]
        private Block _blockPrefab;

        [SerializeField] private Transform _spawnPosition;

        private void Start()
        {
            SpawnInitialRows();
        }

        private void Update()
        {
            TrySpawnNewRows();
        }

        private void SpawnInitialRows()
        {
            SpawnRows(_gridConfig.RowsPerSpawn);
        }

        private void SpawnRows(int count)
        {
            for (int y = 0; y < count; y++)
            {
                SpawnRow();
            }
        }

        private void SpawnRow()
        {
            var currentTopBlock = _model.GetTopBlock();
            var spawnHeight = currentTopBlock == null
                ? _spawnPosition.position.y
                : currentTopBlock.TopBlockPosition.position.y;

            var horizontalOrigin = LevelManager.Instance.HorizontalOrigin.transform.position.x;
            for (int i = 0; i < _gridConfig.Columns; i++)
            {
                var horizontal = horizontalOrigin + (_gridConfig.HorizontalGap + _blockPrefab.GetSize().x) * i;
                var spawnPos = new Vector3(horizontal, spawnHeight);
                var block = SpawnBlock(spawnPos);
                _model.AddBlock(block);
            }
        }

        private FallingBlock SpawnBlock(Vector3 position)
        {
            var block = BlockFactory.Instance.CreateFallingBlock();
            block.transform.position = position;

            var isSelected = Random.value < .5f;
            block.SetType(isSelected ? BlockType.One : BlockType.Zero);

            return block;
        }

        private void TrySpawnNewRows()
        {
            var topBlock = _model.GetTopBlock();
            if (topBlock == null)
            {
                SpawnRows(_gridConfig.RowsPerSpawn);
                return;
            }

            if (topBlock.transform.position.y < _spawnPosition.position.y)
            {
                SpawnRows(_gridConfig.RowsPerSpawn);
            }
        }
    }
}