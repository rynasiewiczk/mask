namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using Spawning;
    using UnityEngine;

    public class BlocksFallSystem : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private BlocksModel _model;

        private void FixedUpdate()
        {
            var blocks = _model.FallingBlocks;
            foreach (var block in blocks)
            {
                block.Fall(_gridConfig.fallSpeed * Time.fixedDeltaTime);
            }
        }
    }
}