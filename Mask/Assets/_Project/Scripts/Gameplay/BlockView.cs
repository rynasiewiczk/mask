namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using UnityEngine;

    public class BlockPassView : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void SetPass(BlockMechanicType blockMechanic)
        {
            switch (blockMechanic)
            {
                case BlockMechanicType.LeftPass:
                    _pivot.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case BlockMechanicType.RightPass:
                    _pivot.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case BlockMechanicType.UpPass:
                    _pivot.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case BlockMechanicType.DownPass:
                    _pivot.localRotation = Quaternion.Euler(0, 0, 180);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blockMechanic), blockMechanic, null);
            }
        }
    }
    
    public class BlockInvertedView : MonoBehaviour
    {
        [SerializeField] private GameObject _invertedZeroView;
        [SerializeField] private GameObject _invertedOneView;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void SetInverted(BlockType blockType)
        {
            _invertedZeroView.SetActive(blockType == BlockType.Zero);
            _invertedOneView.SetActive(blockType == BlockType.One);
        }
    }

    public class BlockView : MonoBehaviour
    {
        [SerializeField] private BlockPassView _passView;
        [SerializeField] private BlockInvertedView _invertedView;
        
        [SerializeField] private GameObject _oneObject;
        [SerializeField] private GameObject _zeroObject;
        [SerializeField] private GameObject _unknownObject;
        [SerializeField] private Transform _nextBlockPosition;
        public Transform NextBlockPosition => _nextBlockPosition;
        
        private BlockType _blockType;
        
        public void SetBlockType(BlockType blockType)
        {
            _blockType = blockType;
            _oneObject.SetActive(blockType == BlockType.One);
            _zeroObject.SetActive(blockType == BlockType.Zero);
        }

        private void SetUnknown(bool isUnknown)
        {
            _unknownObject.SetActive(isUnknown);
        }

        public void SetMechanic(BlockMechanicType blockMechanic)
        {
            Clear();
            switch (blockMechanic)
            {
                case BlockMechanicType.None:
                    break;
                case BlockMechanicType.LeftPass:
                case BlockMechanicType.RightPass:
                case BlockMechanicType.UpPass:
                case BlockMechanicType.DownPass:
                    _passView.SetActive(true);
                    _passView.SetPass(blockMechanic);
                    break;
                case BlockMechanicType.Unknown:
                    SetUnknown(true);
                    break;
                case BlockMechanicType.Inverted:
                    _invertedView.SetActive(true);
                    _invertedView.SetInverted(_blockType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blockMechanic), blockMechanic, null);
            }
        }

        private void Clear()
        {
            SetUnknown(false);
            _invertedView.SetActive(false);
            _passView.SetActive(false);
        }
    }
}