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
                case BlockMechanicType.LeftInverted:    
                    _pivot.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case BlockMechanicType.RightPass:
                case BlockMechanicType.RightInverted:
                    _pivot.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case BlockMechanicType.UpPass:
                case BlockMechanicType.UpInverted:
                    _pivot.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case BlockMechanicType.DownPass:
                case BlockMechanicType.DownInverted:
                    _pivot.localRotation = Quaternion.Euler(0, 0, 180);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blockMechanic), blockMechanic, null);
            }
        }
    }
}