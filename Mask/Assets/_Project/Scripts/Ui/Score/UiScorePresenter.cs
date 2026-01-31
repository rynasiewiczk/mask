namespace _Project.Scripts.Ui
{
    using System.Linq;
    using DG.Tweening;
    using Gameplay.Input.Score;
    using TMPro;
    using UnityEngine;

    public class UiScorePresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private Tween _scoreTween;
        private int _currentScore = 0;
        
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
            const int totalLength = 8;

            // Kill previous animation if still running
            _scoreTween?.Kill();

            int startScore = _currentScore;

            _scoreTween = DOTween.To(
                () => startScore,
                x =>
                {
                    startScore = x;
                    _currentScore = x;

                    // Format with leading zeros
                    string full = x.ToString().PadLeft(totalLength, '0');

                    // Count leading zeros
                    int zeroCount = full.TakeWhile(c => c == '0').Count();

                    string zeros = full.Substring(0, zeroCount);
                    string number = full.Substring(zeroCount);

                    _scoreText.text = $"<color=grey>{zeros}</color>{number}";
                },
                score,
                0.6f // animation duration
            ).SetEase(Ease.OutCubic);
        }
        
        
    }
}