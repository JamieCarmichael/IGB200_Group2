using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Fields

    [SerializeField] private float playerSpeed = 5.0f;
    
    private CharacterController characterController;

    #endregion

    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();


    }

    private void Update()
    {
        Move(InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>());
    }
    #endregion

    private void Move(Vector2 moveInput)
    {
        Vector3 moveVector = new Vector3(moveInput.x, 0.0f, moveInput.y);
        moveVector = moveVector.normalized * Time.deltaTime * playerSpeed;

        characterController.Move(moveVector);
    }
}
