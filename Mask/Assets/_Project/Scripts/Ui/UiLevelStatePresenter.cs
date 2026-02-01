using _Project.Scripts.Ui;
using UnityEngine;

public class UiLevelStatePresenter : MonoBehaviour
{
    [SerializeField] private PreparingPanel _preparingPanel;
    [SerializeField] private UiLevelFinishedPanelPresenter _finishedPanel;
    [SerializeField] private StartGameHandler _startGameHandler;

    private void Start()
    {
        LevelManager.Instance.State.Subscribe(UpdateStatePanels, true);
    }

    private void OnDestroy()
    {
        LevelManager.Instance.State.Unsubscribe(UpdateStatePanels);
    }
    
    private void UpdateStatePanels(LevelState state)
    {
        if (state == LevelState.Playing)
        {
            _finishedPanel.Hide();
            _preparingPanel.Hide();
        }
        else if (state == LevelState.Preparing)
        {
            _finishedPanel.Hide();
            _preparingPanel.Show();
        }

        if (state == LevelState.Playing)
        {
            _startGameHandler.ShowGame();
        }
        else if (state == LevelState.Finished)
        {
            _finishedPanel.Show();
        }
    }
}