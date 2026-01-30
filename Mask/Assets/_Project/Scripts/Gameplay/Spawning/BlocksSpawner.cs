namespace _Project.Scripts.Gameplay.Spawning
{
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class BlocksSpawner : MonoBehaviour
    {
        [SerializeField] private BlocksModel _model;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private float _heightGap = .5f;
        
        [FormerlySerializedAs("_prefab")] [SerializeField] private Block _blockPrefab;
        
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
            SpawnRows(_gridConfig.rowsPerSpawn);
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
                : currentTopBlock.transform.position.y + _blockPrefab.GetSize().y + _heightGap;

            var horizontalPosition = -3;
            var delta = 1.5f;
            for (int i = 0; i < 5; i++)
            {
                var horizontal = horizontalPosition + delta * i * _blockPrefab.GetSize().x;
                var spawnPos =  new Vector3(horizontal, spawnHeight);
                var block = SpawnBlock(spawnPos);
                _model.AddBlock(block);
            }
        }

        private Block SpawnBlock(Vector3 position)
        {
            var block = Instantiate(_blockPrefab);
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
                SpawnRows(_gridConfig.rowsPerSpawn);
                return;
            }
            
            if (topBlock.transform.position.y < _spawnPosition.position.y)
            {
                SpawnRows(_gridConfig.rowsPerSpawn);
            }
        }
    }
}