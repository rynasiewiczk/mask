namespace _Project.Scripts.Leaderboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LootLocker.Requests;
    using UnityEngine;

    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error starting LootLocker session");

                    return;
                }

                Debug.Log("successfully started LootLocker session");
            });
        }
        
        private const string LeaderboardKey = "bit_mask_leaderboard"; 

        public void SendResult(string nickname, int score, Action<LootLockerSubmitScoreResponse> callback)
        {
            LootLockerSDKManager.SubmitScore(nickname, score, LeaderboardKey, callback);
        }

        public void GetResults(Action<LootLockerGetScoreListResponse> callback)
        {
            LootLockerSDKManager.GetScoreList(LeaderboardKey, 2000, 0, callback);
        }
    }
}