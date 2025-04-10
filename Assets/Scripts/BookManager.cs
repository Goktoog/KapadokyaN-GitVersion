using UnityEngine;

public class BookController : MonoBehaviour
{
    public GameObject bookPanel; // Kitap UI içindeki panel
    public GameObject bookObject; // Sahnedeki fiziksel kitap (Mbook)
    public Transform player; // Oyuncu referansı

    private bool isOpen = false;
    private bool hasBook = false;

    void Start()
    {
        bookPanel.SetActive(false); // Başlangıçta kapalı
    }

    void Update()
    {
        if (hasBook && Input.GetKeyDown(KeyCode.F) && !isOpen)
        {
            OpenBook();
        }
        else if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBook();
        }
        else if (hasBook && Input.GetKeyDown(KeyCode.G))
        {
            DropBook(); // G tuşu ile kitabı bırak
        }
    }

    public void EnableBook()
    {
        hasBook = true; // Kitap açılabilir hale gelsin
        Debug.Log("Kitap açılabilir hale geldi!");
    }

    public void DisableBook()
    {
        hasBook = false; // Kitap tekrar kapatılamaz hale gelsin
        CloseBook();
        Debug.Log("Kitap kapatıldı ve tekrar açılması engellendi!");
    }

    void OpenBook()
    {
        isOpen = true;
        bookPanel.SetActive(true);
        Debug.Log("Kitap açıldı!");
    }

    void CloseBook()
    {
        isOpen = false;
        bookPanel.SetActive(false);
        Debug.Log("Kitap kapandı!");
    }

    public bool IsAnimationFinished()
{
    // Burada animasyon tamamlandı mı kontrol edilecek
    // Eğer animasyon tamamlandıysa true döndürecek
    return true; // Şu anlık her zaman true döndürür, animasyon eklediğinde değiştirebilirsin
}

    void DropBook()
    {
        hasBook = false;
        bookPanel.SetActive(false); // Kitap UI'yi kapat
        bookObject.transform.position = player.position + player.forward * 1.5f; // Kitabı oyuncunun önüne koy
        bookObject.SetActive(true); // Kitabı tekrar sahnede görünür yap

        Debug.Log("Kitap yere bırakıldı!");
    }
}
