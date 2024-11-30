using UnityEngine;

public class Game : MonoBehaviour
{
    public Transform handModel;
    public Transform objectToGrab;
    public float grabDistance = 1.0f; // Distance to grab the object

    // Game state
    private bool isGrabbing = false;
    private bool taskCompleted = false;

    // Reference to HandController
    public HandController handController;

    void Update()
    {
        // Check if the player is trying to grab an object
        CheckForGrab();

        // If object is grabbed, move it with the hand
        if (isGrabbing)
        {
            MoveObjectWithHand();
        }

        // Check if the task is completed
        if (taskCompleted)
        {
            // Add logic for task completion (e.g., move to the next challenge)
            Debug.Log("Task Completed!");
            // Trigger next level, score increase, etc.
        }
    }

    // Check if the player is performing a pinch gesture (thumb and index finger)
    void CheckForGrab()
    {
        // Get flex sensor values for thumb and index fingers
        int thumbValue = handController.flexSensor1Value;
        int indexValue = handController.flexSensor2Value;

        // Determine if the thumb and index are close to forming a "pinch"
        bool isPinching = thumbValue > 500 && indexValue > 500; // Adjust these values based on your calibration

        if (isPinching && !isGrabbing && Vector3.Distance(handModel.position, objectToGrab.position) < grabDistance)
        {
            // Grab the object if the pinch gesture is detected and within range
            isGrabbing = true;
        }

        // If the player opens the hand (releases the pinch), release the object
        if (isGrabbing && !isPinching)
        {
            ReleaseObject();
        }
    }

    // Move the object with the hand
    void MoveObjectWithHand()
    {
        // Move the object to follow the hand
        objectToGrab.position = handModel.position;
        objectToGrab.rotation = handModel.rotation;
    }

    // Release the object
    void ReleaseObject()
    {
        isGrabbing = false;
        taskCompleted = true; // In a real game, you might check if the object was placed correctly first
    }
}
