namespace _Project.Scripts.Ui
{
    using System;
    using Gameplay.Input.Score;
    using TMPro;
    using UnityEngine;

    public class UiScorePresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        [SerializeField] private GainedScoreEntry _gainedScorePrefab;
        [SerializeField] private Transform _gainedScoresOrigin;

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

            if (score == 0)
            {
                return;
            }
            
            var prevScore = ScoreManager.Instance.Score.PreviousValue;
            var delta = score - prevScore;

            SpawnGainedScore(delta);
        }

        private void SpawnGainedScore(int value)
        {
            var instance = Instantiate(_gainedScorePrefab);
            instance.Setup(value, _gainedScoresOrigin.position);
        }
    }
}