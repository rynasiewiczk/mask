namespace _Project.Scripts.Gameplay.Spawning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Input;
    using Input.Blocks;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class BlocksSpawner : MonoBehaviour
    {
        [SerializeField] private FallingBlocksModel _model;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private float _heightGap = .5f;

        [SerializeField] private Block _blockPrefab;

        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private Transform _initSpawnPosition;
        private HashSet<Vector2Int> _usedPositions = new();

        public int SpawnsCount { get; private set; }
        public Vector2 SpawnPosition => _spawnPosition.position;

        private void Start()
        {
            SpawnInitialRows();
            LevelManager.Instance.State.Subscribe(HandleReset);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.State.Unsubscribe(HandleReset);
        }

        private void HandleReset(LevelState state)
        {
            if (state != LevelState.Preparing)
            {
                return;
            }
            
            SpawnsCount = 0;
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
            var spawnHeight = SpawnsCount == 0 ?
                _initSpawnPosition.position.y :
                _model.GetTopBlock() == null
                ? _spawnPosition.position.y
                : currentTopBlock.TopBlockPosition.position.y;
            SpawnBlocks(count, _gridConfig.Columns, spawnHeight, difficultySettings);

            SpawnsCount++;
        }

        private FallingBlock CreateBlock(BlockType type, Vector2Int gridPos, float startY, BlockMechanicType mechanicType)
        {
            var block = SpawnBlock(GetWorldPosition(gridPos, startY), type, mechanicType);
            _usedPositions.Add(gridPos);
            _model.AddBlock(block);
            return block;
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

        private void HandleInvertedPass(Vector2Int blockPosition, Vector2Int gridSize, float startY)
        {
            var blockMechanic = GetInvertedPassBlocksType(blockPosition, gridSize);
            var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;

            if (blockMechanic == BlockMechanicType.None)
            {
                CreateBlock(blockType, blockPosition, startY, blockMechanic);
                return;
            }

            CreateBlock(blockType, blockPosition, startY, blockMechanic);

            var passedPos = blockMechanic.GetDirectionVector() + blockPosition;
            CreateBlock(blockType == BlockType.One ? BlockType.Zero : BlockType.One, passedPos, startY, BlockMechanicType.Unknown);
        }

        private void HandleChain(Vector2Int blockPosition, Vector2Int gridSize, float startY, int maxLength)
        {
            var blocks = new List<Block>();
            var chainGuid = Guid.NewGuid();
            CreateChainIfValid(blockPosition);
            return;

            void CreateChainIfValid(Vector2Int thisBlockPosition)
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

                if (validChainDirections.Contains(BlockMechanicType.ChainLeft) && validChainDirections.Contains(BlockMechanicType.ChainRight) && maxLength > 2)
                {
                    validChainDirections.Remove(BlockMechanicType.ChainLeft);
                    validChainDirections.Remove(BlockMechanicType.ChainRight);
                }
                else
                {
                    validChainDirections.Remove(BlockMechanicType.ChainBoth);
                }
                
                var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;

                if (!validChainDirections.Any() || blocks.Count + 1 >= maxLength)
                {
                    var lastBlock = CreateBlock(blockType, thisBlockPosition, startY, blocks.Count > 0 ? BlockMechanicType.ChainEnd : BlockMechanicType.None)
                        .WithChainGuid(chainGuid);
                    blocks.Add(lastBlock);
                    return;
                }

                var chainType = validChainDirections.OrderBy(x => Random.value).First();
                var block = CreateBlock(blockType, thisBlockPosition, startY, chainType).WithChainGuid(chainGuid);
                blocks.Add(block);

                if (blocks.Count >= maxLength)
                {
                    return;
                }

                if (chainType == BlockMechanicType.ChainBoth)
                {
                    CreateChainIfValid(thisBlockPosition + Vector2Int.left);
                    if (blocks.Count >= maxLength)
                    {
                        return;
                    }
                    
                    CreateChainIfValid(thisBlockPosition + Vector2Int.right);
                }
                else
                {
                    CreateChainIfValid(thisBlockPosition + chainType.GetChainDirectionVector());
                }
            }
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
            
            value = Random.value;
            activateProbability = difficultySettings.InvertedPassProbability;
            if (value < activateProbability)
            {
                HandleInvertedPass(blockPosition, gridSize, startY);
                return;
            }

            value = Random.value;
            activateProbability = difficultySettings.InvertedProbability;
            if (value < activateProbability)
            {
                var blockType = Random.value < .5f ? BlockType.One : BlockType.Zero;
                CreateBlock(blockType, blockPosition, startY, BlockMechanicType.Inverted);
                return;
            }

            value = Random.value;
            activateProbability = difficultySettings.ChainProbability;
            if (value < activateProbability && !HasChainInRow())
            {
                HandleChain(blockPosition, gridSize, startY, difficultySettings.ChainMaxLength);
                return;
            }

            bool HasChainInRow()
            {
                var worldPosition = GetWorldPosition(blockPosition, startY);
                var blocksInLine = _model.GetAllBlocksAtSameLine(worldPosition.y);
                var hasChain =  blocksInLine.Any(b => b.IsChain);
                return hasChain;
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
        
        private BlockMechanicType GetInvertedPassBlocksType(Vector2Int thisBlock, Vector2Int gridSize)
        {
            var validPasses = new HashSet<BlockMechanicType> { BlockMechanicType.DownInverted, BlockMechanicType.LeftInverted, BlockMechanicType.RightInverted, BlockMechanicType.UpInverted };

            if (thisBlock.y == 0 || _usedPositions.Contains(thisBlock + BlockMechanicType.DownInverted.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.DownInverted);
            }

            if (thisBlock.y == gridSize.y - 1 || _usedPositions.Contains(thisBlock + BlockMechanicType.UpInverted.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.UpInverted);
            }

            if (thisBlock.x == gridSize.x - 1 || _usedPositions.Contains(thisBlock + BlockMechanicType.RightInverted.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.RightInverted);
            }

            if (thisBlock.x == 0 || _usedPositions.Contains(thisBlock + BlockMechanicType.LeftInverted.GetDirectionVector()))
            {
                validPasses.Remove(BlockMechanicType.LeftInverted);
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
                var difficulty = _gridConfig.GetDifficultySettings(SpawnsCount);
                SpawnRows(_gridConfig.RowsPerSpawn, difficulty);
            }
        }
    }
}