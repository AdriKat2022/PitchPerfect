using System.Collections;
using UnityEngine;

public class OvenBehaviour : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite _openOven;
    [SerializeField] private Sprite _openClosed;
    [SerializeField] private Sprite _openClosedPitched;

    private Coroutine _cookCoroutine;
    private AudioSource _audioSource;

    private IEnumerator Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _spriteRenderer.sprite = _openClosed;
        yield return new WaitForSeconds(2f / 4f * (float)RhythmCore.Instance.BeatInterval);
        _spriteRenderer.sprite = _openOven;
    }

    public void Cook()
    {
        if (_cookCoroutine != null)
        {
            StopCoroutine(_cookCoroutine);
        }

        _cookCoroutine = StartCoroutine(CookCoroutine());
    }

    private IEnumerator CookCoroutine()
    {
        _spriteRenderer.sprite = _openClosedPitched;
        yield return new WaitForSeconds(2f / 4f * (float)RhythmCore.Instance.BeatInterval);
        _spriteRenderer.sprite = _openOven;

        _scoreManager.AddPitch();
        _audioSource.Play();
    }
}
