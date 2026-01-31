namespace _Project.Scripts.Gameplay.Input.Score
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ScoreConfig", menuName = "Gameplay/ScoreConfig", order = 0)]
    public class ScoreConfig : ScriptableObject
    {
        public int BreakPoints = 1;
        public int ClearLinePoints = 2;
        public int PerfectPoints = 3;
    }
}