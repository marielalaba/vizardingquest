using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleAnimation : MonoBehaviour
{
    private Vector3 initialPosition;
    public float amplitude = 0.3f; // Decrease the amplitude for a smaller range of movement
    public float frequency = 0.5f; // Decrease the frequency for a slower movement

    void Start()
    {
        // Store the initial position of the candle
        initialPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // Animate up and down smoothly and slowly from the initial position
        transform.position = new Vector3(
            initialPosition.x,
            initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude,
            initialPosition.z
        );
    }
}
