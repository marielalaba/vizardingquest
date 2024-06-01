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

            if (distance < 2.0f && !isHolding)
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
        if (wandMovingTime > 0.5f)
        {
            wandMovingTime = 0.0f;
            SpawnParticleEffect(wand.transform.position);
        }
    }

    void MoveWandWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        wand.transform.Translate(new Vector3(mouseX, mouseY, 0) * Time.deltaTime * 10); // Adjust speed as necessary
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
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        particleEffectObject.SetActive(false); // Disable the game object
    }
}