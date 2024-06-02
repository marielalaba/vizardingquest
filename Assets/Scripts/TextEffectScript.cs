using System.Collections;
using UnityEngine;
using TMPro;

public class TextEffectScript : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public string[] instructionLines = {
        "Movement: Use the arrow keys or WASD to navigate your character.",
        "Grab/Drop Wand: Press the E key to pick up or drop the wand.",
        "Cast Spells: Hold the mouse button and move the mouse to cast spells.",
        "Summon Wand: Press C to use \"Accio\" and bring the wand to your current position.",
        "Close Guide: Press X to close this text",
        "Show This Again: Press T to view this tutorial again"
    };
    public float fadeInDuration = 2f; // Duration of the initial fade-in effect
    public float pulseDuration = 5f; // Duration of each pulse cycle
    public float pulseAmplitude = 0.1f; // Amplitude of the pulse effect
    public float lineSpacing = 1f; // Spacing between lines

    public GameObject instructions;

    private Coroutine fadeInCoroutine;
    private Coroutine pulsateCoroutine;

    void Start()
    {
        textMeshPro.text = string.Join("\n", instructionLines);
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0f); // Set initial alpha to 0
        textMeshPro.lineSpacing = lineSpacing; // Set the line spacing
        StartCoroutine(DelayedFadeInEffect(3f));
    }

    IEnumerator DelayedFadeInEffect(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        StartFadeInEffect();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1f); // Set alpha to 1 (fully visible)
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StopAllCoroutines();
            textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0f); // Set alpha to 0 (fully invisible)
            instructions.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartFadeInEffect(); // Start the fade-in effect
            instructions.SetActive(true);
        }
    }

    void StartFadeInEffect()
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }
        fadeInCoroutine = StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        float elapsedTime = 0f;
        float startAlpha = 0f; // Initial alpha value
        float targetAlpha = 1f; // Target alpha value

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeInDuration);
            textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, targetAlpha); // Ensure alpha is set to the target value at the end
        StartPulsateEffect();
    }

    void StartPulsateEffect()
    {
        if (pulsateCoroutine != null)
        {
            StopCoroutine(pulsateCoroutine);
        }
        pulsateCoroutine = StartCoroutine(PulsateText());
    }

    IEnumerator PulsateText()
    {
        float elapsedTime = 0f;
        while (true)
        {
            float alpha = Mathf.Sin(elapsedTime * 2f * Mathf.PI / pulseDuration) * pulseAmplitude + 1f - pulseAmplitude / 2f;
            textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}