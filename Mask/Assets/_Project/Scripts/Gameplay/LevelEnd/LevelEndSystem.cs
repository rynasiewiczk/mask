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

        [SerializeField] private Transform _cameraMinZoomHeight;
        [SerializeField] private Transform _cameraMaxZoomHeight;
        [SerializeField] private float _cameraLerpSpeed = .2f;

        [SerializeField] private AudioClip _levelEndCLip;
        
        private bool _isWarningPlaying;

        private Sequence _dangerLineSequence;
        private Sequence _cameraShakeSequence;

        private void OnDestroy()
        {
            _dangerLineSequence?.Kill();
            _cameraShakeSequence?.Kill();
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
                return;
            }
            
            UpdateCamera(lowestBlockHeight);

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
            _cameraShakeSequence?.Kill();
            AudioManager.Instance.HandleWarningSfx(0);
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

        private void UpdateCamera(float lowestBlockHeight)
        {
            var blocksHeightLerp = Mathf.InverseLerp(_cameraMinZoomHeight.transform.position.y,
                _cameraMaxZoomHeight.transform.position.y, lowestBlockHeight);
            blocksHeightLerp = Mathf.Clamp01(blocksHeightLerp);
            
            var targetCameraSize = Mathf.Lerp(_cameraDefaultSize, _cameraDangerSize, blocksHeightLerp);
            var currentCameraSize = CameraView.Instance.CameraSize;
            var nextCameraSize = Mathf.Lerp(currentCameraSize, targetCameraSize, _cameraLerpSpeed);
            CameraView.Instance.SetSize(nextCameraSize);
            
            var targetCameraPosition = Vector3.Lerp(_cameraDefaultPosition, _cameraDangerPosition, blocksHeightLerp);
            var currentCameraPosition = CameraView.Instance.transform.position;
            var nextCameraPosition = Vector3.Lerp(currentCameraPosition, targetCameraPosition, _cameraLerpSpeed);
            CameraView.Instance.transform.position = nextCameraPosition;
            
            AudioManager.Instance.HandleWarningSfx(blocksHeightLerp);
        }

        private void PlayCameraDanger()
        {
            _cameraShakeSequence?.Kill();
            _cameraShakeSequence = DOTween.Sequence();
            _cameraShakeSequence.Join(CameraView.Instance.DoDangerShake(_cameraShakeSettings));
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