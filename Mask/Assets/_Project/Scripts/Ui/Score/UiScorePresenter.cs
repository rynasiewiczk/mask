namespace _Project.Scripts.Ui
{
    using Gameplay.Input.Score;
    using TMPro;
    using UnityEngine;

    public class UiScorePresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Start()
        {
            ScoreManager.Instance.Score.Subscribe(ChangeScore, true);
        }

        private void OnDestroy()
        {
            ScoreManager.Instance.Score.Unsubscribe(ChangeScore);
        }

        private void ChangeScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}