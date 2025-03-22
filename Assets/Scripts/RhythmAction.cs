using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmAction : MonoBehaviour
{
    [Header("Tolerance")]
    [SerializeField] private float _toleranceExcellent = 0.2f;
    [SerializeField] private float _toleranceGreat = 0.4f;
    [SerializeField] private float _toleranceGood = 0.6f;
    [SerializeField] private float _toleranceOk = 0.8f;

    [Header("Settings")]
    [SerializeField] private OvenBehaviour _oven;
    [SerializeField] private InputActionReference _flipKickAction;

    private readonly List<BreadBehaviour> _breads = new();

    public OvenBehaviour Oven => _oven;

    public void RegisterBread(BreadBehaviour bread)
    {
        _breads.Add(bread);
    }

    public void UnregisterBread(BreadBehaviour bread)
    {
        _breads.Remove(bread);
    }

    #region Input Management
    private void OnEnable()
    {
        _flipKickAction.action.Enable();
        _flipKickAction.action.performed += FlipKick;
        _flipKickAction.action.canceled += FlipKick;
    }

    private void OnDisable()
    {
        _flipKickAction.action.Disable();
        _flipKickAction.action.performed -= FlipKick;
        _flipKickAction.action.canceled -= FlipKick;
    }
    #endregion

    private void FlipKick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Make a flip
            for (int i = 0; i < _breads.Count; i++)
            {
                RewardType rewardType = ComputeRewardType(_breads[i]);
                if (rewardType != RewardType.None)
                {
                    Debug.Log("Flipping bread: " + rewardType);
                    _breads[i].Flip(rewardType);
                }
            }
        }
        if (context.canceled)
        {
            // Make a kick
            for (int i = 0; i < _breads.Count; i++)
            {
                RewardType rewardType = ComputeRewardType(_breads[i]);
                if (rewardType != RewardType.None)
                {
                    _breads[i].Kick((float)context.duration, rewardType);
                    Debug.Log("KICK bread: " + rewardType);
                }
            }
        }
    }

    private RewardType ComputeRewardType(BreadBehaviour breadBehaviour)
    {
        float distance = Mathf.Abs(breadBehaviour.transform.position.y - transform.position.y);

        if (distance > 1f) return RewardType.None;

        float beatDistance = RhythmCore.GetBeatDistance();

        if (beatDistance < _toleranceExcellent)
        {
            return RewardType.Excellent;
        }
        else if (beatDistance < _toleranceGreat)
        {
            return RewardType.Great;
        }
        else if (beatDistance < _toleranceGood)
        {
            return RewardType.Good;
        }
        else if (beatDistance < _toleranceOk)
        {
            return RewardType.Ok;
        }
        else
        {
            return RewardType.None;
        }
    }
}

public enum RewardType
{
    None,
    Ok,
    Good,
    Great,
    Excellent
}
