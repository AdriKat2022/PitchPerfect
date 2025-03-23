using UnityEngine;

public class BreadSpawner : MonoBehaviour
{
    [Header("Wave")]
    [SerializeField] private BreadData[] _breadWave;

    [Header("Settings")]
    [SerializeField] private RhythmAction _rhythmActionLine;
    [SerializeField] private BreadBehaviour _breadPrefab;

    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnXRadius = 1f;
    [SerializeField] private bool _startOnAwake = false;

    private bool _isPlaying = false;
    private int _currentBreadIndex = 0;
    private int _delayCount = 0;

    private void Awake()
    {
        if (_startOnAwake)
        {
            _isPlaying = true;
        }
    }

    private void OnEnable()
    {
        RhythmCore.OnBeat += NextSpawn;
    }
    private void OnDisable()
    {
        RhythmCore.OnBeat -= NextSpawn;
    }

    private void NextSpawn()
    {
        if (!_isPlaying) return;

        if (_delayCount > 0)
        {
            _delayCount--;
            return;
        }

        if (_currentBreadIndex < _breadWave.Length)
        {
            SpawnBread(_breadWave[_currentBreadIndex]);
            _delayCount = _breadWave[_currentBreadIndex].NextBeatDelay;
            _currentBreadIndex++;
        }
        else
        {
            _isPlaying = false;
            // TODO: End animation
        }
    }

    public void SpawnBread(BreadData breadData)
    {
        Vector3 spawnPosition = transform.position + Vector3.up * UnityEngine.Random.Range(-_spawnXRadius, _spawnXRadius);
        BreadBehaviour bread = Instantiate(_breadPrefab, spawnPosition, Quaternion.identity);
        bread.Initialize(_rhythmActionLine, spawnPosition, _rhythmActionLine.Oven, _rhythmActionLine.transform.position.y, breadData);
        _rhythmActionLine.RegisterBread(bread);
    }
}