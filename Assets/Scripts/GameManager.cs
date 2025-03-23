using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _startAction;
    [SerializeField] private RhythmAction _rhythmAction;

    [SerializeField] private AudioSource _audioSource;

    private IEnumerator Start()
    {
        // Schedule music and start the rhythm
        yield return new WaitUntil(() => _startAction.action.triggered);

        Debug.Log("Starting game");

        // Shedule music
        _audioSource.PlayScheduled(AudioSettings.dspTime + 1.0);

        // Wait for the music to start and then start the rhythm
        yield return new WaitForSeconds(1.0f);

        RhythmCore.StartClock();
    }
}
