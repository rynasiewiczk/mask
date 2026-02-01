namespace _Project.Scripts.Gameplay.Input.LevelEnd
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Blocks;
    using DG.Tweening;
    using LazySloth.Utilities;
    using Spawning;
    using UnityEngine;

    public class LevelEndAnimationController : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private FallingBlocksModel _fallingBlocksModel;
        [SerializeField] private InputRow  _inputRow;
        [SerializeField] private FloatRange _initialColumnDropDelayRange = new(.5f, .9f);
        [SerializeField] private FloatRange _columnDelayBetweenBottomAndTopBlockRange = new(.8f, 1.6f);
        [SerializeField] private float _gravity = -7.5f;
        
        [SerializeField] private AudioSource _fallingBlocksAudio;

        private void Start()
        {
            LevelManager.Instance.State.Subscribe(HandleLevelEnd);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.State.Unsubscribe(HandleLevelEnd);
        }

        private void HandleLevelEnd(LevelState state)
        {
            if (state != LevelState.Finished)
            {
                return;
            }

            var blocks = _fallingBlocksModel.FallingBlocks;
            foreach (var inputBlock in _inputRow.InputBlocks)
            {
                var inputPosition = inputBlock.transform.position.x;
                var blocksInColumn = blocks.Where(b => Mathf.Abs(b.transform.position.x - inputPosition) < .2f).ToList();
                DropBlocks(blocksInColumn);
                DropBlock(inputBlock, .3f);
            }
        }

        private void DropBlocks(List<FallingBlock> blocksInColumn)
        {
            var orderedFromBottom = blocksInColumn.OrderBy(b => b.transform.position.y).ToList();
            var initialDelay = _initialColumnDropDelayRange.GetRandom();
            DOVirtual.DelayedCall(initialDelay, () =>
            {
                var timeSpan = _columnDelayBetweenBottomAndTopBlockRange.GetRandom();
                for (int i = 0; i < orderedFromBottom.Count(); i++)
                {
                    var normalizedIndex = i/(float)orderedFromBottom.Count;
                    var delay = timeSpan * normalizedIndex;
                    var i1 = i;
                    DOVirtual.DelayedCall(delay, () =>
                    {
                        DropBlock(orderedFromBottom.ElementAt(i1));
                        _fallingBlocksAudio.Play();
                    });
                }
            });
        }

        private async Task DropBlock(Block block, float initDelay = 0)
        {
            await Task.Delay((int)(initDelay* 1000));
            
            var velocity = _gravity * Time.deltaTime;

            while (block.transform.position.y > -12)
            {
                block.transform.Translate(0, velocity, 0);
                velocity += _gravity * Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }
        }
    }
}