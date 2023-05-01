using UnityEngine;

public class UILookAtCamera : MonoBehaviour {

    private enum LookMode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CamerForwardInverted
    }

    [SerializeField] private LookMode lookMode;

    private void LateUpdate() {
        switch (lookMode) {
            case LookMode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case LookMode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case LookMode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case LookMode.CamerForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}