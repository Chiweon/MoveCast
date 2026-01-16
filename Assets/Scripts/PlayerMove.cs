using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 direction;
    private Camera mainCamera;

    private float verticalVelocity;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundedGravity = -0.5f;
    [SerializeField] private float jumpValue = 2.0f;
    [SerializeField] private float rotationSpeed = 720.0f;

    // 점프 기능 보류로 인해 coyoteTime 관련 코드 주석처리
    //private float coyoteTime = 0.2f;
    //private float coyoteTimeCounter;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // 점프 기능 일단 보류
        //inputActions.Player.Jump.performed += ctx => OnJump();


    }
    void OnEnable()
    {
        inputActions.Player.Enable();
    }
    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        Gravity();
        Move();
        RotateToCursor();
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        direction = moveDirection * speed;
        direction.y = verticalVelocity * Time.deltaTime;
        //Debug.Log(direction.y);
        controller.Move(direction * Time.deltaTime);
    }

    private void RotateToCursor()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            Vector3 lookDirection = (targetPoint - transform.position).normalized;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void Gravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            Debug.Log("isGrounded");
            verticalVelocity += groundedGravity;
            //coyoteTimeCounter = coyoteTime;
        }
        else
        {
            Debug.Log("is not Grounded");
            //float multiplier = verticalVelocity > 0 ? 2.0f : 1.0f;
            verticalVelocity += gravity;
            //coyoteTimeCounter -= Time.deltaTime;
        }

    }



    //private void OnJump()
    //{
    //    if (coyoteTimeCounter > 0)
    //    {

    //        verticalVelocity = Mathf.Sqrt(jumpValue * -2.0f * gravity);
    //        if (verticalVelocity < 0)
    //        {
    //            Debug.Log("verticalVelocity 음수");
    //        }
    //        coyoteTimeCounter = 0;
    //        Debug.Log("Jump!");
    //    }
    //}
}
