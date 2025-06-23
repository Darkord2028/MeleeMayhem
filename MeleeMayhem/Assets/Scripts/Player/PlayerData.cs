using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Game Data")]
    public float interval = 3;

    [Header("Base Data")]
    public float gravity;
    public float downwardForce;
    public float groundCheckRadius;
    public float viewRadius;
    public LayerMask whatIsGround;
    public LayerMask enemyMask;

    [Header("Locomotion State")]
    public float moveSpeed;
    public float movementRotation;

    [Header("Snap to Enemy State")]
    public float snapDistance = 1.5f;

}
