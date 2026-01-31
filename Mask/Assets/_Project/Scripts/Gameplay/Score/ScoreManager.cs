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
        private ObservableProperty<int> _combo = new();

        public IObservableProperty<int> Score => _score;
        public IObservableProperty<int> Combo => _combo;

        public event Action<Vector2, int> OnLineCleared;

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
            AddPointsWithCombo(_config.BreakPoints);
        }

        private void AddClearedLinePoints(FallingBlock block)
        {
            AddPointsWithCombo(_config.ClearLinePoints);
            var points = _config.ClearLinePoints * _combo.Value;
            OnLineCleared?.Invoke(block.transform.position, points);
        }
        
        public void AddPerfectMovePoints(UserBlocksSequence sequence)
        {
            AddCombo();
            AddPointsWithCombo(_config.PerfectPoints);
        }

        private void AddPointsWithCombo(int points)
        {
            _score.Value += points * _combo.Value;
        }

        public void AddCombo()
        {
            _combo.Value++;
        }
        
        public void ResetCombo()
        {
            _combo.Value = 1;
        }
    }
}