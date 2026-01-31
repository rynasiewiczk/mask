namespace _Project.Scripts.Ui
{
    using Gameplay.Input.Score;
    using TMPro;
    using UnityEngine;

    public class UiComboPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _comboLabel;
        [SerializeField] private TextMeshProUGUI _comboText;

        private void Start()
        {
            ScoreManager.Instance.Combo.Subscribe(ChangeTexts, true);
        }

        private void OnDestroy()
        {
            ScoreManager.Instance.Combo.Unsubscribe(ChangeTexts);
        }

        private void ChangeTexts(int combo)
        {
            if (combo <= 1)
            {
                _comboLabel.gameObject.SetActive(false);
                _comboText.gameObject.SetActive(false);
                return;
            }
            
            _comboLabel.gameObject.SetActive(true);
            _comboText.gameObject.SetActive(true);
            _comboText.text = $"x{combo}";
        }
    }
}