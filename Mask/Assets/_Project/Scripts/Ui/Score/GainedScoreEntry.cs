namespace _Project.Scripts.Ui
{
    using System;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;

    public class GainedScoreEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        [SerializeField] private float _firstDuration = 2;
        [SerializeField] private float _secondDuration = 2;
        [SerializeField] private float _firstDistance = 50;
        [SerializeField] private float _secondDistance = 200;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Tween _tween;

        private void OnDisable()
        {
            _tween?.Kill();
        }

        public void Setup(int score, Vector2 origin)
        {
            _text.text = score.ToString();
            transform.position = origin;
            
            var rectTransform = transform as RectTransform;
            rectTransform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0;
            
            rectTransform.DOScale(1, _firstDuration);
            _canvasGroup.DOFade(1, _firstDuration);
            _tween = rectTransform.DOMoveY(origin.y + _firstDistance, _firstDuration).OnComplete(() =>
            {
                _canvasGroup.DOFade(0, _secondDuration);
                rectTransform.DOMoveY(rectTransform.position.y + _secondDistance, _secondDuration).OnComplete(DeleteEntry);
            });
        }

        private void DeleteEntry() => Destroy(gameObject);
    }
}