namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Blocks;
    using DG.Tweening;
    using LazySloth.Utilities;
    using Spawning;
    using UnityEngine;

    public class InputRow : MonoBehaviour
    {
        [SerializeField] private BlockDestroyParticles _rowDestroy;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private Block _blockPrefab;
        [SerializeField] private Transform _verticalOrigin;

        [SerializeField] private InputRowView _view;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private List<Block> _inputBlocks;
        [SerializeField] private float _cooldown = 0.75f;
        [SerializeField] private Transform _underScreenPosition;

        public List<Block> InputBlocks => _inputBlocks;

        private Block CurrentBlock => _inputBlocks[_selectedBlockIndex];
        private int _prevSelectedBlockIndex = 0;
        private int _selectedBlockIndex = 0;

        private bool _locked = false;
        private Tween _cooldownTween;

        public event Action<UserBlocksSequence> OnSequenceCompleted;

        private void Awake()
        {
            var horizontalOrigin = LevelManager.Instance.HorizontalOrigin.position.x;
            for (int i = 0; i < _gridConfig.Columns; i++)
            {
                var hPos = horizontalOrigin + i * (_blockPrefab.GetSize().x + _gridConfig.HorizontalGap);
                var pos = new Vector2(hPos, _verticalOrigin.position.y);
                var block = Instantiate(_blockPrefab, pos, Quaternion.identity, transform);
                _inputBlocks.Add(block);
            }

            _inputManager.OnLeft += OnLeft;
            _inputManager.OnRight += OnRight;
            _inputManager.OnNumber += OnNumber;
            _inputManager.OnConfirm += OnConfirm;
            _inputManager.OnChange += OnChange;
            _inputManager.OnUseBooster += OnUseBooster;

            _view.SetSelectionPos(_inputBlocks[_selectedBlockIndex].transform.position, true);
            UpdateCurrentBlock(true);
        }

        private void Start()
        {
            LevelManager.Instance.State.Subscribe(ResetOnPreparation);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.State.Unsubscribe(ResetOnPreparation);
        }

        private void ResetOnPreparation(LevelState state)
        {
            if (state == LevelState.Finished)
            {
                _view.SetSelectionVisible(false);
            }
            else if (state == LevelState.Playing)
            {
                _view.SetSelectionVisible(true);
            }
            
            
            if (state != LevelState.Preparing)
            {
                return;
            }

            var horizontalOrigin = LevelManager.Instance.HorizontalOrigin.position.x;
            for (var i = 0; i < _inputBlocks.Count; i++)
            {
                var hPos = horizontalOrigin + i * (_blockPrefab.GetSize().x + _gridConfig.HorizontalGap);
                var pos = new Vector2(hPos, _verticalOrigin.position.y);
                _inputBlocks[i].transform.position = pos;
            }
        }

        private void OnChange()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            CameraView.Instance.DoShake(0.1f, 0.03f);

            _view.ConfirmSelection();
            CurrentBlock.Change();
            _view.ChangeForBlock(CurrentBlock.BlockType);
        }

        private void OnConfirm()
        {
            if (_locked)
            {
                return;
            }

            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            LevelManager.Instance.StartFalling = true;
            CameraView.Instance.DoShake(0.3f, 0.1f);
            CreateFlyRow();
        }

        private void OnRight()
        {
            _prevSelectedBlockIndex = _selectedBlockIndex;
            _selectedBlockIndex = (_selectedBlockIndex + 1) % _inputBlocks.Count;
            UpdateCurrentBlock();
        }

        private void OnLeft()
        {
            _prevSelectedBlockIndex = _selectedBlockIndex;
            _selectedBlockIndex = (_selectedBlockIndex - 1 + _inputBlocks.Count) % _inputBlocks.Count;
            UpdateCurrentBlock();
        }

        private void OnNumber(int number)
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            var index = number - 1;
            _inputBlocks[index].Change();
        }

        private void OnUseBooster()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            if (!BoostersManager.Instance.HasBoosterReady())
            {
                return;
            }

            StartCoroutine(DoLinesBreakerCoroutine());
        }

        private IEnumerator DoLinesBreakerCoroutine()
        {
            _locked = true;
            var mostBottomBlock = FallingBlocksModel.Instance.GetBottomBlock();
            if (mostBottomBlock == null)
            {
                yield break;
            }

            BlocksFallSystem.Instance.SetPaused(true);
            var verticalGap = FallingBlocksModel.Instance.FallingBlocks.First().VerticalGap;

            BoostersManager.Instance.UseFirstBooster();
            var rowPosition = mostBottomBlock.transform.position.y;

            for (var i = 0; i < 5; i++)
            {
                var effect = Instantiate(_rowDestroy, new Vector3(0, rowPosition), Quaternion.identity);
                effect.Setup(Color.red);
                yield return new WaitForSeconds(0.1f);
                CameraView.Instance.DoShake(0.3f, 0.4f);
                var allInRow = FallingBlocksModel.Instance.GetAllBlocksAtSameLine(rowPosition);
                foreach (var block in allInRow)
                {
                    FallingBlocksModel.Instance.BreakBlock(block);
                }

                yield return new WaitForSeconds(0.15f);
                rowPosition += verticalGap;
            }

            FallingBlocksModel.Instance.CheckForPassDown(rowPosition - verticalGap);

            BlocksFallSystem.Instance.SetPaused(false);
            _locked = false;
        }

        private void UpdateCurrentBlock(bool force = false)
        {
            if (!LevelManager.Instance.IsPlaying && !force)
            {
                return;
            }

            bool instant = Mathf.Abs(_prevSelectedBlockIndex - _selectedBlockIndex) > 1;

            for (int i = 0; i < _inputBlocks.Count; i++)
            {
                if (i == _selectedBlockIndex)
                {
                    _view.SetSelectionPos(_inputBlocks[i].transform.position, instant);
                }
            }

            _view.ChangeForBlock(CurrentBlock.BlockType);
            CameraView.Instance.DoShake(0.1f, 0.03f);
        }

        public void CreateFlyRow(bool selectedColumnOnly = false)
        {
            _view.SetSelectionVisible(false);

            _locked = true;
            _cooldownTween = DOVirtual.DelayedCall(_cooldown, Unlock);

            var newBlocks = new List<UserBlock>();

            foreach (var templateBlock in _inputBlocks)
            {
                if (selectedColumnOnly && templateBlock != CurrentBlock)
                {
                    continue;
                }

                if (TryFindTargetBlock(templateBlock.transform.position, out var targetBlock))
                {
                    var newBlock = BlockFactory.Instance.CreateUserBlock();
                    newBlock.transform.localPosition = templateBlock.transform.position;
                    newBlock.transform.localRotation = Quaternion.identity;
                    newBlock.transform.localScale = Vector3.one;
                    newBlock.SetType(templateBlock.BlockType);
                    newBlock.SetTargetBlock(targetBlock);
                    newBlocks.Add(newBlock);

                    var originalHeight = templateBlock.transform.position.y;
                    templateBlock.transform.position =
                        new Vector2(templateBlock.transform.position.x, _underScreenPosition.position.y);
                    templateBlock.transform.DOMoveY(originalHeight, _cooldown);
                }
            }

            foreach (var userBlock in newBlocks)
            {
                if (userBlock.TargetBlock == null || !userBlock.TargetBlock.BlockMechanicType.IsChainPart())
                {
                    continue;
                }

                var chainGuid = userBlock.TargetBlock.ChainGuid;
                var blocksInLine =
                    FallingBlocksModel.Instance.GetAllBlocksAtSameLine(userBlock.TargetBlock.transform.position.y);
                var blocksWithSameChain = blocksInLine.Where(b => b.ChainGuid == chainGuid).ToList();
                var areAllTargeted = blocksWithSameChain.All(b => newBlocks.Any(nb => nb.TargetBlock == b));
                if (!areAllTargeted)
                {
                    continue;
                }

                var userBlocksWithChainAsTarget = newBlocks.Where(nb => blocksWithSameChain.Contains(nb.TargetBlock)).ToList();
                var canAllBeDestroyed = userBlocksWithChainAsTarget.All(b => b.CanDestroyTarget());
                if (canAllBeDestroyed)
                {
                    blocksWithSameChain.ForEach(b => b.CanBeDestroyedAsChain = true);
                }
            }

            var sequence = new UserBlocksSequence(newBlocks);
            sequence.OnAllDestroyed += HandleSequenceComplete;

            void HandleSequenceComplete(UserBlocksSequence sequence)
            {
                sequence.OnAllDestroyed -= HandleSequenceComplete;
                OnSequenceCompleted?.Invoke(sequence);
            }
        }

        private bool TryFindTargetBlock(Vector3 origin, out FallingBlock fallingBlock)
        {
            var hit = Physics2D.Raycast(origin, Vector3.up, 20f, LayerMask.GetMask("FallingBlock"));
            if (hit.collider && hit.transform.TryGetComponent(out FallingBlock otherBlock))
            {
                fallingBlock = otherBlock;
                return true;
            }

            fallingBlock = null;
            return false;
        }

        private void Unlock()
        {
            _view.SetSelectionVisible(true);
            _locked = false;
        }
    }
}