namespace _Project.Scripts.Ui
{
    using DG.Tweening;
    using Gameplay.Input.Score;
    using TMPro;
    using UnityEngine;

    public class UiLineCleared : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private RectTransform _container;
        [SerializeField] private RectTransform _imageRect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _scaleUpDuration = .5f;
        [SerializeField] private float _delay = 1.5f;
        [SerializeField] private float _fadeOutDuration = .5f;

        [SerializeField] private TextMeshProUGUI _text;

        private Tween _tween;

        private void Start()
        {
            ScoreManager.Instance.OnLineCleared += ShowEffect;
            
            HideInstant();
        }

        private void OnDestroy()
        {
            ScoreManager.Instance.OnLineCleared -= ShowEffect;
            _tween?.Kill();
        }

        private void HideInstant()
        {
            _container.transform.localScale = new Vector2(1, 0);
            _imageRect.transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
        }

        private void ShowEffect(Vector2 position, int points)
        {
            var screenPosition = _camera.WorldToScreenPoint(position);
            _container.anchoredPosition = new Vector2(_container.anchoredPosition.x, screenPosition.y);
            _text.text = $"Line cleared  +{points}";

            PlayAnimation();
        }

        private void PlayAnimation()
        {
            _tween?.Kill();
            var sequence = DOTween.Sequence();

            HideInstant();

            sequence.Join(_container.DOScaleY(1, _scaleUpDuration));
            sequence.Join(DOVirtual.DelayedCall(_delay, () =>
            {
                _canvasGroup.DOFade(0, _fadeOutDuration);
                _imageRect.DOScaleY(2, _fadeOutDuration * 2);
            }));

            _tween = sequence;
        }
    }
}