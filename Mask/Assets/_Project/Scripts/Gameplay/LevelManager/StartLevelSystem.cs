    public class StartLevelSystem
    {
        private InputManager _input;
        private LevelManager _levelManager;

        public StartLevelSystem(InputManager input, LevelManager levelManager)
        {
            _input = input;
            _levelManager = levelManager;

            _input.OnConfirm += TryStartPlaying;
        }
        
        public void Dispose()
        {
            _input.OnConfirm -= TryStartPlaying;
        }

        private void TryStartPlaying()
        {
            if (_levelManager.State.Value != LevelState.Preparing)
            {
                return;
            }

            _levelManager.SetPlaying();
        }

        
    }
