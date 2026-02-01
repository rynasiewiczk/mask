namespace _Project.Scripts.Ui
{
    using System;
    using DG.Tweening;
    using Gameplay.Input.Score;
    using LazySloth.Utilities.DoTween;
    using TMPro;
    using UnityEngine;

    public class UiComboPresenter : MonoBehaviour
    {
        public static UiComboPresenter Instance { get; private set; }
        
        [SerializeField] private DOTweenPunchSettings _punchSettings;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private Transform _comboContainer;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _midColor;
        [SerializeField] private Color _endColor;
        
        private Tween _tween;

        private void Awake()
        {
            Instance = this;
        }

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
            _comboText.text = $"{combo}";

            HandlePunch(combo);
        }

        [ContextMenu("Update Combo Texts")]
        private void TextPunch() => HandlePunch(3);
        
        private int _previousCombo;
        
        private void HandlePunch(int combo)
        {
            var colorToUse = GetColor(combo);
            _comboText.DOColor(colorToUse, 0.3f);
            
            if (combo <= _previousCombo)
            {
                _previousCombo = combo;
                return;
            }
            
            _previousCombo = combo;
            var newScale = GetScale(combo);
            
            _tween?.Kill();
            _comboContainer.transform.localScale = Vector3.one;

            _comboText.transform.DOScale(newScale, 0.3f).SetEase(Ease.OutBounce);
            _comboText.transform.DOPunchRotation(_punchSettings);
            
            if (combo > 1)
            {
                _tween = _comboContainer.transform.DOPunchScale(_punchSettings);
            }
        }

        public float GetScale(int combo)
        {
            return Mathf.Lerp(1f, 2.5f, Mathf.InverseLerp(1, 25, combo));
        }

        public Color GetColor(int combo)
        {
            return combo > 20 ? _endColor : combo >  10  ? _midColor : _startColor;
        }
    }
}