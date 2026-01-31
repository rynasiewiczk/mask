public class ResetLevelSystem
{
    private InputManager _input;
    private LevelManager _levelManager;

    public ResetLevelSystem(InputManager input, LevelManager levelManager)
    {
        _input = input;
        _levelManager = levelManager;

        _input.OnReset += TryResetLevel;
    }

    public void Dispose()
    {
        _input.OnReset -= TryResetLevel;
    }

    private void TryResetLevel()
    {
        if (_levelManager.State.Value != LevelState.Finished)
        {
            return;
        }

        _levelManager.SetPreparing();
    }
}