using UnityEngine;

public class LootInfo : MonoBehaviour
{
    public Sprite itemSprite;
    public string itemName;
    public float itemPrice;

    [Header("Prefab Referansý (Kendi Prefab'ýný buraya koy)")]
    public GameObject itemPrefab;

    [HideInInspector] public Vector3 originalPosition;

    private void Awake()
    {
        // Sahnedeki obje üzerinden çalýþýyorsa, prefab referansýný korumak adýna bu null kontrolü iyidir.
        if (itemPrefab == null)
        {
            Debug.LogWarning($"{gameObject.name} prefab referansý eksik! itemPrefab alanýný prefab içinde doldur.");
        }

        // Orijinal pozisyonu sakla (gerekiyorsa geri döndürmek için kullanýlýr)
        originalPosition = transform.position;
    }
}
