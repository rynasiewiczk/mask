namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

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
}