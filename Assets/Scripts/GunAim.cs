using UnityEngine;

public class GunAim : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform hipPosition;
    [SerializeField] private Transform aimPosition;

    [Header("Aim Settings")]
    [SerializeField] private float aimSpeed = 12f;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float aimFOV = 50f;

    [Header("Mouse Sensitivity")]
    [SerializeField] private PlayerControllerFPS playerController;
    [SerializeField] private float aimingSensitivityMultiplier = 0.7f;

    public bool IsAiming { get; private set; }

    private void Update()
    {
        IsAiming = Input.GetMouseButton(1);

        UpdateGunPosition();
        UpdateCameraFOV();
        UpdateMouseSensitivity();
    }

    private void UpdateGunPosition()
    {
        if (hipPosition == null || aimPosition == null) return;

        Transform targetPosition = IsAiming ? aimPosition : hipPosition;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition.position,
            Time.deltaTime * aimSpeed
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetPosition.rotation,
            Time.deltaTime * aimSpeed
        );
    }

    private void UpdateCameraFOV()
    {
        if (playerCamera == null) return;

        float targetFOV = IsAiming ? aimFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * aimSpeed
        );
    }

    private void UpdateMouseSensitivity()
    {
        if (playerController == null) return;

        if (IsAiming)
        {
            playerController.SetSensitivityMultiplier(aimingSensitivityMultiplier);
        }
        else
        {
            playerController.SetSensitivityMultiplier(1f);
        }
    }
}