namespace _Project.Scripts.Gameplay.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using Spawning;
    using UnityEngine;

    public class LevelEndSystem : MonoBehaviour
    {
        [SerializeField] private Transform _warningHeight;
        [SerializeField] private Transform _endLine;
        [SerializeField] private FallingBlocksModel _fallingBlocksModel;

        [SerializeField] private Color _defaultLineColor;
        [SerializeField] private Color _warningLineColor1;
        [SerializeField] private Color _warningLineColor2;
        [SerializeField] private List<SpriteRenderer> _lines;
        private bool _isWarningPlaying;

        private Sequence _sequence;

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

            if (lowestBlockHeight < _warningHeight.position.y)
            {
                PlayWarning();
            }
            else
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
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            foreach (var line in _lines)
            {
                _sequence.Join(line.DOColor(_defaultLineColor, .3f));
            }
        }

        private void PlayWarning()
        {
            if (_isWarningPlaying)
            {
                return;
            }

            _isWarningPlaying = true;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            foreach (var line in _lines)
            {
                _sequence.Join(line.DOColor(_warningLineColor1, .3f)).OnComplete(() =>
                {
                    line.DOColor(_warningLineColor1, .3f);
                }).SetLoops(-1);
            }
        }
    }
}