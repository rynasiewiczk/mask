namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

    public class BlockView : MonoBehaviour
    {
        [SerializeField] private GameObject _oneObject;
        [SerializeField] private GameObject _zeroObject;
        [SerializeField] private GameObject _unknownObject;
        [SerializeField] private Transform _nextBlockPosition;
        public Transform NextBlockPosition => _nextBlockPosition;
        
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