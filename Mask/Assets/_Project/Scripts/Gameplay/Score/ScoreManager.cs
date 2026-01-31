namespace _Project.Scripts.Gameplay.Input.Score
{
    using System;
    using System.Linq;
    using Blocks;
    using LazySloth.Observable;
    using Spawning;
    using UnityEngine;

    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [SerializeField] private ScoreConfig _config;
        [SerializeField] private InputRow _inputRow;
        [SerializeField] private FallingBlocksModel _fallingBlocksModel;

        private ScorePerfectMovePointsSystem _perfectMovePointsSystem;
        
        private ObservableProperty<int> _score = new();

        public IObservableProperty<int> Score => _score;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LevelManager.Instance.State.Subscribe(ResetWhenPreparingState);
            _fallingBlocksModel.OnBlockBreak += AddPoints;

            _perfectMovePointsSystem = new ScorePerfectMovePointsSystem(this, _inputRow);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.State.Unsubscribe(ResetWhenPreparingState);
            _fallingBlocksModel.OnBlockBreak -= AddPoints;
            
            _perfectMovePointsSystem.Dispose();
        }

        private void ResetWhenPreparingState(LevelState state)
        {
            if (state != LevelState.Preparing)
            {
                return;
            }

            _score.Value = 0;
        }

        private void AddPoints(FallingBlock brokenBlock)
        {
            AddBreakBlockPoints(brokenBlock);

            var yPos = brokenBlock.transform.position.y;
            if (IsLineCleared())
            {
                AddClearedLinePoints(brokenBlock);
            }

            return;

            bool IsLineCleared()
            {
                var blocksInLine = _fallingBlocksModel.FallingBlocks
                    .Where(b => Mathf.Abs(b.transform.position.y - yPos) < .2f).ToList();
                return !blocksInLine.Any() || (blocksInLine.Count == 1 && blocksInLine.First() == brokenBlock);
            }
        }

        private void AddBreakBlockPoints(FallingBlock block)
        {
            _score.Value += _config.BreakPoints;
            Debug.LogWarning("BLOCK");
        }

        private void AddClearedLinePoints(FallingBlock block)
        {
            _score.Value += _config.ClearLinePoints;
            Debug.LogWarning("LINE");
        }
        
        public void AddPerfectMovePoints(UserBlocksSequence sequence)
        {
            _score.Value += _config.PerfectPoints;
            Debug.LogWarning("PERFECT");
        }
    }
}