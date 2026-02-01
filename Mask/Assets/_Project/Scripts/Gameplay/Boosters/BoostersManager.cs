using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Gameplay.Input;
using _Project.Scripts.Gameplay.Input.Blocks;
using _Project.Scripts.Gameplay.Spawning;
using LazySloth.Observable;
using UnityEngine;

public class BoostersManager : MonoBehaviour
{
    public static BoostersManager Instance { get; private set; }

    [SerializeField] private int _mainBoosterPointsRequired = 100;
    
    [SerializeField] private int _boostersCount;
    [SerializeField] private float _useCooldown;

    public List<BoosterHandler> Boosters = new();
    public BoosterHandler MainBooster => Boosters.First();
    
    private void Awake()
    {
        Instance = this;
        for (var i = 0; i < _boostersCount; i++)
        {
           Boosters.Add(new BoosterHandler(_mainBoosterPointsRequired));
        }

    }

    private void Start()
    {
        FallingBlocksModel.Instance.OnBlockBreak += UpdateBoosters;
        LevelManager.Instance.State.Subscribe(ResetOnRestart);
    }

    private void ResetOnRestart(LevelState state)
    {
        if (state == LevelState.Preparing)
        {
            foreach (var booster in Boosters)
            {
                booster.Reset();
            }
        }
    }
    
    private void UpdateBoosters(FallingBlock block)
    {
        if(BlocksFallSystem.Instance.IsPaused) { return; } //ignore blocks destroyed when movement is paused

        foreach (var booster in Boosters)
        {
            booster.UpdateProgress();
        }
    }
    
    public bool HasBoosterReady() => Boosters.Any(x => x.IsReadyToUse.Value);
    
    public void UseFirstBooster()
    {
        foreach (var booster in Boosters)
        {
            if (booster.IsReadyToUse.Value)
            {
                booster.MarkUsed();
                return;
            }
        }
    }
}

public class BoosterHandler
{
    private float _cooldown;
    
    public ObservableProperty<bool> IsReadyToUse { get; } = new(); 

    public ObservableProperty<int> CurrentAmount { get; } = new();
    public int FullAmount => _fullAmount;

    private readonly int _fullAmount;
    
    public void UpdateProgress()
    {
        if(IsReadyToUse.Value) { return; }
        CurrentAmount.Value += 1;
        
        IsReadyToUse.Value = CurrentAmount.Value >= _fullAmount;
    }

    public void Reset()
    {
        IsReadyToUse.Value = true;
        CurrentAmount.Value = _fullAmount;
    }
    
    public BoosterHandler(int fullAmount)
    {
        IsReadyToUse.Value = true;
        _fullAmount = fullAmount;
        CurrentAmount.Value = _fullAmount;
    }

    public void MarkUsed()
    {
        IsReadyToUse.Value = false;
        CurrentAmount.Value = 0;
    }
}