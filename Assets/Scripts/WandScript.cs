using System.Collections;
using UnityEngine;

public class WandScript : MonoBehaviour
{
    private Camera playerCamera;
    public GameObject wand;
    public Transform holdPoint;
    private bool isHolding = false;
    private bool isMovingWand = false;
    private Rigidbody wandRigidbody;
    private float wandMovingTime = 0.0f;

    public GameObject particleEffect;
    public GameObject[] randomObjects;
    public GameObject tornadoEffect;
    private Vector3[] initialPositions;

    public GameObject incendioPrefab;

    public float MagicDuration = 5f;
    public float PickUpRadius = 2.0f;
    public float WandSpeed = 10.0f;
    public Vector3 MovementRange = new Vector3(5.0f, 5.0f, 5.0f);
    public float WandMagicStart = 0.5f;
    public float WandMoveDuration = 3.0f; // Duration to move the wand to hold point

    public AudioClip wandMovingSound;
    public AudioClip incendioSpellSound;
    private AudioSource audioSource;
    private bool isCallingWand = false;

    void Start()
    {
        playerCamera = Camera.main;
        wandRigidbody = wand.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Deactivate the particle effect GameObject initially
        if (particleEffect != null)
        {
            particleEffect.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(playerCamera.transform.position, wand.transform.position);

            if (distance < PickUpRadius && !isHolding)
            {
                isHolding = true;
            }
            else if (isHolding)
            {
                isHolding = false;
                isMovingWand = false;
                wandRigidbody.isKinematic = false;
            }
            else
            {
                Debug.Log("Object is too far away");
            }

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {


            if (!isHolding && !isMovingWand)
            {

                StartCoroutine(MoveWandToHoldPoint());
            }
        }

        else if (Input.GetKeyDown(KeyCode.I)) // Check for the "I" key press
        {
            StartCoroutine(CastIncendioSpell());

        }

        if (isHolding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMovingWand = true;
                FindObjectOfType<FirstPersonLook>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                wandRigidbody.isKinematic = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMovingWand = false;
                FindObjectOfType<FirstPersonLook>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                wandRigidbody.isKinematic = false;
            }

            if (!isMovingWand)
            {
                UpdateWandPositionAndRotation();
            }
        }

        if (isMovingWand && !isCallingWand)
        {
            MoveWandWithMouse();
            if (wandMovingTime > WandMagicStart)
            {
                wandMovingTime = 0.0f;
                SpawnParticleEffect(wand.transform.position);
            }
        }

    }

    void MoveWandWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        wand.transform.Translate(new Vector3(mouseX, mouseY, 0) * Time.deltaTime * WandSpeed);
        wandMovingTime += Time.deltaTime;

        if (!audioSource.isPlaying && wandMovingSound != null) // Check if the sound is not already playing
        {
            audioSource.PlayOneShot(wandMovingSound); // Play the sound effect
        }

    }

    void UpdateWandPositionAndRotation()
    {
        wand.transform.position = holdPoint.position;
        wand.transform.rotation = playerCamera.transform.rotation;
        wand.transform.position += playerCamera.transform.forward * 0.5f; // Adjust the distance from the camera as necessary
    }

    void SpawnParticleEffect(Vector3 position)
    {
        if (particleEffect != null)
        {
            StartCoroutine(EnableDisableParticleEffect(particleEffect));
        }
        else
        {
            Debug.LogWarning("Particle effect prefab not assigned.");
        }
    }

    IEnumerator EnableDisableParticleEffect(GameObject particleEffectObject)
    {
        particleEffectObject.SetActive(true); // Enable the game object
        tornadoEffect.SetActive(true); // Enable the tornado effect

        StoreInitialPositions();
        foreach (GameObject obj in randomObjects)
        {
            StartCoroutine(MoveRandomObject(obj));
        }
        yield return new WaitForSeconds(MagicDuration); // Wait for 5 seconds
        particleEffectObject.SetActive(false); // Disable the game object
        tornadoEffect.SetActive(false); // Disable the tornado effect   

        RestoreInitialPositions();
    }

    IEnumerator MoveRandomObject(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = startPosition + new Vector3(Random.Range(-MovementRange.x, MovementRange.x), Random.Range(-MovementRange.y, MovementRange.y), Random.Range(-MovementRange.z, MovementRange.z));
        float duration = 2f; // Duration of the movement
        float elapsedTime = 0f;

        // Generate random rotation angles around the X and Y axes
        float rotationAngleX = Random.Range(0f, 360f);
        float rotationAngleY = Random.Range(0f, 360f);

        Quaternion startRotation = obj.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(rotationAngleX, rotationAngleY, 0f);

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

            // Rotate the object around both the X and Y axes
            obj.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = endPosition;
        obj.transform.rotation = endRotation; // Set the final rotation
    }

    void StoreInitialPositions()
    {
        initialPositions = new Vector3[randomObjects.Length];
        for (int i = 0; i < randomObjects.Length; i++)
        {
            initialPositions[i] = randomObjects[i].transform.position;
        }
    }

    void RestoreInitialPositions()
    {
        for (int i = 0; i < randomObjects.Length; i++)
        {
            randomObjects[i].transform.position = initialPositions[i];
        }
    }

    IEnumerator MoveWandToHoldPoint()
    {
        isMovingWand = true;
        isCallingWand = true;

        Vector3 startPosition = wand.transform.position;
        Quaternion startRotation = wand.transform.rotation;
        Vector3 endPosition = holdPoint.position;
        Quaternion endRotation = playerCamera.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < WandMoveDuration)
        {
            wand.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / WandMoveDuration);
            wand.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / WandMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wand.transform.position = endPosition;
        wand.transform.rotation = endRotation;
        isMovingWand = false;
        isHolding = true;
        isCallingWand = false;
    }

    IEnumerator CastIncendioSpell()
    {

        if (incendioPrefab != null)
        {
            incendioPrefab.SetActive(true);

            if (incendioSpellSound != null)
            {
                audioSource.PlayOneShot(incendioSpellSound);
            }
            yield return new WaitForSeconds(2.0f);
            incendioPrefab.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Incendio prefab not assigned.");
        }
    }
}
