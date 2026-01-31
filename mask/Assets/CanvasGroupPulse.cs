using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupPulse : MonoBehaviour
{
    [Header("Alpha Range")]
    [Range(0f, 1f)] public float minAlpha = 0.35f;
    [Range(0f, 1f)] public float maxAlpha = 1.0f;

    [Header("Timing")]
    [Tooltip("Seconds for a full cycle (min->max->min).")]
    public float period = 1.0f;

    [Tooltip("Optional delay before pulsing starts.")]
    public float startDelay = 0f;

    [Header("Options")]
    [Tooltip("Use unscaled time (recommended for UI).")]
    public bool useUnscaledTime = true;

    [Tooltip("If true, start pulsing automatically on enable.")]
    public bool playOnEnable = true;

    [Tooltip("Keep current alpha when stopped. If false, snaps to maxAlpha.")]
    public bool keepAlphaOnStop = true;

    private CanvasGroup _cg;
    private float _t0;
    private bool _playing;

    void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (playOnEnable)
            Play(resetPhase: true);
    }

    void OnDisable()
    {
        // Optional: stop to avoid running while disabled
        _playing = false;
    }

    void Update()
    {
        if (!_playing) return;
        if (period <= 0.0001f) return;

        float time = useUnscaledTime ? Time.unscaledTime : Time.time;
        float elapsed = time - _t0;

        if (elapsed < startDelay)
            return;

        float p = (elapsed - startDelay) / period; // cycles
        // PingPong gives 0..1..0 repeating (triangle wave)
        float tri = Mathf.PingPong(p, 1f);

        // Smooth it for nicer ease-in/out
        float eased = Smooth01(tri);

        _cg.alpha = Mathf.Lerp(minAlpha, maxAlpha, eased);
    }

    // Public API
    public void Play(bool resetPhase = false)
    {
        if (resetPhase)
        {
            float time = useUnscaledTime ? Time.unscaledTime : Time.time;
            _t0 = time;
        }
        _playing = true;
    }

    public void Stop(bool snapToMax = false)
    {
        _playing = false;

        if (snapToMax || !keepAlphaOnStop)
            _cg.alpha = maxAlpha;
    }

    public void SetAlphaInstant(float a)
    {
        _cg.alpha = Mathf.Clamp01(a);
    }

    // Smoothstep-like easing (0..1 -> 0..1)
    private static float Smooth01(float x)
    {
        x = Mathf.Clamp01(x);
        return x * x * (3f - 2f * x);
    }
}