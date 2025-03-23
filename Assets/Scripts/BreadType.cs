using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBreadType", menuName = "BreadRhythm/BreadType")]
public class BreadType : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Flips { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Height { get; private set; }
}

[Serializable]
public struct BreadData
{
    public BreadType Bread;
    [Range(0f, 1f)]
    public float BeatOffset;
    public int NextBeatDelay;
}