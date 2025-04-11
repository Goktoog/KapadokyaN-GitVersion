using UnityEngine;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    public GameObject interactText; // UI Text
    public Transform teleportLocation; // Işınlanacağımız yer
    private bool isPlayerNear = false;

    private void Start()
    {
        interactText.SetActive(false); // Başlangıçta kapalı IsTrigger ile açılacak
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
 {
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null && teleportLocation != null)
    {
        player.transform.position = teleportLocation.position;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
 }

 private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            interactText.SetActive(true); 
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            interactText.SetActive(false);
            isPlayerNear = false;
        }
    }
    
}

