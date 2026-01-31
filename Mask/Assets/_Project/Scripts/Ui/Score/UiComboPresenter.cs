namespace _Project.Scripts.Ui
{
    using DG.Tweening;
    using Gameplay.Input.Score;
    using LazySloth.Utilities.DoTween;
    using TMPro;
    using UnityEngine;

    public class UiComboPresenter : MonoBehaviour
    {
        [SerializeField] private DOTweenPunchSettings _punchSettings;
        [SerializeField] private TextMeshProUGUI _comboText;
        
        private Tween _tween;

        private void Start()
        {
            ScoreManager.Instance.Combo.Subscribe(ChangeTexts, true);
        }

        private void OnDestroy()
        {
            ScoreManager.Instance.Combo.Unsubscribe(ChangeTexts);
            _tween?.Kill();
        }

        private void ChangeTexts(int combo)
        {
            _comboText.text = $"x{combo}";

            HandlePunch(combo);
        }

        [ContextMenu("Update Combo Texts")]
        private void TextPunch() => HandlePunch(3);
        
        private void HandlePunch(int combo)
        {
            _tween?.Kill();
            _comboText.transform.localScale = Vector3.one;

            if (combo > 1)
            {
                _tween = _comboText.transform.DOPunchScale(_punchSettings);
            }
        }
    }
}