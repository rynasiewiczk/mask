    public class StartLevelSystem
    {
        private InputManager _input;
        private LevelManager _levelManager;

        public StartLevelSystem(InputManager input, LevelManager levelManager)
        {
            _input = input;
            _levelManager = levelManager;

            _input.OnChange += TryStartPlaying;
        }
        
        public void Dispose()
        {
            _input.OnChange -= TryStartPlaying;
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
