public class ResetLevelSystem
{
    private InputManager _input;
    private LevelManager _levelManager;

    public ResetLevelSystem(InputManager input, LevelManager levelManager)
    {
        _input = input;
        _levelManager = levelManager;

        _input.OnNumber += TryResetLevel;
    }

    public void Dispose()
    {
        _input.OnNumber -= TryResetLevel;
    }

    private void TryResetLevel(int _)
    {
        if (_levelManager.State.Value != LevelState.Finished)
        {
            return;
        }

        _levelManager.SetPreparing();
    }
}