using UnityEngine;

public class UiLevelStatePresenter : MonoBehaviour
{
    [SerializeField] private RectTransform _preparingStateContent;
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
        _preparingStateContent.gameObject.SetActive(state == LevelState.Preparing);
        _finishedStateContent.gameObject.SetActive(state == LevelState.Finished);
    }
}