using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WandScript : MonoBehaviour
{
    private Camera playerCamera;
    public GameObject wand;
    private float distance;
    public Transform holdPoint;

    private bool isHolding = false;

    void Start()
    {
        playerCamera = Camera.main;
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
            }
            else
            {
                Debug.Log("Object is too far away");
            }
        }

        if (isHolding)
        {
            wand.transform.position = holdPoint.position;
        }

    }
}