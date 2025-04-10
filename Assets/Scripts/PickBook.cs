using UnityEngine;
using TMPro;

public class BookPickup : MonoBehaviour
{
    public TextMeshProUGUI interactText; // "E - Kitabı Al" yazısı
    public GameObject bookCanvas; // Kitap UI paneli
    public BookController bookController; // Kitap kontrol scripti
    
    private bool canPickup = false;
    private bool hasBook = false;

    private void Start()
    {
        interactText.gameObject.SetActive(false); // Başlangıçta kapalı
        bookCanvas.SetActive(false); // Kitap UI başlangıçta kapalı
    }

    private void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E) && !hasBook)
        {
            Debug.Log("E tuşuna basıldı, kitap alınıyor...");        
            PickupBook();
        }

        if (hasBook && Input.GetKeyDown(KeyCode.G))
        {
            DropBook();
        }
    }

    private void PickupBook()
    {
        hasBook = true;
        interactText.gameObject.SetActive(false); // "Kitabı Al" yazısını gizle
        gameObject.SetActive(false); // Kitabı sahneden kaldır

        if (bookCanvas != null)
        {
            bookCanvas.SetActive(true); // Kitap UI açılabilir hale gelsin

            if (bookController != null)
            {
                bookController.EnableBook(); // Kitap açılabilir hale gelsin
            }
            else
            {
                Debug.LogError("HATA: BookController atanmamış! Inspector'dan ekleyin.");
            }
        }
        else
        {
            Debug.LogError("HATA: bookCanvas atanmamış! Inspector'dan ekleyin.");
        }
    }

    private void DropBook()
{
    hasBook = false;
    
    // Varsayılan bırakma pozisyonu (oyuncunun önüne)
    Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;

    // Raycast ile zemini bul (yerden 2 metre yukarıdan aşağıya ışın atıyoruz)
    RaycastHit hit;
    if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out hit, 5f))
    {
        dropPosition = hit.point + Vector3.up * 0.2f; // Zeminden biraz yukarı koy
    }

    // Kitabı yeni pozisyona taşı
    transform.position = dropPosition;
    
    // Kitabı tekrar toplanabilir hale getirmek için Trigger Collider'ı aç
    Collider col = GetComponent<Collider>();
    if (col != null)
    {
        col.enabled = true; // Eğer kapandıysa tekrar aç
    }

    // Kitabı görünür yap
    gameObject.SetActive(true);

    // Fizik bileşeni varsa, hareketini sıfırla ve düşmesini sağla
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.velocity = Vector3.zero; // Önceki hızını sıfırla
        rb.angularVelocity = Vector3.zero; // Dönme hareketini sıfırla
    }

    if (bookController != null)
    {
        bookController.DisableBook();
    }

    Debug.Log("Kitap yere bırakıldı ve tekrar toplanabilir!");
}



    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Kitap toplanabilir hale geldi!");
        interactText.gameObject.SetActive(true); // "Kitabı Al" yazısını göster
        canPickup = true;
    }
}

private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        interactText.gameObject.SetActive(false); // Oyuncu uzaklaşınca yazıyı gizle
        canPickup = false;
    }
}

private void PickUpBook()
{
    hasBook = true;
    interactText.gameObject.SetActive(false); // UI yazıyı gizle
    gameObject.SetActive(false); // Kitabı sahneden kaldır

    if (bookCanvas != null)
    {
        bookCanvas.SetActive(true); // Kitap UI aç
        if (bookController != null)
        {
            bookController.EnableBook();
        }
    }

    // Kitabı alınca Trigger'ı kapat
    Collider col = GetComponent<Collider>();
    if (col != null)
    {
        col.enabled = false;
    }

    Debug.Log("Kitap alındı!");
}

}
