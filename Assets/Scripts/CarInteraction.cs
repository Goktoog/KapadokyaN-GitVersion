using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    public GameObject interactText; // UI'daki "E'ye bas" yazısı
    public GameObject parchmentCanvas; // Görev/informasyon UI Paneli

    private bool isPlayerNear = false;
    private bool isCanvasOpen = false;

    private void Start()
    {
        interactText.SetActive(false);
        parchmentCanvas.SetActive(false); // Başta kapalı
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleCanvas();
        }
    }

    void ToggleCanvas()
    {
        isCanvasOpen = !isCanvasOpen;
        parchmentCanvas.SetActive(isCanvasOpen);

        // Eğer cursor görünür olmalıysa:
        Cursor.visible = isCanvasOpen;
        Cursor.lockState = isCanvasOpen ? CursorLockMode.None : CursorLockMode.Locked;
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

            // Oyuncu uzaklaşırsa Canvas kapanır
            parchmentCanvas.SetActive(false);
            isCanvasOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
