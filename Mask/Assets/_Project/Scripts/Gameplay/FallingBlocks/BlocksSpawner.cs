namespace _Project.Scripts.Gameplay.Spawning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Input;
    using Input.Blocks;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Random = UnityEngine.Random;

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
            SpawnRows(_gridConfig.RowsPerSpawn, true);
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
            Vector3 GetWorldPosition(Vector2Int gridPos)
            {
                var horizontalOrigin = LevelManager.Instance.HorizontalOrigin.transform.position.x;
                var horizontal = horizontalOrigin + (_gridConfig.HorizontalGap + _blockPrefab.GetSize().x) * gridPos.x;
                var vertical = gridPos.y * _blockPrefab.VerticalGap + startY;
                var spawnPos = new Vector3(horizontal, vertical);
                return spawnPos;
            }

            void CreateBlock(BlockType type, Vector2Int gridPos, BlockMechanicType mechanicType)
            {
                
                var block = SpawnBlock(GetWorldPosition(gridPos), type, mechanicType);
                _model.AddBlock(block);
                
            }

            var shuffeldRows = Enumerable.Range(0, rows).OrderBy(x => Random.value).ToArray();
            var suffeldColumns = Enumerable.Range(0, columns).OrderBy(x => Random.value).ToArray();

            HashSet<Vector2Int> usedPositions = new();

            var toSpawn = rows * columns;
            var rowToUseIndex = 0;
            var columnToUseIndex = 0;
            for (int i = 0; i < toSpawn; i++)
            {
                var rowToUse = shuffeldRows[rowToUseIndex];
                var columnToUse = suffeldColumns[columnToUseIndex];
                var blockPosition = new Vector2Int(columnToUse, rowToUse);

                if (!usedPositions.Add(blockPosition))
                {
                    rowToUseIndex++;

                    if (rowToUseIndex == shuffeldRows.Length)
                    {
                        columnToUseIndex = (columnToUseIndex + 1) % suffeldColumns.Length;
                        rowToUseIndex = 0;
                    }
                    continue;
                }

                var blockMechanicType = useMechanics ? GetBlockMechanic(blockPosition, new Vector2Int(columns, rows), usedPositions) : BlockMechanicType.None;

                var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;
                CreateBlock(blockType, blockPosition, blockMechanicType);
                
                
                if (blockMechanicType.IsPass())
                {
                    var passedPos = blockMechanicType.GetDirectionVector() + blockPosition;
                    usedPositions.Add(passedPos);
                    CreateBlock(blockType, passedPos, BlockMechanicType.Unknown);
                }

                rowToUseIndex++;

                if (rowToUseIndex == shuffeldRows.Length)
                {
                    columnToUseIndex = (columnToUseIndex + 1) % suffeldColumns.Length;
                    rowToUseIndex = 0;
                }
            }
        }

        public BlockMechanicType GetBlockMechanic(Vector2Int thisBlock, Vector2Int gridSize, HashSet<Vector2Int> usedPositions)
        {
            var value = Random.value;
            if (value < _gridConfig.PassProbability)
            {
                return GetPassBlocksType(thisBlock, gridSize, usedPositions);
            }
            if (value < _gridConfig.InvertedProbability + _gridConfig.PassProbability)
            {
                return BlockMechanicType.Inverted;
            }
            
            return BlockMechanicType.None;
        }

        private BlockMechanicType GetPassBlocksType(Vector2Int thisBlock, Vector2Int gridSize, HashSet<Vector2Int> usedPositions)
        {
            var validPasses = new HashSet<BlockMechanicType> { BlockMechanicType.DownPass, BlockMechanicType.LeftPass, BlockMechanicType.RightPass, BlockMechanicType.UpPass};

            if (thisBlock.y == 0 || usedPositions.Contains(thisBlock + BlockMechanicType.DownPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.DownPass);
            }
            if (thisBlock.y == gridSize.y - 1 || usedPositions.Contains(thisBlock + BlockMechanicType.UpPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.UpPass);
            }

            if (thisBlock.x == gridSize.x - 1 || usedPositions.Contains(thisBlock + BlockMechanicType.RightPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.RightPass);
            }

            if (thisBlock.x == 0 || usedPositions.Contains(thisBlock + BlockMechanicType.LeftPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.LeftPass);
            }

            if (!validPasses.Any())
            {
                return  BlockMechanicType.None;
            }
            
            return validPasses.OrderBy(x => Random.value).First();
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