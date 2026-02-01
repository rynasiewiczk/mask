using _Project.Scripts.Gameplay.Input.Score;
using _Project.Scripts.Ui;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShowPoints : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Transform _rootScale;
    
    public void Setup(int points)
    {
        var combo = ScoreManager.Instance.Combo.Value;
        
        var color = UiComboPresenter.Instance.GetColor(combo);
        var scale = Mathf.Lerp(1f, 1.6f, Mathf.InverseLerp(1, 25, combo));
            
        _rootScale.localScale = new Vector3(scale, scale, scale);
        _text.color = color;
        _text.text = $"+{points.ToString()}";
        DOVirtual.DelayedCall(3f, () => Destroy(gameObject));
    }
}
