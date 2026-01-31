namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

    public class BlockView : MonoBehaviour
    {
        [SerializeField] private GameObject _oneObject;
        [SerializeField] private GameObject _zeroObject;
        [SerializeField] private GameObject _unknownObject;
        [SerializeField] private Transform _topBlockPosition;
        [SerializeField] private Transform _bottomBlockPosition;
        
        public Transform TopBlockPosition => _topBlockPosition;
        public Transform BottomBlockPosition => _bottomBlockPosition;
        
        public void SetBlockType(BlockType blockType)
        {
            _oneObject.SetActive(blockType == BlockType.One);
            _zeroObject.SetActive(blockType == BlockType.Zero);
        }

        public void SetUnknown(bool isUnknown)
        {
            _unknownObject.SetActive(isUnknown);
        }
    }
}