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
        private HashSet<Vector2Int> _usedPositions = new();

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
            SpawnRows(_gridConfig.RowsPerSpawn, _gridConfig.Start);
        }

        private void SpawnRows(int count, DifficultySettings difficultySettings)
        {
            var currentTopBlock = _model.GetTopBlock();
            var spawnHeight = _model.GetTopBlock() == null
                ? _spawnPosition.position.y
                : currentTopBlock.TopBlockPosition.position.y;
            SpawnBlocks(count, _gridConfig.Columns, spawnHeight, difficultySettings);
        }

        private void CreateBlock(BlockType type, Vector2Int gridPos, float startY, BlockMechanicType mechanicType)
        {
            var block = SpawnBlock(GetWorldPosition(gridPos, startY), type, mechanicType);
            _usedPositions.Add(gridPos);
            _model.AddBlock(block);
        }

        Vector3 GetWorldPosition(Vector2Int gridPos, float startY)
        {
            var horizontalOrigin = LevelManager.Instance.HorizontalOrigin.transform.position.x;
            var horizontal = horizontalOrigin + (_gridConfig.HorizontalGap + _blockPrefab.GetSize().x) * gridPos.x;
            var vertical = gridPos.y * _blockPrefab.VerticalGap + startY;
            var spawnPos = new Vector3(horizontal, vertical);
            return spawnPos;
        }

        private void SpawnBlocks(int rows, int columns, float startY, DifficultySettings difficultySettings)
        {
            _usedPositions.Clear();
            var shuffeldRows = Enumerable.Range(0, rows).OrderBy(x => Random.value).ToArray();
            var suffeldColumns = Enumerable.Range(0, columns).OrderBy(x => Random.value).ToArray();


            var toSpawn = rows * columns;
            var rowToUseIndex = 0;
            var columnToUseIndex = 0;
            for (int i = 0; i < toSpawn; i++)
            {
                var rowToUse = shuffeldRows[rowToUseIndex];
                var columnToUse = suffeldColumns[columnToUseIndex];
                var blockPosition = new Vector2Int(columnToUse, rowToUse);

                if (!_usedPositions.Contains(blockPosition))
                {
                    CreateBlocks(blockPosition, new Vector2Int(columns, rows), startY, difficultySettings);
                }

                rowToUseIndex++;

                if (rowToUseIndex == shuffeldRows.Length)
                {
                    columnToUseIndex = (columnToUseIndex + 1) % suffeldColumns.Length;
                    rowToUseIndex = 0;
                }
            }
        }

        private void HandlePass(Vector2Int blockPosition, Vector2Int gridSize, float startY)
        {
            var blockMechanic = GetPassBlocksType(blockPosition, gridSize);
            var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;

            if (blockMechanic == BlockMechanicType.None)
            {
                CreateBlock(blockType, blockPosition, startY, blockMechanic);
                return;
            }

            CreateBlock(blockType, blockPosition, startY, blockMechanic);

            var passedPos = blockMechanic.GetDirectionVector() + blockPosition;
            CreateBlock(blockType, passedPos, startY, BlockMechanicType.Unknown);
        }

        public void HandleChain(Vector2Int blockPosition, Vector2Int gridSize, float startY)
        {
            void CreateChainIfValid(Vector2Int thisBlockPosition, bool anyInChain)
            {
                var validChainDirections = new HashSet<BlockMechanicType> { BlockMechanicType.ChainLeft, BlockMechanicType.ChainRight, BlockMechanicType.ChainBoth };

                if (thisBlockPosition.x == 0 || _usedPositions.Contains(thisBlockPosition + Vector2Int.left))
                {
                    validChainDirections.Remove(BlockMechanicType.ChainLeft);
                }

                if (thisBlockPosition.x == gridSize.x - 1 || _usedPositions.Contains(thisBlockPosition + Vector2Int.right))
                {
                    validChainDirections.Remove(BlockMechanicType.ChainRight);
                }

                if (validChainDirections.Contains(BlockMechanicType.ChainLeft) && validChainDirections.Contains(BlockMechanicType.ChainRight))
                {
                    validChainDirections.Remove(BlockMechanicType.ChainLeft);
                    validChainDirections.Remove(BlockMechanicType.ChainRight);
                }
                else
                {
                    validChainDirections.Remove(BlockMechanicType.ChainBoth);
                }
                
                var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;

                if (!validChainDirections.Any())
                {
                    CreateBlock(blockType, thisBlockPosition, startY, anyInChain ? BlockMechanicType.ChainEnd : BlockMechanicType.None);
                    return;
                }

                var chainType = validChainDirections.OrderBy(x => Random.value).First();
                CreateBlock(blockType, thisBlockPosition, startY, chainType);
                if (chainType == BlockMechanicType.ChainBoth)
                {
                    CreateChainIfValid(thisBlockPosition + Vector2Int.left, true);
                    CreateChainIfValid(thisBlockPosition + Vector2Int.right, true);
                }
                else
                {
                    CreateChainIfValid(thisBlockPosition + chainType.GetChainDirectionVector(), true);
                    
                }
            }

            CreateChainIfValid(blockPosition, false);
        }

        private void CreateBlocks(Vector2Int blockPosition, Vector2Int gridSize, float startY, DifficultySettings difficultySettings)
        {
            var value = Random.value;
            var activateProbability = difficultySettings.PassProbability;
            if (value < activateProbability)
            {
                HandlePass(blockPosition, gridSize, startY);
                return;
            }

            activateProbability += difficultySettings.InvertedProbability;
            if (value < activateProbability)
            {
                var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;
                CreateBlock(blockType, blockPosition, startY, BlockMechanicType.Inverted);
                return;
            }

            activateProbability += difficultySettings.ChainProbability;
            if (value < activateProbability)
            {
                HandleChain(blockPosition, gridSize, startY);
                return;
            }

            CreateBlock(Random.value < .5f ? BlockType.One : BlockType.Zero, blockPosition, startY, BlockMechanicType.None);
        }

        private BlockMechanicType GetPassBlocksType(Vector2Int thisBlock, Vector2Int gridSize)
        {
            var validPasses = new HashSet<BlockMechanicType> { BlockMechanicType.DownPass, BlockMechanicType.LeftPass, BlockMechanicType.RightPass, BlockMechanicType.UpPass };

            if (thisBlock.y == 0 || _usedPositions.Contains(thisBlock + BlockMechanicType.DownPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.DownPass);
            }

            if (thisBlock.y == gridSize.y - 1 || _usedPositions.Contains(thisBlock + BlockMechanicType.UpPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.UpPass);
            }

            if (thisBlock.x == gridSize.x - 1 || _usedPositions.Contains(thisBlock + BlockMechanicType.RightPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.RightPass);
            }

            if (thisBlock.x == 0 || _usedPositions.Contains(thisBlock + BlockMechanicType.LeftPass.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.LeftPass);
            }

            if (!validPasses.Any())
            {
                return BlockMechanicType.None;
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
                SpawnRows(_gridConfig.RowsPerSpawn, _gridConfig.Start);
                return;
            }

            if (topBlock.transform.position.y < _spawnPosition.position.y)
            {
                SpawnRows(_gridConfig.RowsPerSpawn, _gridConfig.Medium);
            }
        }
    }
}