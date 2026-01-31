namespace _Project.Scripts.Gameplay.Input
{
    using System.Collections.Generic;
    using Blocks;
    using DG.Tweening;
    using UnityEngine;

    public class InputRow : MonoBehaviour
    {
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

        private void Awake()
        {
            var horizontalOrigin = LevelManager.Instance.HorizontalOrigin.position.x;
            for (int i = 0; i < _gridConfig.Columns; i++)
            {
                var hPos = horizontalOrigin + i * (_blockPrefab.GetSize().x +_gridConfig.HorizontalGap);
                var pos = new Vector2(hPos, _verticalOrigin.position.y);
                var block = Instantiate(_blockPrefab, pos, Quaternion.identity);
                _inputBlocks.Add(block);
            }
            
            _inputManager.OnLeft += OnLeft;
            _inputManager.OnRight += OnRight;
            _inputManager.OnNumber += OnNumber;
            _inputManager.OnConfirm += OnConfirm;
            _inputManager.OnChange += OnChange;

            _view.SetSelectionPos(_inputBlocks[_selectedBlockIndex].transform.position, true);
            UpdateCurrentBlock();
        }
        
        private void OnChange()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }
            
            CurrentBlock.Change();
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

        private void UpdateCurrentBlock()
        {
            if (!LevelManager.Instance.IsPlaying)
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
        }

        public void CreateFlyRow()
        {
            _view.SetSelectionVisible(false);

            _locked = true;
            _cooldownTween = DOVirtual.DelayedCall(_cooldown, Unlock);

            foreach (var templateBlock in _inputBlocks)
            {
                if (TryFindTargetBlock(templateBlock.transform.position, out var targetBlock))
                {
                    var newBlock = BlockFactory.Instance.CreateUserBlock();
                    newBlock.transform.localPosition = templateBlock.transform.position;
                    newBlock.transform.localRotation = Quaternion.identity;
                    newBlock.transform.localScale = Vector3.one;
                    newBlock.SetType(templateBlock.BlockType);
                    newBlock.SetTargetBlock(targetBlock);
                    
                    var originalHeight = templateBlock.transform.position.y;
                    templateBlock.transform.position =
                        new Vector2(templateBlock.transform.position.x, _underScreenPosition.position.y);
                    templateBlock.transform.DOMoveY(originalHeight, _cooldown);
                }
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
