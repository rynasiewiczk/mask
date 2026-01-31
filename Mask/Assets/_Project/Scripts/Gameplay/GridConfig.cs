using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "Gameplay/Grid Config")]
public class GridConfig : ScriptableObject
{
    public int Columns = 4;
    public int RowsPerSpawn = 3;
    public float FallSpeed = .5f;
    public float HorizontalGap = 1.1f;
    // public float VerticalGap = 1.1f;
}
