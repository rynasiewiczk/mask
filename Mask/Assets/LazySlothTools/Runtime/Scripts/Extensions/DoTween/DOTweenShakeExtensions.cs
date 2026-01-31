namespace LazySloth.Utilities.DoTween
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public static class DOTweenShakeExtensions
    {
        public static Tweener DOShakeScale(this Transform t, DOTweenShakeSettings settings)
        {
            return t.DOShakeScale(settings.Duration, settings.Punch, settings.Vibrato, settings.Randomness, settings.FadeOut);
        }
        
        public static Tweener DOShakePosition(this Transform t, DOTweenShakeSettings settings)
        {
            return t.DOShakePosition(settings.Duration, settings.Punch, settings.Vibrato, settings.Randomness,
                settings.Snapping, settings.FadeOut);
        }
        
        public static Tweener DOShakeRotation(this Transform t, DOTweenShakeSettings settings)
        {
            return t.DOShakeRotation(settings.Duration, settings.Punch, settings.Vibrato, settings.Randomness, settings.FadeOut);
        }
    }
    
    [Serializable]
    public class DOTweenShakeSettings
    {
        [SerializeField] private Vector3 _punch = default;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 1f;
        [SerializeField] private bool _snapping = false;
        [SerializeField] private bool _fadeOut = false;

        public Vector3 Punch => _punch;
        public float Duration => _duration;
        public int Vibrato => _vibrato;
        public float Randomness => _randomness;
        public bool Snapping => _snapping;
        public bool FadeOut => _fadeOut;

        public DOTweenShakeSettings(Vector3 punch, float duration, int vibrato = 10, float randomness = 1f, bool snapping = false, bool fadeOut = false)
        {
            _punch = punch;
            _duration = duration;
            _vibrato = vibrato;
            _randomness = randomness;
            _snapping = snapping;
            _fadeOut = fadeOut;
        }
    }
}