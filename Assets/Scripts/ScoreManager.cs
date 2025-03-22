using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _pitchesText;

    private int _score = 0;
    private int _pitches = 0;

    public void AddScore(int value)
    {
        _score += value;
        _scoreText.text = _score.ToString();
    }

    public void AddPitch()
    {
        _pitches++;
        _pitchesText.text = _pitches.ToString();
    }
}
