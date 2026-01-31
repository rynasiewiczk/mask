namespace _Project.Scripts.Gameplay.Input.Score
{
    using Blocks;
    using Spawning;

    public class ScorePerfectMovePointsSystem
    {
        private ScoreManager _scoreManager;
        private InputRow _inputRow;
        private GridConfig _gridConfig;

        private int _breaksInRow;
        
        public ScorePerfectMovePointsSystem(ScoreManager scoreManager, InputRow inputRow)
        {
            _scoreManager = scoreManager;
            _inputRow = inputRow;

            _inputRow.OnSequenceCompleted += HandleSequence;
        }

        public void Dispose()
        {
            _inputRow.OnSequenceCompleted -= HandleSequence;
        }

        private void HandleSequence(UserBlocksSequence sequence)
        {
            if (sequence.DidAnyJoinFalling)
            {
                return;
            }
            
            _scoreManager.AddPerfectMovePoints(sequence);
        }
    }
}