using UnityEngine;

public class TextBillboardAndToggle : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject interactableText;
    [SerializeField] private float displayDistance = 5f;
    [SerializeField] private GameObject npc;

    private bool isInRange = false;

    private void Start()
    {
        if (interactableText == null || player == null || npc == null)
        {
            Debug.LogError("Bazı gerekli referanslar atanmadı.");
            enabled = false;
        }

        interactableText.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        isInRange = distance <= displayDistance;

        if (isInRange)
        {
            interactableText.SetActive(true);
            turnNpc();
            talkInteraction(); // E basma kontrolü
        }
        else
        {
            interactableText.SetActive(false);

            // Oyuncu uzaklaşınca konuşmayı kapat
            npcDialogue.instance?.HideOnlyText(); // sadece text kaybolsun, ses devam etsin
        }
    }

    private void LateUpdate()
    {
        if (interactableText.activeSelf)
        {
            Vector3 direction = Camera.main.transform.position - interactableText.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                interactableText.transform.rotation = Quaternion.Slerp(
                    interactableText.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }
        }
    }

    private void turnNpc()
    {
        if (npc != null && player != null)
        {
            Vector3 direction = player.position - npc.transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void talkInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            npcDialogue.instance?.speakRandomLine();
        }
    }
}
