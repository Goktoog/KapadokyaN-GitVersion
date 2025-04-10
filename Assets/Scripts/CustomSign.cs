using TMPro;
using UnityEngine;

public class CustomSign : MonoBehaviour
{
    public TextMeshPro textMesh;
    public string customText = "Your Text Here";

    void Start()
    {
        if (textMesh != null)
        {
            textMesh.text = customText;
        }
    }
}
