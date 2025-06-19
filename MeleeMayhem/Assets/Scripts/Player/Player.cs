using UnityEngine;

[RequireComponent (typeof(PlayerInputManager), typeof(Animator))]
public class Player : MonoBehaviour
{
    #region Player States

    public PlayerStateMachine StateMachine;
    public PlayerLocomotionState LocomotionState;

    #endregion

    #region Public Variables

    public bool debugAnimBoolName;

    #endregion

    #region Inspector References

    [SerializeField] PlayerData playerData;

    #endregion

    #region Get Variables


    #endregion

    #region Component Variables

    public Animator animator { get; private set; }
    public PlayerInputManager InputManager { get; private set; }

    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        LocomotionState = new PlayerLocomotionState(this, StateMachine, playerData, "move");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        InputManager = GetComponent<PlayerInputManager>();

        StateMachine.InitializeState(LocomotionState);
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

}
