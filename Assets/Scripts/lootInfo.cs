using UnityEngine;

public class LootInfo : MonoBehaviour
{
    public Sprite itemSprite;
    public string itemName;
    public float itemPrice;

    [Header("Prefab Referans� (Kendi Prefab'�n� buraya koy)")]
    public GameObject itemPrefab;

    [HideInInspector] public Vector3 originalPosition;

    private void Awake()
    {
        // Sahnedeki obje �zerinden �al���yorsa, prefab referans�n� korumak ad�na bu null kontrol� iyidir.
        if (itemPrefab == null)
        {
            Debug.LogWarning($"{gameObject.name} prefab referans� eksik! itemPrefab alan�n� prefab i�inde doldur.");
        }

        // Orijinal pozisyonu sakla (gerekiyorsa geri d�nd�rmek i�in kullan�l�r)
        originalPosition = transform.position;
    }
}
