using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmAction : MonoBehaviour
{
    [SerializeField] private InputActionReference _flipKickAction;

    private readonly List<BreadBehaviour> _breads = new();

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
    }

    private void OnDisable()
    {
        _flipKickAction.action.Disable();
        _flipKickAction.action.performed -= FlipKick;
    }
    #endregion

    private void FlipKick(InputAction.CallbackContext context)
    {
        Debug.Log("Flip Kick!");
    }
}
