using UnityEngine;

public class SmoothRotateCube : MonoBehaviour
{
    public SerialController serialController;  // Ardity's SerialController reference
    private string[] gyroData;  // Array to store gyroscope data (gx, gy, gz)
    private Vector3 currentRotation;  // Current rotation of the cube
    private Vector3 targetRotation;  // Target rotation based on IMU data
    public float smoothingFactor = 5.0f;  // Smoothing factor for rotation
    public float rotationLimitX = 60.0f;  // Max rotation limit for X (like wrist rotation)
    public float rotationLimitY = 45.0f;  // Max rotation limit for Y (side-to-side)
    public float rotationLimitZ = 30.0f;  // Max rotation limit for Z (twisting)

    void Start()
    {
        // Initialize rotation values
        currentRotation = transform.rotation.eulerAngles;
        targetRotation = currentRotation;
    }

    void Update()
    {
        // Read the incoming serial data
        string message = serialController.ReadSerialMessage();

        if (message != null)
        {
            if (message == SerialController.SERIAL_DEVICE_CONNECTED)
            {
                Debug.Log("Arduino connected");
            }
            else if (message == SerialController.SERIAL_DEVICE_DISCONNECTED)
            {
                Debug.LogWarning("Arduino disconnected");
            }
            else
            {
                // Process gyroscope data
                gyroData = message.Split(',');

                if (gyroData.Length == 3)
                {
                    try
                    {
                        // Convert gyroscope data to floats
                        float gx = float.Parse(gyroData[0]) * 0.05f;  // Reduce sensitivity (scale down gyro data)
                        float gy = float.Parse(gyroData[1]) * 0.05f;  // Reduce sensitivity
                        float gz = float.Parse(gyroData[2]) * 0.05f;  // Reduce sensitivity

                        // Update the target rotation based on gyroscope data
                        targetRotation.x += gx * Time.deltaTime * smoothingFactor;  // Rotation around X-axis
                        targetRotation.y += gy * Time.deltaTime * smoothingFactor;  // Rotation around Y-axis
                        targetRotation.z += gz * Time.deltaTime * smoothingFactor;  // Rotation around Z-axis

                        // Clamp the rotations to mimic hand movement limits
                        targetRotation.x = Mathf.Clamp(targetRotation.x, -rotationLimitX, rotationLimitX);
                        targetRotation.y = Mathf.Clamp(targetRotation.y, -rotationLimitY, rotationLimitY);
                        targetRotation.z = Mathf.Clamp(targetRotation.z, -rotationLimitZ, rotationLimitZ);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning("Error parsing gyro data: " + e.Message);
                    }
                }
            }
        }

        // Smoothly interpolate between current rotation and target rotation
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, Time.deltaTime * smoothingFactor);

        // Apply the new rotation to the cube, converting Euler angles to Quaternion
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
