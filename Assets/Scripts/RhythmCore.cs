using System;
using System.Collections;
using UnityEngine;

public class RhythmCore : MonoBehaviour
{
    public static RhythmCore Instance { get; private set; }

    // Clock event
    public static event Action OnBeat;

    [SerializeField] private bool _activateOnAwake;
    [SerializeField] private float _globalSpeed;

    private bool _isRunning = false;
    private float _lastBeatTime;

    public float LastBeatTime => _lastBeatTime;
    public float BeatInterval => 1f / _globalSpeed;

    public static float GetBeatDistance()
    {
        return Mathf.Min(Mathf.Abs(Instance.LastBeatTime - Time.time), Mathf.Abs(Instance.LastBeatTime + Instance.BeatInterval - Time.time));
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
