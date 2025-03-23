using UnityEngine;

[CreateAssetMenu(fileName = "BreadWave", menuName = "BreadRhythm/BreadWave")]
public class BreadWave : ScriptableObject
{
    [field: SerializeField] public BreadData[] Wave { get; private set; }
    [field: SerializeField] public bool TutorialMode { get; private set; }
    [field: SerializeField] public string TutorialText { get; private set; }
    [field: SerializeField] public string TutorialTextSuccess { get; private set; }
    [field: SerializeField] public int MinimumPitchsToProceed { get; private set; }
    [field: SerializeField] public int MinimumFlipsToProceed { get; private set; }
}
