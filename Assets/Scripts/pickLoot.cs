using UnityEngine;

public class LootTextToggle : MonoBehaviour
{
    [SerializeField] private GameObject promptText;
    [SerializeField] private Transform player;
    [SerializeField] private float displayDistance = 5f;

    private LootInfo info;

    private void Start()
    {
        info = GetComponent<LootInfo>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (promptText != null)
            promptText.SetActive(false);

        if (info == null)
            Debug.LogWarning("LootInfo scripti eksik!");
    }

    private void Update()
    {
        if (player == null || promptText == null || info == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        promptText.SetActive(distance <= displayDistance);

        collectLoot();
    }

    private void LateUpdate()
    {
        if (promptText != null && promptText.activeSelf)
        {
            Vector3 direction = Camera.main.transform.position - promptText.transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                promptText.transform.rotation = Quaternion.Slerp(promptText.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    private void collectLoot()
    {
        if (promptText.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            InventorySystem.Instance.AddItem(info.itemSprite, info.itemName, info.itemPrice, gameObject);
            gameObject.SetActive(false); //  Destroy ETME
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, displayDistance);
    }
}
