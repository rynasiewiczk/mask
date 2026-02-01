namespace _Project.Scripts.Ui
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UiBoosterPresenter : MonoBehaviour
    {
        [SerializeField] private Image _progressImage;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private GameObject _fullIndicator;
        [SerializeField] private Color _regularColor;
        [SerializeField] private Color _fullColor;
        [SerializeField] private Transform _shift;

        private BoosterHandler _booster;

        private void Start()
        {
            _booster = BoostersManager.Instance.MainBooster;
            _booster.IsReadyToUse.Subscribe(Refresh);
            _booster.CurrentAmount.Subscribe(Refresh, true);
        }

        private void Refresh()
        {
            var newValue = (_booster.CurrentAmount.Value / (float)_booster.FullAmount);
            _progressImage.DOFillAmount(newValue, 0.3f);
            _progressText.text = $"{_booster.CurrentAmount.Value.ToString()}/{_booster.FullAmount}";
            _fullIndicator.SetActive(_booster.IsReadyToUse.Value);
            SetPulse(_booster.IsReadyToUse.Value);
            _progressText.color = _progressImage.color = _booster.IsReadyToUse.Value ? _fullColor : _regularColor;
        }

        private void SetPulse(bool pulse)
        {
            if (!pulse)
            {
                _shift.DOKill();
                _shift.localScale = Vector3.one;
            }
            else
            {
                _shift.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}