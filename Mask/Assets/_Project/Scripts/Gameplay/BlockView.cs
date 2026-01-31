namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;
    using UnityEngine.Serialization;

    public class BlockView : MonoBehaviour
    {
        [SerializeField] private GameObject _oneObject;
        [SerializeField] private GameObject _zeroObject;
        [SerializeField] private Transform _leftBlockPosition;
        [SerializeField] private Transform _topBlockPosition;
        [FormerlySerializedAs("_nextBlockPosition")] [SerializeField] private Transform _bottomBlockPosition;
        
        public Transform LeftBlockPosition => _leftBlockPosition;
        public Transform TopBlockPosition => _topBlockPosition;
        public Transform BottomBlockPosition => _bottomBlockPosition;
        
        public void SetBlockType(BlockType blockType)
        {
            _oneObject.SetActive(blockType == BlockType.One);
            _zeroObject.SetActive(blockType == BlockType.Zero);
        }
    }
}