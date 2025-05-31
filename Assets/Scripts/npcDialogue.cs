using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string text;
    public AudioClip voiceClip;
}

public class npcDialogue : MonoBehaviour
{
    public static npcDialogue instance;// Bu kodu diğer scriptlerde kullanabilmek için static

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private DialogueLine[] dialogueLines;
    private Coroutine hideDialogueCoroutine;



    void Start()
    {
        dialogueCanvas.SetActive(false); // Baslangicta diyalogu gizle
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Eger instance null ise, bu sınıf örneğini oluşturur.
        }
    }


    //private string[] gibberishLines =
    //{
    //    "Glibbo Glabba: Glib glop glip gaba",
    //    "Glibbo Glabba: Değerli madenler güzeldir Glibbo Glabba onları sever",
    //    "Glibbo Glabba: Bububububup",
    //    "Glibbo Glabba: Değersiz madenlerini sakın ola glablama yani asansöre anladın değil mi ?",
    //    "Glibbo Glabba: Giberlink dilinde sözler...",
    //    "Glibbo Glabba: Asansörü glob glablamaktan asla çekinme!"
    //};



    public void speakRandomLine()
    {
        int index = Random.Range(0, dialogueLines.Length);
        DialogueLine selectedLine = dialogueLines[index];

        dialogueText.text = selectedLine.text;
        dialogueCanvas.SetActive(true);

        if (selectedLine.voiceClip != null && audioSource != null)
        {
            audioSource.clip = selectedLine.voiceClip;
            audioSource.Play();
        }

        // Eğer eski bir coroutine çalışıyorsa durdur yoksa kapanır
        if (hideDialogueCoroutine != null)
            StopCoroutine(hideDialogueCoroutine);

        // 5 saniye sonra canvas'ı kapat
        hideDialogueCoroutine = StartCoroutine(HideAfterDelay(5f));

    }

    public void HideOnlyText()
    {
        dialogueCanvas.SetActive(false); // sadece canvas kapatılıyor, ses durmuyor
    }



    public void HideDialogue()
    {
        dialogueCanvas.SetActive(false); // Alandan uzaklasinca diyalogu gizlemek icin
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogueCanvas.SetActive(false);
    }

    void Update()
    {

    }
}
