namespace LazySloth.Utilities.DoTween
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public static class DOTweenPunchExtensions
    {
        public static Tweener DOPunchScale(this Transform t, DOTweenPunchSettings settings)
        {
            return t.DOPunchScale(settings.Punch, settings.Duration, settings.Vibrato, settings.Elasticity);
        }

        public static Tweener DOPunchPosition(this Transform t, DOTweenPunchSettings settings)
        {
            return t.DOPunchPosition(settings.Punch, settings.Duration, settings.Vibrato, settings.Elasticity,
                settings.Snapping);
        }

        public static Tweener DOPunchRotation(this Transform t, DOTweenPunchSettings settings)
        {
            return t.DOPunchRotation(settings.Punch, settings.Duration, settings.Vibrato, settings.Elasticity);
        }

    }

    [Serializable]
    public class DOTweenPunchSettings
    {
        [SerializeField] private Vector3 _punch = default;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 1f;
        [SerializeField] private bool _snapping = false;

        public Vector3 Punch
        {
            get => _punch;
            set => _punch = value;
        }

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public int Vibrato
        {
            get => _vibrato;
            set => _vibrato = value;
        }
        
        public float Elasticity
        {
            get => _elasticity;
            set => _elasticity = value;
        }

        public bool Snapping
        {
            get => _snapping;
            set => _snapping = value;
        }

        public DOTweenPunchSettings(Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f, bool snapping = false)
        {
            _punch = punch;
            _duration = duration;
            _vibrato = vibrato;
            _elasticity = elasticity;
            _snapping = snapping;
        }
    }
}