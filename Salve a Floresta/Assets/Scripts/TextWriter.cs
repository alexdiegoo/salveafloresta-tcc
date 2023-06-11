using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    public float letterDelay = 0.1f; // Tempo de atraso entre cada letra
    public string fullText; // Texto completo a ser exibido

    private string currentText = "";
    public Text text;
    public Button button;

    private void Start()
    {
        button.interactable = false;
        StartCoroutine(WriteText());
    }

    private IEnumerator WriteText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            text.text = currentText;
            yield return new WaitForSeconds(letterDelay);
        }
        button.interactable = true;
    }
}
