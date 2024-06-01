using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public GameObject objectToGrab; // Reference to the GameObject you want to grab
    private bool isGrabbing = false; // Flag to track if the object is being grabbed
    private FixedJoint joint; // Reference to the joint used for grabbing

    void Start()
    {
        Debug.Log("GrabObject script started.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // 'E' key pressed
        {
            Debug.Log("'E' key pressed.");
            if (!isGrabbing)
            {
                Debug.Log("Grabbing object.");
                Grab();
            }
            else
            {
                Debug.Log("Releasing object.");
                Release();
            }
        }
    }

    void Grab()
    {
        Debug.Log("Grab method called.");
        isGrabbing = true;
        joint = objectToGrab.AddComponent<FixedJoint>();
        joint.connectedBody = null; // Connect to a fixed point in the world
        joint.anchor = Camera.main.transform.position; // Set the anchor to the camera's position
    }

    void Release()
    {
        Debug.Log("Release method called.");
        isGrabbing = false;
        Destroy(joint);
    }
}