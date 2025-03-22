using System;
using System.Collections;
using UnityEngine;

public class ScalePulse : MonoBehaviour
{
    [Header("Squish")]
    [SerializeField] private float _scaleFactor = 0.1f;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] private SquashStretchAxis _axis = SquashStretchAxis.Y;

    [Header("On Beat")]
    [SerializeField] private bool _pulseOnBeat = true;
    [SerializeField] private bool _matchPulseDurationToBeat = false;
    [SerializeField] private float _pulseDuration = 1f;


    private Vector3 _initialScale;
    private bool _isPulsing = false;
    private Coroutine _pulseCoroutine;

    #region SquashStretchAxis
    [Flags]
    public enum SquashStretchAxis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }

    private bool _affectX => (_axis & SquashStretchAxis.X) == SquashStretchAxis.X;
    private bool _affectY => (_axis & SquashStretchAxis.Y) == SquashStretchAxis.Y;
    private bool _affectZ => (_axis & SquashStretchAxis.Z) == SquashStretchAxis.Z;
    #endregion

    private void Awake()
    {
        _initialScale = transform.localScale;
    }

    private void Start()
    {
        if (_matchPulseDurationToBeat)
        {
            _pulseDuration = RhythmCore.Instance.BeatInterval;
        }
    }

    private void OnEnable()
    {
        if (_pulseOnBeat)
        {
            RhythmCore.OnBeat += Pulse;
            if (_matchPulseDurationToBeat)
            {
                RhythmCore.OnTempoChange += OnTempoChange;
            }
        }
    }

    private void OnDisable()
    {
        if (_pulseOnBeat)
        {
            RhythmCore.OnBeat -= Pulse;
            if (_matchPulseDurationToBeat)
            {
                RhythmCore.OnTempoChange -= OnTempoChange;
            }
        }
    }

    private void Pulse()
    {
        if (_isPulsing && _pulseCoroutine != null)
        {
            StopCoroutine(_pulseCoroutine);
        }
        _isPulsing = true;
        _pulseCoroutine = StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        float timer = 0;
        while (_isPulsing && timer < _pulseDuration)
        {
            timer += Time.deltaTime;
            Vector3 scale = _initialScale;
            float scaleModifier = 1 + _animationCurve.Evaluate(timer / _pulseDuration) * _scaleFactor;

            if (_affectX)
            {
                scale.x *= scaleModifier;
            }
            else
            {
                scale.x /= scaleModifier;
            }

            if (_affectY)
            {
                scale.y *= scaleModifier;
            }
            else
            {
                scale.y /= scaleModifier;
            }

            if (_affectZ)
            {
                scale.z *= scaleModifier;
            }
            else
            {
                scale.z /= scaleModifier;
            }

            transform.localScale = scale;

            yield return null;
        }

        _isPulsing = false;
    }

    private void OnTempoChange(float tempo)
    {
        if (_matchPulseDurationToBeat)
        {
            Debug.Log("Pulse duration changed to match beat interval" + _pulseDuration);
            _pulseDuration = 1 / tempo;
        }
    }
}
