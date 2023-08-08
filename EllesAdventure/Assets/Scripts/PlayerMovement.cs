using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Fields
    [Tooltip("How much the players speed decreases every second while no move key is pressed.")]
    [SerializeField] private float deceleration = 5.0f;
    [Tooltip("How much the players speed changes each second when walking.")]
    [SerializeField] private float walkAcceleration = 5.0f;
    [Tooltip("The maximum speed that")]
    [SerializeField] private float maxWalkSpeed = 5.0f;

    [SerializeField] private float runAcceleration = 10.0f;

    [SerializeField] private float maxRunSpeed = 10.0f;


    private CharacterController characterController;

    private Vector3 velocity = Vector3.zero;
    #endregion

    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }


    private void LateUpdate()
    {
        Move();
    }
    #endregion

    private void Move()
    {
        if (!InputManager.Instance.PlayerInput.InGame.Move.IsInProgress())
        {
            ApplyDeceleration();
        }
        else
        {
            Vector2 moveInput = InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>();
            Vector3 moveVector = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
            if (InputManager.Instance.PlayerInput.InGame.Sprint.IsInProgress())
            {
                if (velocity.magnitude > maxRunSpeed)
                {
                    ApplyDeceleration();
                }
                else if (velocity.magnitude < maxRunSpeed)
                {
                    moveVector = moveVector * Time.deltaTime * runAcceleration;
                    velocity += moveVector;
                    if (velocity.magnitude > maxRunSpeed)
                    {
                        velocity = velocity.normalized * maxRunSpeed;
                    }
                }

            }
            else
            {
                if (velocity.magnitude > maxWalkSpeed)
                {
                    ApplyDeceleration();
                }
                else if (velocity.magnitude < maxWalkSpeed)
                {
                    moveVector = moveVector * Time.deltaTime * walkAcceleration;

                    velocity += moveVector;
                    if (velocity.magnitude > maxWalkSpeed)
                    {
                        velocity = velocity.normalized * maxWalkSpeed;
                    }
                }

            }
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void ApplyDeceleration()
    {
        if (velocity.magnitude < 0.1f)
        {
            velocity = Vector3.zero;
        }
        else
        {
            float speed = velocity.magnitude - (deceleration * Time.deltaTime);
            velocity = velocity.normalized * speed;
        }

    }
}
