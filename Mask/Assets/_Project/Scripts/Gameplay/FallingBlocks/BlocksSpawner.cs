namespace _Project.Scripts.Gameplay.Spawning
{
    using System.Linq;
    using Input;
    using Input.Blocks;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class BlocksSpawner : MonoBehaviour
    {
        [SerializeField] private FallingBlocksModel _model;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private float _heightGap = .5f;

        [SerializeField] private Block _blockPrefab;

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
            SpawnRows(_gridConfig.RowsPerSpawn, false);
        }

        private void SpawnRows(int count, bool useMechanics)
        {
            var currentTopBlock = _model.GetTopBlock();
            var spawnHeight = _model.GetTopBlock() == null
                ? _spawnPosition.position.y
                : currentTopBlock.TopBlockPosition.position.y;
            SpawnBlocks(count, _gridConfig.Columns, useMechanics, spawnHeight);
        }

        private void SpawnBlocks(int rows, int columns, bool useMechanics, float startY)
        {
            Vector3 GetPosition(int row, int column)
            {
                var horizontalOrigin = LevelManager.Instance.HorizontalOrigin.transform.position.x;
                var horizontal = horizontalOrigin + (_gridConfig.HorizontalGap + _blockPrefab.GetSize().x) * column;
                var vertical = row * _blockPrefab.VerticalGap + startY;
                var spawnPos = new Vector3(horizontal, vertical);
                return spawnPos;
            }

            var shuffeldRows = Enumerable.Range(0, rows).OrderBy(x => Random.value).ToArray();
            var suffeldColumns = Enumerable.Range(0, columns).OrderBy(x => Random.value).ToArray();

            var toSpawn = rows * columns;
            var rowToUseIndex = 0;
            var columnToUseIndex = 0;
            for (int i = 0; i < toSpawn; i++)
            {
                var rowToUse = shuffeldRows[rowToUseIndex];
                var columnToUse = suffeldColumns[columnToUseIndex];

                Debug.Log($"Row: {rowToUse}, Column: {columnToUse}");

                var blockMechanicType = useMechanics ? GetBlockMechanic() : BlockMechanicType.None;
                var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;
                var block = SpawnBlock(GetPosition(rowToUse, columnToUse), blockType, blockMechanicType);
                _model.AddBlock(block);

                rowToUseIndex++;

                if (rowToUseIndex == shuffeldRows.Length)
                {
                    columnToUseIndex = (columnToUseIndex + 1) % suffeldColumns.Length;
                    rowToUseIndex = 0;
                }
            }
        }

        public BlockMechanicType GetBlockMechanic()
        {
            var value = Random.value;
            // if (value < _gridConfig.PassProbability)
            // {
            //     return BlockMechanicType.DownPass;
            // }
             if (value < _gridConfig.InvertedProbability + _gridConfig.PassProbability)
            {
                return BlockMechanicType.Inverted;
            }
            else
            {
                return BlockMechanicType.None;
            }
        }

        private FallingBlock SpawnBlock(Vector3 position, BlockType blockType, BlockMechanicType blockMechanicType)
        {
            var block = BlockFactory.Instance.CreateFallingBlock();
            block.transform.position = position;

            block.SetType(blockType);
            block.SetMechanic(blockMechanicType);

            return block;
        }

        private void TrySpawnNewRows()
        {
            var topBlock = _model.GetTopBlock();
            if (topBlock == null)
            {
                SpawnRows(_gridConfig.RowsPerSpawn, true);
                return;
            }

            if (topBlock.transform.position.y < _spawnPosition.position.y)
            {
                SpawnRows(_gridConfig.RowsPerSpawn, true);
            }
        }
    }
}