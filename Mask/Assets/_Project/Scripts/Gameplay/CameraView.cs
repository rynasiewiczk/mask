namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using System.Linq;
    using UnityEngine;

    public class CameraView : MonoBehaviour
    {
        [SerializeField] private InputRow _inputRow;
        
        private void Start()
        {
            var blocks = _inputRow.InputBlocks;
            var leftBlockPos = blocks.Min(b => b.transform.position.x);
            var rightBlockPos = blocks.Max(b => b.transform.position.x);
            var middle = Mathf.Lerp(leftBlockPos, rightBlockPos, 0.5f);
            transform.position = new Vector3(middle, transform.position.y, transform.position.z);
        }
    }
}