using Fusion;
using UnityEngine;

public class CharacterInputController : NetworkBehaviour
{
    public Vector3 MovementInput { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsFirePressed { get; private set; }
    public bool IsSprintPressed { get; private set; }
    public bool IsCrouchPressed { get; private set; }
    public bool IsInteractPressed { get; private set; }

    public void ApplyInputs(NetworkInputData inputs)
    {
        MovementInput = inputs.MovementInput;
        IsJumpPressed = inputs.IsJumpPressed;
        IsFirePressed = inputs.IsFirePressed;
        IsSprintPressed = inputs.IsSprintPressed;
        IsCrouchPressed = inputs.IsCrouchPressed;
        IsInteractPressed = inputs.IsInteractPressed;
    }
}