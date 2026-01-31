namespace _Project.Scripts.Gameplay.Input
{
    using System.Linq;
    using DG.Tweening;
    using LazySloth.Utilities.DoTween;
    using UnityEngine;

    public class CameraView : MonoBehaviour
    {
        public static CameraView Instance { get; private set; }

        [SerializeField] private Camera _camera;
        [SerializeField] private InputRow _inputRow;
        [SerializeField] private Transform _cameraShake;
        [SerializeField] private Transform _cameraDangerShake;
        
        public float CameraSize => _camera.orthographicSize;

        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            var blocks = _inputRow.InputBlocks;
            var leftBlockPos = blocks.Min(b => b.transform.position.x);
            var rightBlockPos = blocks.Max(b => b.transform.position.x);
            var middle = Mathf.Lerp(leftBlockPos, rightBlockPos, 0.5f);
            transform.position = new Vector3(middle, transform.position.y, transform.position.z);
        }

        public void DoShake(float duration, float strength, float timeFreeze = 0f)
        {
            void DoSakeNow() => _cameraShake.DOShakePosition(duration, strength);
            _cameraShake.DOKill();
            _cameraShake.transform.localPosition = Vector3.zero;
            if (timeFreeze > 0)
            {
                Time.timeScale = 0;
                DOVirtual.DelayedCall(timeFreeze, () => Time.timeScale = 1).OnComplete(DoSakeNow);
            }
            else
            {
                DoSakeNow();
            }
            
        }

        public Tween DoDangerShake(DOTweenShakeSettings settings)
        {
            return _cameraDangerShake.DOShakePosition(settings);
        }
        
        public void SetSize(float size) => _camera.orthographicSize = size;
        
        public Tween DoSize(float size, float duration, Ease ease)
        {
            return _camera.DOOrthoSize(size, duration).SetEase(ease);
        }

        public Tween DoResetShake(float toDefaultDuration)
        {
            return _cameraDangerShake.DOLocalMove(Vector3.zero, toDefaultDuration);
        }
    }
}