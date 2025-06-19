using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    #region Input Flags

    public Vector2 MovementInput { get; private set; }
    public float moveAmount;

    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        HandleRawMovementInput();
    }

    #endregion

    #region Input Actions

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    #endregion

    #region Handle Input Functions

    private void HandleRawMovementInput()
    {
        moveAmount = Mathf.Clamp01(Mathf.Abs(MovementInput.x) + Mathf.Abs(MovementInput.y));

        if (moveAmount <= 0.5f && moveAmount > 0f)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5f && moveAmount <= 1f)
        {
            moveAmount = 1f;
        }
    }

    #endregion

    #region Use Input Function

    //Use Input flags after using it
    // public void UseJumpInput() => JumpInput = false;

    #endregion

}
