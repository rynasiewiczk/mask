namespace _Project.Scripts.Gameplay.Spawning
{
    public class ResetFallingBlocksSystem
    {
        private LevelManager _levelManager;
        private FallingBlocksModel _fallingBlocksModel;

        public ResetFallingBlocksSystem(LevelManager levelManager, FallingBlocksModel fallingBlocksModel)
        {
            _levelManager = levelManager;
            _fallingBlocksModel = fallingBlocksModel;
            
            _levelManager.State.Subscribe(ResetOnPreparing);
        }

        public void Dispose()
        {
            _levelManager.State.Unsubscribe(ResetOnPreparing);
        }

        private void ResetOnPreparing(LevelState state)
        {
            if (state != LevelState.Preparing)
            {
                return;
            }

            _fallingBlocksModel.ClearBlocks();
        }
    }
}