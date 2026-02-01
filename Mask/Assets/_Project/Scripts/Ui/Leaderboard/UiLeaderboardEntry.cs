namespace _Project.Scripts.Ui.Leaderboard
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;

    public class UiLeaderboardEntry : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private TMP_Text _rankText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _nickNameText;
        [SerializeField] private Color _regularColor;
        [SerializeField] private Color _ourColor;
        [SerializeField] private GameObject _ourBg;

        public void Setup(int rank, int score, string nickname, bool isOurEntry)
        {
            _root.localScale = Vector3.zero;
            _root.DOScale(1f, 0.4f).SetDelay(rank * 0.2f);
            
            _rankText.text = rank.ToString();
            _scoreText.text = score.ToString();
            _nickNameText.text = nickname;
            
            _scoreText.color = isOurEntry ? _ourColor : _regularColor;
            _nickNameText.color = isOurEntry ? _ourColor : _regularColor;
            _ourBg.SetActive(isOurEntry);
        }
    }
}