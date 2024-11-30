using UnityEngine;

/**
 * This script controls the rotation of the hand bones based on the flex sensor data received via Ardity's SerialController.
 */
public class HandController : MonoBehaviour
{
    // Assign the finger bones in the Unity editor (thumb, middle, and ring fingers)
    public Transform thumbBone;
    public Transform middleFingerBone;
    public Transform ringFingerBone;

    // Reference to the Ardity SerialController to receive data from Arduino
    public SerialController serialController;

    // Store the flex sensor values for each finger
    public int flexSensor1Value = 0;  // Thumb
    public int flexSensor2Value = 0;  // Middle Finger
    public int flexSensor3Value = 0;  // Ring Finger

    void Update()
    {
        // Check if the serial controller is receiving data
        string message = serialController.ReadSerialMessage();

        if (!string.IsNullOrEmpty(message))
        {
            // Split the message by commas to get individual sensor values
            string[] sensorValues = message.Split(',');

            // Ensure there are exactly 3 values for the three fingers
            if (sensorValues.Length == 3)
            {
                // Parse the flex sensor values for the thumb, middle, and ring fingers
                flexSensor1Value = int.Parse(sensorValues[0]);  // Thumb
                flexSensor2Value = int.Parse(sensorValues[1]);  // Middle Finger
                flexSensor3Value = int.Parse(sensorValues[2]);  // Ring Finger
            }
        }

        // Update the finger rotations based on the flex sensor values
        UpdateFingerRotation(thumbBone, flexSensor1Value, true);  // Thumb (rotate around Z-axis)
        UpdateFingerRotation(middleFingerBone, flexSensor2Value, false);  // Middle Finger (rotate around X-axis)
        UpdateFingerRotation(ringFingerBone, flexSensor3Value, false);  // Ring Finger (rotate around X-axis)
    }

    // Update the rotation of each finger based on the flex sensor value
    void UpdateFingerRotation(Transform fingerBone, int flexValue, bool isThumb)
    {
        // Map flex sensor value (0-1023) to a reasonable rotation range (0-90 degrees)
        float rotationAmount = Mathf.Lerp(0f, 120f, flexValue / 1023f);

        // Apply the rotation to the finger bone with different axes for thumb and other fingers
        if (isThumb)
        {
            // Rotate thumb around the Z-axis (adjust as needed)
            fingerBone.localRotation = Quaternion.Euler(0f, rotationAmount, 50f);
        }
        else
        {
            // Rotate other fingers around the X-axis
            fingerBone.localRotation = Quaternion.Euler(rotationAmount, 0f, 0f);
        }
    }
}
