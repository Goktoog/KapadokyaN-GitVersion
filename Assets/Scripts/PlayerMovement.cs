using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public Animator animator;

    [Header("Surface Footstep Sounds")]
    public AudioClip[] pavementClips;
    public AudioClip[] undergrowthClips;
    public AudioClip[] parquetClips;
    public AudioClip[] linoleumClips;
    public AudioClip[] ironClips;
    public AudioClip[] defaultFootstepSounds;
    public AudioSource audioSource;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpForce = 6f; // Jump force
    public float gravityMultiplier = 1.5f;
    public Transform groundCheck;

    [Header("Step Climb Settings")]
    public float stepHeight = 0.3f;
    public float stepSmooth = 0.1f;

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
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        HandleMovement();
        StepClimb(); // Merdiven tirmanmasi icin cagiriyorum.

        if (!isGrounded)
        {
            // Ekstra yercekimi uygulamak icin karakter ucmasin.
            Vector3 extraGravity = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
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

        Vector3 velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);
        rb.velocity = velocity;
    }

    bool CheckIfGrounded()
    {
        float rayDistance = 0.15f;
        Vector3 origin = groundCheck != null ? groundCheck.position : transform.position + Vector3.up * 0.1f;
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Footstep()
    {
        if (!isGrounded) return;
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            string surfaceTag = hit.collider.tag;
            AudioClip[] selectedClips = null;
            switch (surfaceTag)
            {
                case "Pavement":
                    selectedClips = pavementClips;
                    break;
                case "Undergrowth":
                    selectedClips = undergrowthClips;
                    break;
                case "Parquet":
                    selectedClips = parquetClips;
                    break;
                case "Linoleum":
                    selectedClips = linoleumClips;
                    break;
                case "Iron":
                    selectedClips = ironClips;
                    break;
                default:
                    selectedClips = defaultFootstepSounds; // if mesh is not working then use default sound
                    break;
            }
            if (selectedClips != null && selectedClips.Length > 0)
            {
                int index = Random.Range(0, selectedClips.Length);
                audioSource.PlayOneShot(selectedClips[index]);
            }
        }
    }

    void StepClimb() // cok kompleks oldu
    {
        float rayDistance = 0.5f;
        float lowerRayHeight = 0.1f;
        float upperRayHeight = stepHeight;

        Vector3 originLower = transform.position + Vector3.up * lowerRayHeight;
        Vector3 originUpper = transform.position + Vector3.up * upperRayHeight;

        RaycastHit hitLower;
        RaycastHit hitUpper;

        // Sol
        Vector3 dirLeft = transform.forward - transform.right * 0.25f;
        if (Physics.Raycast(originLower + dirLeft * 0.2f, transform.forward, out hitLower, rayDistance))
        {
            if (!Physics.Raycast(originUpper + dirLeft * 0.2f, transform.forward, out hitUpper, rayDistance))
            {
                rb.position += new Vector3(0f, stepSmooth, 0f);
            }
        }

        // Orta
        if (Physics.Raycast(originLower, transform.forward, out hitLower, rayDistance))
        {
            if (!Physics.Raycast(originUpper, transform.forward, out hitUpper, rayDistance))
            {
                rb.position += new Vector3(0f, stepSmooth, 0f);
            }
        }

        // Sag
        Vector3 dirRight = transform.forward + transform.right * 0.25f;
        if (Physics.Raycast(originLower + dirRight * 0.2f, transform.forward, out hitLower, rayDistance))
        {
            if (!Physics.Raycast(originUpper + dirRight * 0.2f, transform.forward, out hitUpper, rayDistance))
            {
                rb.position += new Vector3(0f, stepSmooth, 0f);
            }
        }
    }

    void UpdateAnimator()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool isMoving = moveX != 0 || moveZ != 0;
        animator.SetBool("isRunning", isMoving);

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        animator.SetFloat("Speed", isMoving ? speed : 0f);
    }

    void HandleJump()//Buraya bir de debug koycam çalýþýyor mu diye kontrol etmek için
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump"); // sadece animasyonu baþlat
        }

    }


    public void PerformJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)// groundCheck null deðilse
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.15f);
        }
    }
}
