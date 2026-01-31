using UnityEngine;

public class UiLevelStatePresenter : MonoBehaviour
{
    [SerializeField] private PreparingPanel _preparingPanel;
    [SerializeField] private RectTransform _finishedStateContent;

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
            _preparingPanel.Hide();
        }
        else if (state == LevelState.Preparing)
        {
            _preparingPanel.Show();
        }
        
        _finishedStateContent.gameObject.SetActive(state == LevelState.Finished);
    }
}