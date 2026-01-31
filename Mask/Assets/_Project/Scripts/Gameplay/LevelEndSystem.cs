namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using System.Linq;
    using Spawning;
    using UnityEngine;

    public class LevelEndSystem : MonoBehaviour
    {
        [SerializeField] private Transform _endLine;
        [SerializeField] private FallingBlocksModel _fallingBlocksModel;

        private void Update()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            if (_fallingBlocksModel.FallingBlocks.Count == 0)
            {
                return;
            }

            var block = _fallingBlocksModel.FallingBlocks.First();

            var lowestBlockHeight = _fallingBlocksModel.FallingBlocks.Min(b => b.transform.position.y);
            if (lowestBlockHeight - block.GetSize().y / 2 < _endLine.position.y)
            {
                LevelManager.Instance.SetFinished();
            }
        }
    }
}