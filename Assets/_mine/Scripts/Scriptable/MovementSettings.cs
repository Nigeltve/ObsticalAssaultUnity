using UnityEngine;
using MyBox;


[CreateAssetMenu(fileName = "movesettings", menuName = "Unit/MoveSettings", order = 0)]
public class MovementSettings : ScriptableObject
{
    [Separator("Basic Movement")]
    public float MovementSpeed = 5f;
    public float TurnTime = 0.05f;

    [Separator("Jump movement")] 
    public float JumpHeight = 15f;
    public float GravityMultiplier = 3.0f;
    

    [Separator("Dash Movement")] 
    public float DashDuration = 0.2f;
    public float DashCoolDown = 3f;
    public float DashStrength = 3f;
}
