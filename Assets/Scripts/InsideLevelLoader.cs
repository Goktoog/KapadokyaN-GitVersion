using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideLevelLoader : MonoBehaviour
{
    [SerializeField] private string nextScene;  // Inspector'da görünmesi için

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            SceneManager.LoadScene(nextScene);  
        }
    }
}
