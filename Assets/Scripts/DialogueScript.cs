using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public string explanationText = "This is the text to display letter by letter.";
    public float letterDelay = 0.1f; // Time delay between each letter
    private Coroutine revealTextCoroutine;

    void Start()
    {
        textMeshPro.text = "";
        StartRevealText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowText();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            HideText();
        }
    }

    void StartRevealText()
    {
        if (revealTextCoroutine != null)
        {
            StopCoroutine(revealTextCoroutine);
        }
        revealTextCoroutine = StartCoroutine(RevealText());
    }

    IEnumerator RevealText()
    {
        textMeshPro.text = "";
        foreach (char letter in explanationText.ToCharArray())
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    void ShowText()
    {
        textMeshPro.text = explanationText;
    }

    void HideText()
    {
        textMeshPro.text = "";
    }
}
