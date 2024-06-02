using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlaneFadeIn : MonoBehaviour
{
    public float fadeInDuration = 2f; // Duration of the fade-in effect in seconds
    private Renderer planeRenderer;
    private float startTime;

    void Start()
    {
        planeRenderer = GetComponent<Renderer>();
        if (planeRenderer == null)
        {
            Debug.LogError("No Renderer component found on the game object. Fade-in effect will not work.");
            enabled = false; // Disable the script if no Renderer is found
            return;
        }

        startTime = Time.time;
        planeRenderer.material.color = new Color(1f, 1f, 1f, 0f); // Set initial color to transparent
    }

    void Update()
    {
        if (planeRenderer == null)
            return; // Exit the function if no Renderer is found

        float timeSinceStart = Time.time - startTime;
        float percentageComplete = timeSinceStart / fadeInDuration;

        if (percentageComplete < 1f)
        {
            Color newColor = new Color(1f, 1f, 1f, percentageComplete);
            planeRenderer.material.color = newColor;
        }
        else
        {
            planeRenderer.material.color = Color.white; // Set final color to opaque white
            enabled = false; // Disable this script after fade-in is complete
        }
    }
}