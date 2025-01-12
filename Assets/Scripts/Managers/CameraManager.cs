using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera thirdPersonCamera;

    public void SetupCamera(Transform playerTransform)
    {
        if (thirdPersonCamera != null)
        {
            thirdPersonCamera.transform.SetParent(playerTransform);
            thirdPersonCamera.transform.localPosition = new Vector3(0, 2, -3);
            thirdPersonCamera.transform.localRotation = Quaternion.Euler(25, 0, 0);
        }
        else
        {
            Debug.LogWarning("No se ha asignado una cámara de tercera persona.");
        }
    }
}