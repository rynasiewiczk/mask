namespace _Project.Scripts.Ui.Leaderboard
{
    using System.Collections.Generic;
    using LootLocker.Requests;
    using Scripts.Leaderboard;
    using UnityEngine;

    public class UiLeaderboardPanel : MonoBehaviour
    {
        [SerializeField] private UiLeaderboardEntry _entryPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _loadingIndicator;

        private List<UiLeaderboardEntry> _createdEntries = new();
        
        private string _currentPlayerNickname;

        public void Init()
        {
            foreach (var entry in _createdEntries)
            {
                entry.gameObject.SetActive(false);
            }
            _loadingIndicator.SetActive(true);
        }
        
        public void Show(string currentPlayerNickname)
        {
            _loadingIndicator.SetActive(true);
            
            _currentPlayerNickname = currentPlayerNickname;
            LeaderboardManager.Instance.GetResults(OnResults);
        }

        private void OnResults(LootLockerGetScoreListResponse response)
        {
            _loadingIndicator.SetActive(false);
            var index = 0;
            foreach (var member in response.items)
            {
                var entry = GetInstance(index);
                entry.gameObject.SetActive(true);
                var isOurResult = _currentPlayerNickname == member.member_id;
                entry.Setup(member.rank, member.score, member.member_id,  isOurResult);
                
                index++;
            }
        }

        private UiLeaderboardEntry GetInstance(int index)
        {
            if (_createdEntries.Count <= index)
            {
                var newEntry = Instantiate(_entryPrefab, _content);
                _createdEntries.Add(newEntry);
            }
            
            return _createdEntries[index];
        }
    }
}