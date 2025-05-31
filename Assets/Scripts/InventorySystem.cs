using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [Header("UI Ayarlarý")]
    public GameObject slotPrefab;
    public Transform slotParent;
    public GameObject inventoryPanel;

    private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        HandleInventoryToggle();
    }

    public struct InventoryItem
    {
        public Sprite sprite;
        public string name;
        public float price;
        public GameObject prefab;
        public GameObject instanceRef; // Sahne üzerindeki obje


        public InventoryItem(Sprite sprite, string name, float price, GameObject prefab, GameObject instanceRef)
        {
            this.sprite = sprite;
            this.name = name;
            this.price = price;
            this.prefab = prefab;
            this.instanceRef = instanceRef;
        }
    }

    public void AddItem(Sprite sprite, string name, float price, GameObject prefab)
    {
        // Eðer ayný instanceRef zaten varsa, ekleme
        foreach (var item in inventoryItems)
        {
            if (item.instanceRef == prefab)
            {
                Debug.Log("Bu item zaten envanterde var.");
                return;
            }
        }

        InventoryItem newItem = new InventoryItem(sprite, name, price, prefab, prefab);
        inventoryItems.Add(newItem);

        GameObject newSlot = Instantiate(slotPrefab, slotParent);
        Image icon = newSlot.transform.Find("ItemIcon").GetComponent<Image>();
        icon.sprite = sprite;
        icon.enabled = true;

        Button btn = newSlot.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => DropItem(newItem, newSlot));
        }
    }

    //  Drop edilen item'i sahneye býrak + slot'u sil
    private void DropItem(InventoryItem item, GameObject slotObject)
    {
        GameObject dropped = item.prefab;

        if (dropped.activeSelf == false)
        {
            dropped.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            dropped.SetActive(true);

            inventoryItems.Remove(item);
            Destroy(slotObject);
        }
        else
        {
            Debug.Log("Bu item zaten sahnede.");
        }
    }

    private void HandleInventoryToggle()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);

            if (!isActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
