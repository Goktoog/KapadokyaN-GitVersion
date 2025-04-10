using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] grassSteps;
    public AudioClip[] woodSteps;
    public float stepInterval = 0.5f;

    private float stepTimer = 0f;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Sadece Player tagine sahip objenin ad�m sesi �almas�n� sa�la ve karakter y�r�yor mu kontrol et
        if (gameObject.CompareTag("Player") && characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer > stepInterval && !audioSource.isPlaying)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // Karakter durunca saya� s�f�rlan�r
            audioSource.Stop(); // Hareket durdu�unda sesi kes
        }
    }

    void PlayFootstepSound()
    {
        AudioClip[] currentSurface = DetermineSurface();
        if (currentSurface.Length > 0)
        {
            int randomIndex = Random.Range(0, currentSurface.Length);
            audioSource.PlayOneShot(currentSurface[randomIndex]);
        }
    }

    AudioClip[] DetermineSurface()
    {
        // Karakterin alt�ndaki y�zeyi belirleme
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            if (hit.collider.CompareTag("Grass"))
            {
                return grassSteps;
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                return woodSteps;
            }
        }
        return grassSteps;
    }
}
