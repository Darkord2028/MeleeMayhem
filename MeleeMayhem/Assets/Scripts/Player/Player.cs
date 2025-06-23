using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerInputManager), typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Player States

    public PlayerStateMachine StateMachine;
    public PlayerIdleState IdleState;
    public PlayerMoveState MoveState;
    public PlayerAttackState AttackState;

    #endregion

    #region Public Variables

    public bool debugAnimBoolName;

    #endregion

    #region Private Variables

    private Vector3 playerVelocity;

    private Collider[] results = new Collider[10];

    #endregion

    #region Inspector References

    [SerializeField] PlayerData playerData;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform playerCamera;

    #endregion

    #region Public Get Variables


    #endregion

    #region Component Variables

    public Animator animator { get; private set; }
    public PlayerInputManager InputManager { get; private set; }
    public CharacterController characterController { get; private set; }

    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        AttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        InputManager = GetComponent<PlayerInputManager>();
        characterController = GetComponent<CharacterController>();

        StateMachine.InitializeState(IdleState);
        StateMachine.CurrentState.Enter();
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Public Functions

    public void HandleGravity()
    {
        playerVelocity.y = playerVelocity.y + playerData.gravity * Time.deltaTime;
        if (isGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = playerData.downwardForce;
        }
        characterController.Move(playerVelocity * Time.deltaTime);

    }

    public void SetMovement(float movementSpeed)
    {
        Vector3 moveDirection;
        moveDirection = playerCamera.forward * InputManager.MovementInput.y;
        moveDirection = moveDirection + playerCamera.right * InputManager.MovementInput.x;
        moveDirection.Normalize();
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        characterController.Move(movementVelocity * Time.deltaTime);
    }

    public void SetRotation(float rotationSpeed)
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = playerCamera.forward * InputManager.MovementInput.y;
        targetDirection = targetDirection + playerCamera.right * InputManager.MovementInput.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    #endregion

    #region Do Check Functions

    public bool isGrounded()
    {
        Collider[] hitColliders = new Collider[10];
        int numColliders = Physics.OverlapSphereNonAlloc(groundCheck.position, playerData.groundCheckRadius, hitColliders, playerData.whatIsGround);

        foreach (Collider collider in hitColliders)
        {
            if (collider != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (characterController.isGrounded)
        {
            return true;
        }

        return false;
    }

    public Transform FindClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, playerData.viewRadius, results, playerData.enemyMask);

        for (int i = 0; i < hitCount; i++)
        {
            Transform target = results[i].transform;
            float distToTarget = Vector3.Distance(transform.position, target.position);

            if (distToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distToTarget;
                closestEnemy = target;
            }
        }

        return closestEnemy;
    }

    #endregion

    #region Animation Events

    public void OnAnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    public void OnFinishAnimationTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, playerData.viewRadius);
    }

    #endregion

}
