namespace _Project.Scripts.Gameplay.Input
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using LazySloth.Utilities.DoTween;
    using Spawning;
    using UnityEngine;

    public class LevelEndSystem : MonoBehaviour
    {
        [SerializeField] private Transform _warningActivateHeight;
        [SerializeField] private Transform _warningDeactivateHeight;
        [SerializeField] private Transform _endLine;
        [SerializeField] private FallingBlocksModel _fallingBlocksModel;

        [SerializeField] private Color _defaultLineColor;
        [SerializeField] private Color _warningLineColor1;
        [SerializeField] private Color _warningLineColor2;
        [SerializeField] private List<SpriteRenderer> _lines;

        [SerializeField] private float _cameraDefaultSize;
        [SerializeField] private Vector3 _cameraDefaultPosition;
        [SerializeField] private float _cameraTDefaultDuration = 2;
        [SerializeField] private Ease _cameraToDefaultSizeEase = Ease.OutBack;

        [SerializeField] private float _cameraDangerSize;
        [SerializeField] private float _cameraToDangerDuration = .75f;
        [SerializeField] private Vector3 _cameraDangerPosition;
        [SerializeField] private Ease _cameraToDangerEase = Ease.OutCubic;
        [SerializeField] private DOTweenShakeSettings _cameraShakeSettings;

        private bool _isWarningPlaying;

        private Sequence _dangerLineSequence;
        private Sequence _cameraSequence;

        private void OnDestroy()
        {
            _dangerLineSequence?.Kill();
            _cameraSequence?.Kill();
        }

        private void Update()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                ResetWarning();
                return;
            }

            if (_fallingBlocksModel.FallingBlocks.Count == 0)
            {
                ResetWarning();
                return;
            }

            var block = _fallingBlocksModel.FallingBlocks.First();

            var lowestBlockHeight = _fallingBlocksModel.FallingBlocks.Min(b => b.transform.position.y);
            if (lowestBlockHeight - block.GetSize().y / 2 < _endLine.position.y)
            {
                LevelManager.Instance.SetFinished();
            }

            if (!_isWarningPlaying && lowestBlockHeight < _warningActivateHeight.position.y)
            {
                PlayWarning();
            }

            if (_isWarningPlaying && lowestBlockHeight > _warningDeactivateHeight.position.y)
            {
                ResetWarning();
            }
        }

        private void ResetWarning()
        {
            if (!_isWarningPlaying)
            {
                return;
            }

            _isWarningPlaying = false;
            ResetDangerLineWarning();
            ResetCameraWarning();
        }

        private void ResetDangerLineWarning()
        {
            _dangerLineSequence?.Kill();
            _dangerLineSequence = DOTween.Sequence();

            foreach (var line in _lines)
            {
                _dangerLineSequence.Join(line.DOColor(_defaultLineColor, .3f));
            }
        }

        private void ResetCameraWarning()
        {
            _cameraSequence?.Kill();
            _cameraSequence = DOTween.Sequence();
            _cameraSequence.Join(CameraView.Instance.transform.DOMove(_cameraDefaultPosition, _cameraTDefaultDuration));
            _cameraSequence.Join(CameraView.Instance.DoSize(_cameraDefaultSize, _cameraTDefaultDuration,
                _cameraToDefaultSizeEase));
            _cameraSequence.Join(CameraView.Instance.DoResetShake(_cameraTDefaultDuration));
        }

        private void PlayWarning()
        {
            if (_isWarningPlaying)
            {
                return;
            }

            _isWarningPlaying = true;
            PlayDangerLineWarning();
            PlayCameraDanger();
        }

        private void PlayCameraDanger()
        {
            _cameraSequence?.Kill();
            _cameraSequence = DOTween.Sequence();

            _cameraSequence.Join(CameraView.Instance.DoSize(_cameraDangerSize, _cameraToDangerDuration,
                _cameraToDangerEase));
            _cameraSequence.Join(CameraView.Instance.transform.DOMove(_cameraDangerPosition, _cameraTDefaultDuration)
                .SetEase(_cameraToDangerEase));
            _cameraSequence.Join(DOVirtual.DelayedCall(_cameraToDangerDuration / 2, () =>
            {
                _cameraSequence = DOTween.Sequence();
                _cameraSequence.Join(CameraView.Instance.DoDangerShake(_cameraShakeSettings));
            }));
        }

        private void PlayDangerLineWarning()
        {
            _dangerLineSequence?.Kill();
            _dangerLineSequence = DOTween.Sequence();

            foreach (var line in _lines)
            {
                _dangerLineSequence.Join(line.DOColor(_warningLineColor1, .3f)).OnComplete(() =>
                {
                    line.DOColor(_warningLineColor1, .3f);
                }).SetLoops(-1);
            }
        }
    }
}