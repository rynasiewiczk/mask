namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using Spawning;
    using UnityEngine;

    public class BlocksFallSystem : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private FallingBlocksModel _model;

        private void LateUpdate()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            var blocks = _model.FallingBlocks;
            foreach (var block in blocks)
            {
                block.Fall(_gridConfig.FallSpeed * Time.deltaTime);
            }
        }
    }
}