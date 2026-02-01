namespace _Project.Scripts.Ui
{
    using DG.Tweening;
    using Gameplay.Input.Score;
    using Leaderboard;
    using LootLocker.Requests;
    using Scripts.Leaderboard;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UiLevelFinishedPanelPresenter : MonoBehaviour
    {
        [SerializeField] private Image _bgImage;
        [SerializeField] private CanvasGroup _resultPanel;
        [SerializeField] private CanvasGroup _leaderboardCanvasGroup;
        [SerializeField] private UiLeaderboardPanel _leaderboardPanel;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_InputField _nameInput;

        private bool _isVisible;
        private bool _isOnNamePage;
        private bool _isOnLeaderboardPage;
        private bool _scoreSubmitted;
        
        public void Show()
        {
            gameObject.SetActive(true);
            
            _scoreText.text = ScoreManager.Instance.Score.Value.ToString();
            _nameInput.text = string.Empty;
            
            _nameInput.interactable = true;
            _isOnNamePage = true;
            _isOnLeaderboardPage = false;
            _scoreSubmitted = false;
            _nameInput.ActivateInputField();
            _bgImage.DOFade(0f, 0f);
            _resultPanel.alpha = 0f;
            _leaderboardCanvasGroup.alpha = 0f;

            var delay = 2f;
            _bgImage.DOFade(1f, 0.5f).SetDelay(delay);
            _resultPanel.DOFade(1f, 0.5f).SetDelay(delay);
            (_resultPanel.transform as RectTransform).anchoredPosition = new Vector2(0, -500f);
            (_resultPanel.transform as RectTransform).DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutBack).SetDelay(delay);
            DOVirtual.DelayedCall(2f, () => _isVisible = true);
        }

        public void Hide()
        {
            _isVisible = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if(!_isVisible) { return; }

            if (_isOnNamePage)
            {
                if (!_nameInput.isFocused)
                {
                    _nameInput.ActivateInputField();
                    _nameInput.Select();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_isOnNamePage)
                {
                    if (_nameInput.text.Length <= 0)
                    {
                        _nameInput.transform.DOShakePosition(0.5f);
                    }
                    else
                    {
                        _nameInput.DeactivateInputField();
                        _isOnNamePage = false;
                        LeaderboardManager.Instance.SendResult(_nameInput.text, ScoreManager.Instance.Score.Value, OnScoreSubmitted);
                        _leaderboardPanel.Init();
                        
                        _resultPanel.DOFade(0f, 0.5f);
                        _leaderboardCanvasGroup.DOFade(1f, 0.5f);
                        (_leaderboardCanvasGroup.transform as RectTransform).anchoredPosition = new Vector2(0, -500f);
                        (_leaderboardCanvasGroup.transform as RectTransform).DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutBack);
                        
                        _isOnLeaderboardPage = true;
                    }
                }
                else if (_isOnLeaderboardPage)
                {
                    if (LevelManager.Instance.State.Value != LevelState.Finished)
                    {
                        return;
                    }

                    LevelManager.Instance.SetPreparing();
                }
            }
        }

        private void OnScoreSubmitted(LootLockerSubmitScoreResponse response)
        {
            _scoreSubmitted = true;
            _leaderboardPanel.Show(_nameInput.text);
        }
    }
}