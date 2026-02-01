using System;
using DG.Tweening;
using LazySloth.Observable;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private InputManager _input;
    [SerializeField] private Transform _horizontalOrigin;
    
    private StartLevelSystem _startLevelSystem;
    private ResetLevelSystem _resetLevelSystem;

    private ObservableProperty<LevelState> _state = new(LevelState.Preparing);
    public IObservableProperty<LevelState> State => _state;

    public bool IsPlaying => State.Value == LevelState.Playing;
    public Transform HorizontalOrigin => _horizontalOrigin;

    public bool StartFalling { get; set; }

    private void Awake()
    {
        DOTween.SetTweensCapacity(300, 200);
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

    public void SetPreparing()
    {
        StartFalling = false;
        _state.Value = LevelState.Preparing;
    }

    public void SetPlaying()
    {
        StartFalling = false;
        _state.Value = LevelState.Playing;
    }

    public void SetFinished()
    {
        StartFalling = false;
        _state.Value = LevelState.Finished;
    } 
}

public enum LevelState
{
    Preparing,
    Playing,
    Finished,
}