namespace _Project.Scripts
{
    using LazySloth.Utilities;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField] private AudioSource _warningSource;
        [SerializeField] private AudioSource _audioSource;

        [Space, SerializeField] private AudioClip _missmatchedBlocksSfx;
        
        [Space, SerializeField] private AudioClip _testClip1;
        [SerializeField] private float _testPitch1;
        
        [Space, SerializeField] private AudioClip _testClip2;
        [SerializeField] private float _testPitch2;

        private void Awake()
        {
            Instance = this;
        }

        public void PlaySfx(AudioClip clip, FloatRange pitchRange) => PlaySfx(clip, pitchRange.Min,  pitchRange.Max);
        
        public void PlaySfx(AudioClip clip, float pitchMin , float pitchMax )
        {
            var pitch = Random.Range(pitchMin, pitchMax);
            PlaySfx(clip, pitch);
        }
        
        private void PlaySfx(AudioClip clip, float pitch = 1f)
        {
            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(clip);
        }

        public void PlayMismatchedBlocksSfx() => PlaySfx(_missmatchedBlocksSfx, .95f, 1f);

        public void HandleWarningSfx(float normalizedDanger)
        {
            if (normalizedDanger <= 0)
            {
                if (_warningSource.isPlaying)
                {
                    _warningSource.Stop();
                }

                return;
            }
            
            if (!_warningSource.isPlaying)
            {
                _warningSource.Play();
            }
            
            _warningSource.volume = normalizedDanger;
        }
        
        [ContextMenu("Test1")]
        public void FireTest1() => PlaySfx(_testClip1, _testPitch1);
        

        [ContextMenu("Test2")]
        public void FireTest2() => PlaySfx(_testClip2, _testPitch2);
        
        
    }
}