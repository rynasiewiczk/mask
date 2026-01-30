namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using UnityEngine;

    public class InputRow : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Block[] _inputBlocks;

        private Block CurrentBlock => _inputBlocks[_selectedBlockIndex];
        private int _selectedBlockIndex = 0;

        private void Awake()
        {
            _inputManager.OnLeft += OnLeft;
            _inputManager.OnRight += OnRight;
            _inputManager.OnConfirm += OnConfirm;
            _inputManager.OnChange += OnChange;

            UpdateCurrentBlock();
        }

        private void OnChange()
        {
            CurrentBlock.Change();
        }

        private void OnConfirm()
        {
            //todo: add cooldown
            CreateFlyRow();
        }

        private void OnRight()
        {
            _selectedBlockIndex = (_selectedBlockIndex + 1) % _inputBlocks.Length;
            UpdateCurrentBlock();
        }

        private void OnLeft()
        {
            _selectedBlockIndex = (_selectedBlockIndex - 1 + _inputBlocks.Length) % _inputBlocks.Length;
            UpdateCurrentBlock();
        }

        private void UpdateCurrentBlock()
        {
            for (int i = 0; i < _inputBlocks.Length; i++)
            {
                _inputBlocks[i].SetSelected(i == _selectedBlockIndex);
            }
        }
        
        public void CreateFlyRow()
        {
            for (int i = 0; i < _inputBlocks.Length; i++)
            {
                var newBlock = BlockFactory.Instance.CreateBlock();
                newBlock.transform.parent = transform;
                newBlock.transform.localPosition = Vector3.zero;
                newBlock.transform.localRotation = Quaternion.identity;
                newBlock.transform.localScale = Vector3.one;
            }
        }
    }
}