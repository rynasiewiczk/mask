public class ResetLevelSystem
{
    private InputManager _input;
    private LevelManager _levelManager;

    public ResetLevelSystem(InputManager input, LevelManager levelManager)
    {
        _input = input;
        _levelManager = levelManager;

        _input.OnConfirm += TryResetLevel;
    }

    public void Dispose()
    {
        _input.OnConfirm -= TryResetLevel;
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