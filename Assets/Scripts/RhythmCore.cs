using System;
using UnityEngine;

public class RhythmCore : MonoBehaviour
{
    public static RhythmCore Instance { get; private set; }

    // Clock event
    public static event Action OnBeat;
    public static event Action<float> OnTempoChange;

    [SerializeField] private float _acceleration = 1f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _activateOnAwake;
    [SerializeField] private float _globalSpeed;
    [SerializeField] private bool _useBpm;
    [SerializeField] private float _bpm;

    private bool _isRunning = false;
    private double _lastBeatTime;
    private float _secondsPerBeat;
    private float _songStartTime;
    private int _lastBeat;

    public double LastBeatTime => _lastBeatTime;
    public double BeatInterval => 1f / (_globalSpeed * _acceleration);

    #region Static Methods
    public static void ChangeTempo(float newTempo)
    {
        Instance._globalSpeed = newTempo;
        OnTempoChange?.Invoke(newTempo);
    }

    public static void ChangeTempoBpm(float newTempo)
    {
        Instance._bpm = newTempo;
        Instance._globalSpeed = newTempo / 60f;
        OnTempoChange?.Invoke(newTempo);
    }

    public static double GetBeatDistance()
    {
        return Mathf.Min(Mathf.Abs((float)(Instance.LastBeatTime - AudioSettings.dspTime)), Mathf.Abs((float)(Instance.LastBeatTime + Instance.BeatInterval - AudioSettings.dspTime)));
    }

    public static double GetSignedBeatDistance()
    {
        // Return the distance to the nearest beat, with the sign indicating if the beat is in the past or in the future

        // Time since the last beat
        double lateDistance = AudioSettings.dspTime - Instance.LastBeatTime;
        // Time before the next beat
        double earlyDistance = Instance.LastBeatTime + Instance.BeatInterval - AudioSettings.dspTime;

        if (lateDistance < earlyDistance)
        {
            return lateDistance;
        }
        else
        {
            return -earlyDistance;
        }
    }

    public static void StartClock()
    {
        Instance.StartClocking();
    }

    public static void PauseClock()
    {
        Instance.PauseClocking();
    }
    #endregion

    #region Control Methods
    public void StartClocking()
    {
        if (_isRunning) return;

        ComputeSongInfo();
        _audioSource.Play();
        _isRunning = true;
    }

    public void PauseClocking()
    {
        if (!_isRunning) return;

        _isRunning = false;
        _audioSource.Pause();
    }

    public void ResumeClocking()
    {
        if (_isRunning) return;

        _isRunning = true;
        _audioSource.Play();
    }
    #endregion

    private void OnValidate()
    {
        if (_useBpm)
        {
            _globalSpeed = _bpm / 60f;
        }

        _audioSource.pitch = _acceleration;

        OnTempoChange?.Invoke(_globalSpeed * _acceleration);

        ComputeSongInfo();
    }

    private void ComputeSongInfo()
    {
        _secondsPerBeat = 60f / _bpm / _acceleration;
        _songStartTime = (float)AudioSettings.dspTime;
    }

    private void Awake()
    {
        Instance = this;

        if (_audioSource == null || _audioSource.clip == null)
        {
            Debug.LogError("Conductor: AudioSource or AudioClip is missing!");
            return;
        }

        ComputeSongInfo();

        if (_activateOnAwake)
        {
            StartClock();
        }
    }

    private void Update()
    {
        if (!_isRunning) return;

        float songPosition = (float)(AudioSettings.dspTime - _songStartTime);
        int currentBeat = Mathf.FloorToInt(songPosition / _secondsPerBeat);

        if (currentBeat > _lastBeat)
        {
            _lastBeatTime = AudioSettings.dspTime;
            _lastBeat = currentBeat;
            OnBeat?.Invoke();
        }
    }
}
