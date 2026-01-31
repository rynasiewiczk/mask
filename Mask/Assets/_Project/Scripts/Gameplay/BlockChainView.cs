namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

    public class BlockChainView : MonoBehaviour
    {
        [SerializeField] private GameObject _chainLeft;
        [SerializeField] private GameObject _chainRight;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetChain(BlockMechanicType blockMechanic)
        {
            _chainLeft.SetActive(blockMechanic == BlockMechanicType.ChainLeft || blockMechanic == BlockMechanicType.ChainBoth);
            _chainRight.SetActive(blockMechanic == BlockMechanicType.ChainRight || blockMechanic == BlockMechanicType.ChainBoth);
        }
    }
}