using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public Animator animator;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpForce = 6f;
    public float gravityMultiplier = 2f;

    [Header("Look Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCol;
    private float rotationX = 0;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCol = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleJump();
        UpdateAnimator(); // her frame animasyonu kontrol eder
    }

    void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
    rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

    float yRotation = Input.GetAxis("Mouse X") * lookSpeed;
    Quaternion deltaRotation = Quaternion.Euler(0f, yRotation, 0f);
    rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        //float speed = move.magnitude;
        animator.SetFloat("Speed", speed);

        Vector3 velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);
        rb.velocity = velocity;
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // düşüş momentumunu sıfırla
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool CheckIfGrounded()
    {
    float rayLength = capsuleCol.height / 10f + 0.1f;
    Vector3 origin = transform.position + Vector3.up * 0.1f;

    // Ortadan ışın
    bool centerHit = Physics.Raycast(origin, Vector3.down, rayLength);

    // Sağ ve sol kenarlardan da kontrol
    bool sideHit1 = Physics.Raycast(origin + transform.right * 0.3f, Vector3.down, rayLength);
    bool sideHit2 = Physics.Raycast(origin - transform.right * 0.3f, Vector3.down, rayLength);

    // Debug çizgileri (isteğe bağlı)
    Debug.DrawRay(origin, Vector3.down * rayLength, centerHit ? Color.green : Color.red);
    Debug.DrawRay(origin + transform.right * 0.3f, Vector3.down * rayLength, sideHit1 ? Color.green : Color.red);
    Debug.DrawRay(origin - transform.right * 0.3f, Vector3.down * rayLength, sideHit2 ? Color.green : Color.red);

    return centerHit || sideHit1 || sideHit2;
    }

    void UpdateAnimator()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isMoving = horizontal != 0 || vertical != 0;
        animator.SetBool("isRunning", isMoving);
    }

}
