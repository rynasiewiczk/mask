namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public class InputRowView : MonoBehaviour
    {
        [SerializeField] private Color _oneBlockColor;
        [SerializeField] private Color _zeroBlockColor;
        
        [SerializeField] private Transform _selection;
        [SerializeField] private float _duration = .25f;
        [SerializeField] private Transform _selectionTransform;
        [SerializeField] private SpriteRenderer _selectionRenderer;
        [SerializeField] private Transform _selectionFrameTransform;
        
        
        private Tween _tween;
        private Tween _scaleTween;


        public void SetSelectionVisible(bool visible)
        {
            _selection.gameObject.SetActive(visible);
        }

        public void ChangeForBlock(BlockType blockType)
        {
            var color = blockType == BlockType.One ? _oneBlockColor : _zeroBlockColor;
            _selectionRenderer.DOColor(color, .2f);
            _selectionTransform.DOKill();
            _selectionTransform.localScale = Vector3.one;
            _selectionTransform.DOPunchScale(Vector3.right * .2f, .2f);
        }

        public void SetSelectionPos(Vector3 position, bool instant)
        {
            _tween?.Kill();
            
            _selectionFrameTransform.DOKill();
            _selectionFrameTransform.localScale = Vector3.one;
            _selectionFrameTransform.DOPunchScale(Vector3.one * .15f, .3f);
            
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