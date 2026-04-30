using UnityEngine;

public class PlayerControllerFPS : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.45f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Mouse Look")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalLimit = 80f;

    private Rigidbody rb;

    private float verticalRotation;
    private bool isGrounded;
    private bool jumpRequested;
    private bool canControl = true;

    private float sensitivityMultiplier = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update()
    {
        if (!canControl) return;

        HandleMouseLook();
        HandleJumpInput();
    }

    private void FixedUpdate()
    {
        if (!canControl)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        CheckGround();
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection.Normalize();

        Vector3 targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
    }

    private void HandleJump()
    {
        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpRequested = false;
    }

    private void CheckGround()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }
        else
        {
            Vector3 checkPosition = transform.position + Vector3.down * 1.05f;

            isGrounded = Physics.CheckSphere(
                checkPosition,
                groundCheckRadius,
                groundLayer
            );
        }
    }

    private void HandleMouseLook()
{
    float mouseX = Input.GetAxisRaw("Mouse X");
    float mouseY = Input.GetAxisRaw("Mouse Y");

    // Strong dead zone to avoid automatic camera drift
    if (Mathf.Abs(mouseX) < 0.2f) mouseX = 0f;
    if (Mathf.Abs(mouseY) < 0.2f) mouseY = 0f;

    mouseX *= mouseSensitivity * sensitivityMultiplier;
    mouseY *= mouseSensitivity * sensitivityMultiplier;

    transform.Rotate(Vector3.up * mouseX);

    verticalRotation -= mouseY;
    verticalRotation = Mathf.Clamp(verticalRotation, -verticalLimit, verticalLimit);

    if (cameraPivot != null)
    {
        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}

    public void SetSensitivityMultiplier(float multiplier)
    {
        sensitivityMultiplier = multiplier;
    }

    public void SetControlEnabled(bool enabled)
    {
        canControl = enabled;

        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !enabled;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        else
        {
            Vector3 checkPosition = transform.position + Vector3.down * 1.05f;
            Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
        }
    }
}