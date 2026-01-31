namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using MoreMountains.Feedbacks;
    using UnityEngine;

    public class BlockView : MonoBehaviour
    {
        private static int _feelChannel = 0;

        [SerializeField] private Transform _viewTransform;
        [SerializeField] private BlockPassView _passView;
        [SerializeField] private BlockInvertedView _invertedView;
        [SerializeField] private BlockChainView _chainView;
        [SerializeField] private GameObject _oneObject;
        [SerializeField] private GameObject _zeroObject;
        [SerializeField] private GameObject _unknownObject;
        [SerializeField] private Transform _topBlockPosition;
        [SerializeField] private Transform _bottomBlockPosition;

        [SerializeField] private MMF_Player _feelPlayer;
        [SerializeField] private MMPositionShaker _feelShaker;

        public Transform TopBlockPosition => _topBlockPosition;
        public Transform BottomBlockPosition => _bottomBlockPosition;
        public Transform ViewTransform => _viewTransform;

        private BlockType _blockType;

        private void Awake()
        {
            _feelShaker.Channel = _feelChannel;
            foreach (var feedback in _feelPlayer.FeedbacksList)
            {
                feedback.Channel = _feelChannel;
            }

            _feelChannel++;
        }

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
                case BlockMechanicType.ChainEnd:
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
                case BlockMechanicType.ChainLeft:
                case BlockMechanicType.ChainRight:
                case BlockMechanicType.ChainBoth:
                    _chainView.SetActive(true);
                    _chainView.SetChain(blockMechanic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blockMechanic), blockMechanic, null);
            }
        }

        private void Clear()
        {
            SetUnknown(false);
            _chainView.SetActive(false);
            _invertedView.SetActive(false);
            _passView.SetActive(false);
        }

        public void ShowMissplay()
        {
            _feelPlayer.PlayFeedbacks();
        }
    }
}