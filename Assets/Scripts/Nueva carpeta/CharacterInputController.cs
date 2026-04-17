using Fusion;
using UnityEngine;

public class CharacterInputController : NetworkBehaviour
{
    [Header("Buttons")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode _interactKey = KeyCode.E;
    [SerializeField] KeyCode _ragdollKey = KeyCode.R;

    bool _isCrouching = false;
    public bool IsCrouching => _isCrouching;

    bool _isJumping = false;
    public bool IsJumping => _isJumping;

    bool _isSprinting = false;
    public bool IsSprinting => _isSprinting;

    bool _isInteracting = false;
    public bool IsInteracting => _isInteracting;

    bool _isShooting = false;
    public bool IsShooted => _isShooting;

    bool _isRagdoll = false;

    public bool IsRagdoll => _isRagdoll;

    Vector3 _direction;
    public Vector3 Direction => _direction;

    public void ArtificialUpdate()
    {
        _direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(_crouchKey)) _isCrouching = !_isCrouching;
        if (Input.GetKeyDown(_sprintKey)) _isSprinting = !_isSprinting;

        if (Input.GetKeyDown(_jumpKey)) _isJumping = true;
        //else _isJumping = false;

        if (Input.GetKeyDown(_interactKey)) _isInteracting = true;
       // else _isInteracting = false;

        if(Input.GetKeyDown(KeyCode.R)) _isRagdoll = !_isRagdoll;

        if (Input.GetKey(_shootKey)) _isShooting = true;
        else _isShooting = false;
    }
}