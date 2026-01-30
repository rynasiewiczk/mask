using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "Gameplay/Grid Config")]
public class GridConfig : ScriptableObject
{
    [Header("Spawn Settings")]
    [Tooltip("Number of rows to spawn at once")]
    public int rowsPerSpawn = 2;

    [Header("Fall Settings")]
    [Tooltip("Speed at which rows fall (units per second)")]
    public float fallSpeed = 2.0f;

}
