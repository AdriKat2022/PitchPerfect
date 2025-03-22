using UnityEngine;
using UnityEngine.Events;

public class ClockAction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _loopDuration = 1f;
    [SerializeField] private UnityEvent _loopAction;
    [SerializeField] private bool _startOnAwake = true;

    private bool _isPlaying = false;
    private float _lastLoopTime;

    private void Awake()
    {
        if (_startOnAwake)
        {
            StartClock();
        }
    }

    // Start the clock
    public void StartClock()
    {
        _lastLoopTime = Time.time;
        _isPlaying = true;
    }

    // Perform the action based on the loop duration
    private void Update()
    {
        if (!_isPlaying) return;

        if (Time.time - _lastLoopTime >= _loopDuration)
        {
            _loopAction.Invoke();
            _lastLoopTime = Time.time;
        }
    }
}
