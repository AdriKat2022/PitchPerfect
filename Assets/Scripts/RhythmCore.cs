using System;
using System.Collections;
using UnityEngine;

public class RhythmCore : MonoBehaviour
{
    public static RhythmCore Instance { get; private set; }

    // Clock event
    public static event Action OnBeat;
    public static event Action<float> OnTempoChange;

    [SerializeField] private bool _activateOnAwake;
    [SerializeField] private float _globalSpeed;
    [SerializeField] private bool _useBpm;
    [SerializeField] private float _bpm;

    private bool _isRunning = false;
    private float _lastBeatTime;

    public float LastBeatTime => _lastBeatTime;
    public float BeatInterval => 1f / _globalSpeed;

    public static float GetBeatDistance()
    {
        return Mathf.Min(Mathf.Abs(Instance.LastBeatTime - Time.time), Mathf.Abs(Instance.LastBeatTime + Instance.BeatInterval - Time.time));
    }

    public static float GetSignedBeatDistance()
    {
        // Return the distance to the nearest beat, with the sign indicating if the beat is in the past or in the future

        // Time since the last beat
        float lateDistance = Time.time - Instance.LastBeatTime;
        // Time before the next beat
        float earlyDistance = Instance.LastBeatTime + Instance.BeatInterval - Time.time;

        if (lateDistance < earlyDistance)
        {
            return lateDistance;
        }
        else
        {
            return -earlyDistance;
        }
    }

    private void OnValidate()
    {
        if (_useBpm)
        {
            _globalSpeed = _bpm / 60f;
        }

        OnTempoChange?.Invoke(_globalSpeed);
    }

    private void Awake()
    {
        Instance = this;

        if (!_activateOnAwake) return;
        StartClock();
    }

    public void StartClock()
    {
        if (_isRunning) return;

        _isRunning = true;

        StartCoroutine(Clock());
    }

    public void StopClock()
    {
        if (!_isRunning) return;
        _isRunning = false;
        StopAllCoroutines();
    }

    private IEnumerator Clock()
    {
        while (_isRunning)
        {
            yield return new WaitForSeconds(1f / _globalSpeed);
            OnBeat?.Invoke();
            _lastBeatTime = Time.time;
        }
    }
}
