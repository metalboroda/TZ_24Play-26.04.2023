using __Game.Scripts.Player;
using Cinemachine;
using CodeMonkey.Utils;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    [SerializeField] private float intensity;

    //Private
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    private void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start() {
        PickableHolderHandler.Instance.OnPickableStacking += Shake;

        perlinNoise.m_AmplitudeGain = 0;
    }

    private void Shake() {
        perlinNoise.m_AmplitudeGain = intensity;

        FunctionTimer.Create(() => {
            perlinNoise.m_AmplitudeGain = 0;
        }, 0.15f);
    }
}