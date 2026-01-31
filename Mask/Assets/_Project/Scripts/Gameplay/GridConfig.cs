using System;
using System.Collections.Generic;
using System.Linq;
using LazySloth.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "Gameplay/Grid Config")]
public class GridConfig : ScriptableObject
{
    public int Columns = 4;
    public int RowsPerSpawn = 3;
    public float HorizontalGap = 1.1f;
    public float AfterProgressionFallSpeedRatePerSpawn = .01f;

    public DifficultySettings Start;
    [SerializeField] private List<DifficultyRange> _difficulties;

    public float GetFallSpeed(int spawnNumber)
    {
        var difficulty = GetDifficultyEntry(spawnNumber);

        if (difficulty == _difficulties.First())
        {
            var normalizedFallSpeed = spawnNumber / (float)difficulty.MaxSpawnNumber;
            return difficulty.settings.FallSpeedRange.Evaluate(normalizedFallSpeed);
        }

        if (difficulty == _difficulties.Last() && spawnNumber > difficulty.MaxSpawnNumber)
        {
            var steps = spawnNumber - difficulty.MaxSpawnNumber;
            var extraSpeed = steps * AfterProgressionFallSpeedRatePerSpawn;
            return difficulty.settings.FallSpeedRange.Max + extraSpeed;
        }

        var prevDifficulty = GetDifficultyEntry(spawnNumber - 1);
        var normalizedSpeed = Mathf.InverseLerp(prevDifficulty.MaxSpawnNumber, difficulty.MaxSpawnNumber, spawnNumber);
        return difficulty.settings.FallSpeedRange.Evaluate(normalizedSpeed);
    }

    public DifficultySettings GetDifficultySettings(int spawnNumber)
    {
        var entry =GetDifficultyEntry(spawnNumber);
        return entry?.settings ?? _difficulties.Last().settings;
    }

    private DifficultyRange GetDifficultyEntry(int spawnNumber)
    {
        var entry = _difficulties.FirstOrDefault(d => d.MaxSpawnNumber >= spawnNumber);
        return entry;
    }

}

[Serializable]
public class DifficultySettings
{
    public FloatRange FallSpeedRange = new();
    public float PassProbability = .05f;
    public float InvertedProbability = .05f;
    public float ChainProbability = .05f;
    public float InvertedPassProbability = .05f;
}

[Serializable]
public class DifficultyRange
{
    public int MaxSpawnNumber;
    public DifficultySettings settings;
}
