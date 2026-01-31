using LazySloth.Observable;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public ObservableProperty<LevelState> State = new(LevelState.Preparing);
    
    public bool IsPlaying => State.Value == LevelState.Playing;

    private void Awake()
    {
        Instance = this;
    }
}

public enum LevelState
{
    Preparing,
    Playing,
    Finished,
}