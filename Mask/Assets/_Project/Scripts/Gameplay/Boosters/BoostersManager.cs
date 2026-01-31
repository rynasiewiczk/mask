using System.Collections.Generic;
using System.Linq;
using LazySloth.Observable;
using UnityEngine;

public class BoostersManager : MonoBehaviour
{
    public static BoostersManager Instance { get; private set; }

    [SerializeField] private int _boostersCount;
    [SerializeField] private float _useCooldown;

    public List<BoosterHandler> Boosters = new();
    
    private void Awake()
    {
        Instance = this;
        for (var i = 0; i < _boostersCount; i++)
        {
            Boosters.Add(new BoosterHandler(_useCooldown));
        }
    }

    private void Update()
    {
        foreach (var booster in Boosters)
        {
            if (booster.IsReadyToUse.Value)
            {
                continue;
            }
            
            booster.ReduceCooldown();
            break;
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

    public ObservableProperty<float> Cooldown { get; } = new();
    
    public void ReduceCooldown()
    {
        Cooldown.Value -= Time.deltaTime;
        if (Cooldown.Value <= 0f)
        {
            IsReadyToUse.Value = true;
        }
    }
    
    public BoosterHandler(float useCooldown)
    {
        IsReadyToUse.Value = true;
        Cooldown.Value = 0f;
        _cooldown = useCooldown;
    }

    public void MarkUsed()
    {
        IsReadyToUse.Value = false;
        Cooldown.Value = _cooldown;
    }
}