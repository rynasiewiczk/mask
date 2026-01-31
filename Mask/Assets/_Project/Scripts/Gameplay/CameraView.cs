namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using System.Linq;
    using DG.Tweening;
    using UnityEngine;

    public class CameraView : MonoBehaviour
    {
        public static CameraView Instance { get; private set; }
        
        [SerializeField] private InputRow _inputRow;
        [SerializeField] private Transform _cameraShake;
        
        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            var blocks = _inputRow.InputBlocks;
            var leftBlockPos = blocks.Min(b => b.transform.position.x);
            var rightBlockPos = blocks.Max(b => b.transform.position.x);
            var middle = Mathf.Lerp(leftBlockPos, rightBlockPos, 0.5f);
            transform.position = new Vector3(middle, transform.position.y, transform.position.z);
        }

        public void DoShake(float duration, float strength)
        {
            _cameraShake.DOKill();
            _cameraShake.DOShakePosition(duration, strength);
        }
    }
}