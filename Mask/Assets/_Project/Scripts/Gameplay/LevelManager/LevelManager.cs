using System;
using _Project.Scripts;
using DG.Tweening;
using LazySloth.Observable;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private InputManager _input;
    [SerializeField] private Transform _horizontalOrigin;
    [SerializeField] private AudioClip _levelFinishedSfx;
    
    private StartLevelSystem _startLevelSystem;
    //private ResetLevelSystem _resetLevelSystem;

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
       // _resetLevelSystem = new ResetLevelSystem(_input, this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Time.timeScale = 1;
        }
    }

    private void OnDestroy()
    {
        _startLevelSystem.Dispose();
        //_resetLevelSystem.Dispose();
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
        AudioManager.Instance.PlaySfx(_levelFinishedSfx, .99f, 1f);
    }
}

public enum LevelState
{
    Preparing,
    Playing,
    Finished,
}