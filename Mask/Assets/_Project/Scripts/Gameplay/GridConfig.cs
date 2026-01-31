using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "Gameplay/Grid Config")]
public class GridConfig : ScriptableObject
{
    public int Columns = 4;
    public int RowsPerSpawn = 3;
    public float FallSpeed = .5f;
    public float HorizontalGap = 1.1f;

    public DifficultySettings Start;
    public DifficultySettings Easy;
    public DifficultySettings Medium;
    public DifficultySettings Hard;
}

[Serializable]
public class DifficultySettings
{
    public float PassProbability = .05f;
    public float InvertedProbability = .05f;
    public float ChainProbability = .05f;
    public float InvertedPassProbability = .05f;
}
