using System;
using LazySloth.Observable;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private InputManager _input;
    
    private StartLevelSystem _startLevelSystem;
    private ResetLevelSystem _resetLevelSystem;

    private ObservableProperty<LevelState> _state = new(LevelState.Preparing);
    public IObservableProperty<LevelState> State => _state;

    public bool IsPlaying => State.Value == LevelState.Playing;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _startLevelSystem = new StartLevelSystem(_input, this);
        _resetLevelSystem = new ResetLevelSystem(_input, this);
    }

    private void OnDestroy()
    {
        _startLevelSystem.Dispose();
        _resetLevelSystem.Dispose();
    }

    public void SetPreparing() => _state.Value = LevelState.Preparing;

    public void SetPlaying() => _state.Value = LevelState.Playing;

    public void SetFinished() => _state.Value = LevelState.Finished;
}

public enum LevelState
{
    Preparing,
    Playing,
    Finished,
}