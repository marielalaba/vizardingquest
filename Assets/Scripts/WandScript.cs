using System.Collections;
using System.Collections.Generic;
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
    private Vector3[] initialPositions;

    public float MagicDuration = 5f;
    public float PickUpRadius = 2.0f;
    public float WandSpeed = 10.0f;
    public Vector3 MovementRange = new Vector3(5.0f, 5.0f, 5.0f);
    public float WandMagicStart = 0.5f;

    void Start()
    {
        playerCamera = Camera.main;
        wandRigidbody = wand.GetComponent<Rigidbody>();
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

        if (isMovingWand)
        {
            MoveWandWithMouse();
        }
        if (wandMovingTime > WandMagicStart)
        {
            wandMovingTime = 0.0f;
            SpawnParticleEffect(wand.transform.position);
        }
    }

    void MoveWandWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        wand.transform.Translate(new Vector3(mouseX, mouseY, 0) * Time.deltaTime * WandSpeed);
        wandMovingTime += Time.deltaTime;
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
        foreach (GameObject obj in randomObjects)
        {
            StartCoroutine(MoveRandomObject(obj));
        }
        yield return new WaitForSeconds(MagicDuration); // Wait for 5 seconds
        particleEffectObject.SetActive(false); // Disable the game object
        RestoreInitialPositions();
    }

    IEnumerator MoveRandomObject(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = startPosition + new Vector3(Random.Range(-MovementRange.x, MovementRange.x), Random.Range(-MovementRange.y, MovementRange.y), Random.Range(-MovementRange.z, MovementRange.z));
        float duration = 2f; // Duration of the movement
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = endPosition;
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

}




