namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

    public class BlockView : MonoBehaviour
    {
        [SerializeField] private GameObject _selectedObject;
        [SerializeField] private GameObject _oneObject;
        [SerializeField] private GameObject _zeroObject;
        
        public void SetSelected(bool selected)
        {
            _selectedObject.SetActive(selected);
        }
        
        public void SetBlockType(BlockType blockType)
        {
            _oneObject.SetActive(blockType == BlockType.One);
            _zeroObject.SetActive(blockType == BlockType.Zero);
        }
    }
}