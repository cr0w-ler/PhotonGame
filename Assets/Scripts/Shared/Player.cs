using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Header("Speed")]
    [SerializeField] float _defaultSpeed = 3f;
    [SerializeField] float _sprintSpeed = 6f;
    [SerializeField] float _jumpForce = 5f;

    [Header("Crouch")]
    [SerializeField] float _crouchSpeed = 3f;
    [SerializeField] float _crouchColHeight = 1f;
    [SerializeField] float _crouchColCenter = 1f;

    [Header("Weapon")]
    [SerializeField] Weapon _weapon;

    [Header("Life")]
    [SerializeField] HealthSystem _health;

    [SerializeField] Ragdoll _ragdoll;
    [SerializeField] CharacterColliderResizer _colliderResizer;
    [SerializeField] CharacterRotator _characterRotator;
    [SerializeField] CharacterInputController _inputController;
    [SerializeField] GroundRaycast _groundRaycast;
    [SerializeField] InteractRaycast _interactRaycast;
    //[SerializeField] CharacterMeshSelector _meshSelector;
    [SerializeField] CharacterAnimationController _animationController;
    bool _isGround;
/*    bool _isInteract;
    bool _isJumping;
    bool _isShooting;
    bool _isCrouching;*/
    bool _isOnAir;
    Camera _camera;
    NetworkRigidbody3D _rb;
    Vector3 _direction;
    //int _randomMeshIndex = 0;

    public override void Spawned()
    {
        _rb = GetComponent<NetworkRigidbody3D>();
        
        _ragdoll.DisableRagdoll();

        _health.OnDead += Death;

        /*_randomMeshIndex = UnityEngine.Random.Range(0, _meshSelector.MecanimAnims.Length);

         _meshSelector.SelectMesh(_randomMeshIndex);
         _animationController.SetAnimator(_meshSelector.MecanimAnims[_randomMeshIndex]);*/

        if (HasInputAuthority)
        {
            _camera = Camera.main;
            _camera.GetComponent<FollowTarget>().SetTarget(this);
        }

        GameManager.Instance.AddToList(this);
    }

    void Update()
    {
        if (_health._isDead) return;
        if (!HasInputAuthority) return;

        _isGround = _groundRaycast.IsRaycasting(Vector3.down);
        Debug.Log(_isGround);
        /*if(_inputController.IsJumpPressed && _isGround)
        {
            _isJumping = true;
        }

        if (_inputController.IsCrouchPressed)
        {
            _isCrouching = true;
        }
        else
        {
            _isCrouching = false;
        }
        
        _isShooting = _inputController.IsFirePressed;
        _direction = _inputController.MovementInput;*/

        _isOnAir = !_isGround;

        _animationController.SetBool(AnimParams.Air, _isOnAir);
    }
        
    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputs)) return;

        _inputController.ApplyInputs(inputs);

        _direction = _inputController.MovementInput;


        Movement();

        if (_health._isDead)
        {
            RPC_ActivateRagdoll(true);
        }
        else
        {
            RPC_ActivateRagdoll(false);
        }

        if (inputs.networkButtons.IsSet(MyButtons.Jump) && _inputController.IsJumpPressed)
        {
            Jump();
        }

        if (inputs.networkButtons.IsSet(MyButtons.Shoot) && _weapon._readyToFire)
        {
            _weapon.Fire();
        }

        

        if (inputs.networkButtons.IsSet(MyButtons.Crouch) && _inputController.IsCrouchPressed && !_inputController.IsJumpPressed)
        {
            Crouch();
        }
        else
        {
            Uncrouch();
        }

        if(inputs.networkButtons.IsSet(MyButtons.Interact))
        {

        }

        _characterRotator.RotateDefault(_inputController.MovementInput);
    }

    void Movement()
    {
        Vector3 velocity = _direction.normalized * _defaultSpeed;

        _rb.Rigidbody.MovePosition(_rb.Rigidbody.position + velocity * Runner.DeltaTime);

        _animationController.SetFloat(AnimParams.Speed, velocity.magnitude);
    }

    void Jump()
    {
        //if(!_isJumping) return;

        _animationController.SetTrigger(AnimParams.Jump);
        _rb.Rigidbody.linearVelocity = Vector3.up * _jumpForce;
        //_isJumping = false;
    }

    void Crouch()
    {
        _colliderResizer.SetSize(1, new Vector3(0, -0.5f, 0));
        _animationController.SetBool(AnimParams.Crouch, true);
    }

    void Uncrouch()
    {
        _colliderResizer.SetSize(2, new Vector3(0, 0, 0));
        _animationController.SetBool(AnimParams.Crouch, false);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ActivateRagdoll(bool activate)
    {
        if (activate)
        {
            _ragdoll.ActivateRagdoll();
        }
        else
        {
            _ragdoll.DisableRagdoll();
        }
    }

    void Death()
    {
        GameManager.Instance.RPC_Defeat(Runner.LocalPlayer);
    }
}