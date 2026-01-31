namespace _Project.Scripts.Gameplay.Input
{
    using DG.Tweening;
    using UnityEngine;

    public class InputRow : MonoBehaviour
    {
        [SerializeField] private InputRowView _view;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Block[] _inputBlocks;
        [SerializeField] private float _cooldown = 0.75f;
        [SerializeField] private Transform _underScreenPosition;

        private Block CurrentBlock => _inputBlocks[_selectedBlockIndex];
        private int _prevSelectedBlockIndex = 0;
        private int _selectedBlockIndex = 0;

        private bool _locked = false;
        private Tween _cooldownTween;

        private void Awake()
        {
            _inputManager.OnLeft += OnLeft;
            _inputManager.OnRight += OnRight;
            _inputManager.OnConfirm += OnConfirm;
            _inputManager.OnChange += OnChange;

            _view.SetSelectionPos(_inputBlocks[_selectedBlockIndex].transform.position, true);
            UpdateCurrentBlock();
        }

        private void OnChange()
        {
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
            _selectedBlockIndex = (_selectedBlockIndex + 1) % _inputBlocks.Length;
            UpdateCurrentBlock();
        }

        private void OnLeft()
        {
            _prevSelectedBlockIndex = _selectedBlockIndex;
            _selectedBlockIndex = (_selectedBlockIndex - 1 + _inputBlocks.Length) % _inputBlocks.Length;
            UpdateCurrentBlock();
        }

        private void UpdateCurrentBlock()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            bool instant = Mathf.Abs(_prevSelectedBlockIndex - _selectedBlockIndex) > 1;

            for (int i = 0; i < _inputBlocks.Length; i++)
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
                var newBlock = BlockFactory.Instance.CreateUserBlock();
                newBlock.transform.localPosition = templateBlock.transform.position;
                newBlock.transform.localRotation = Quaternion.identity;
                newBlock.transform.localScale = Vector3.one;

                newBlock.SetType(templateBlock.BlockType);
                var originalHeight = templateBlock.transform.position.y;
                templateBlock.transform.position =
                    new Vector2(templateBlock.transform.position.x, _underScreenPosition.position.y);
                templateBlock.transform.DOMoveY(originalHeight, _cooldown);
            }
        }

        private void Unlock()
        {
            _view.SetSelectionVisible(true);
            _locked = false;
        }
    }
}