using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageTurner : MonoBehaviour
{
    public BookController bookController; // Kitap kontrol scripti
    public Transform pageHolder; // Sayfaların tutulduğu ana GameObject
    public float rotationDuration = 1f; // Sayfanın dönüş süresi
    public List<Transform> pages = new List<Transform>(); // Sayfa listesini inspector'dan sürükleyerek ekleyebilirsin.

    private int currentPage = 0; // Hangi sayfanın açık olduğunu takip ediyoruz
    private bool isTurning = false; // Animasyon devam ediyor mu?

    void Start()
    {
        // Başlangıçta tüm sayfaları X = 180, Y = 0 pozisyonuna getir
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].rotation = Quaternion.Euler(180, 0, 0);
        }
    }

    void Update()
    {
        if (bookController.IsAnimationFinished() && !isTurning) // Kitap animasyonu bittiyse ve sayfa dönmüyorsa
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                TurnNextPage();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                TurnPreviousPage();
            }
        }
    }

    void TurnNextPage()
    {
        if (currentPage < pages.Count) // Eğer sıradaki sayfa varsa
        {
            StartCoroutine(RotatePage(pages[currentPage], 0, 180)); // Y ekseninde aç
            pages[currentPage].SetAsLastSibling(); // Sayfayı en üste taşı
            currentPage++;
        }
    }

    void TurnPreviousPage()
    {
        if (currentPage > 0) // Eğer geri dönebilecek sayfa varsa
        {
            currentPage--;
            pages[currentPage].SetAsLastSibling(); // Sayfayı en üste taşı
            StartCoroutine(RotatePage(pages[currentPage], 180, 0)); // Y ekseninde kapat
        }
    }

    void SetSortingOrder(Transform page, int order)
{
    Canvas canvas = page.GetComponent<Canvas>();
    if (canvas == null)
    {
        canvas = page.gameObject.AddComponent<Canvas>();
    }
    canvas.overrideSorting = true;
    canvas.sortingOrder = order;
}


    IEnumerator RotatePage(Transform page, float startY, float endY)
    {
        isTurning = true; // Animasyon sırasında yeni girişleri engelle
        Quaternion startRotation = Quaternion.Euler(180, startY, 0);
        Quaternion endRotation = Quaternion.Euler(180, endY, 0);
        float elapsedTime = 0;

        while (elapsedTime < rotationDuration)
        {
            page.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        page.rotation = endRotation; // Kesin dönüş sağla
        isTurning = false; // Animasyon tamamlandı

        foreach (Transform child in page)
        {
        child.gameObject.SetActive(true);
        }
    }

    public void ResetToFirstPage()
    {
    currentPage = 0; // İlk sayfaya dön
    for (int i = 0; i < pages.Count; i++)
    {
        pages[i].rotation = Quaternion.Euler(180, 0, 0); // Sayfaları başlangıç konumuna getir
    }
 }
}
