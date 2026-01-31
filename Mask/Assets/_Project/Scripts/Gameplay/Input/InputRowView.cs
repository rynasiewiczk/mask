namespace _Project.Scripts.Gameplay.Input
{
    using DG.Tweening;
    using UnityEngine;

    public class InputRowView : MonoBehaviour
    {
        [SerializeField] private Transform _selection;
        [SerializeField] private float _duration = .25f;
        
        private Tween _tween;
        private Tween _scaleTween;
        
        public void SetSelectionVisible(bool visible)
        {
            _selection.gameObject.SetActive(visible);
        }

        public void SetSelectionPos(Vector3 position, bool instant)
        {
            _tween?.Kill();
            
            if (instant)
            {
                _selection.position = position;
                return;
            }
            
            _tween =  _selection.DOMove(position, _duration);
        }

        public void ConfirmSelection()
        {
            _scaleTween?.Kill();
            _scaleTween = DOTween.Sequence()
                .Append(_selection.DOScale(Vector3.one * 0.92f, 0.06f))
                .Append(_selection.DOScale(Vector3.one, 0.12f).SetEase(Ease.OutBack));
        }
    }
}