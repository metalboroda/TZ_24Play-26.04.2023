using __Game.Scripts.Player;
using UnityEngine;

namespace Assets.__Game.Scripts.VFX {
    public class VFXHandler : MonoBehaviour {
        [SerializeField] private bool inPool = true;
        [SerializeField] private bool needResetPosition = false;
        [SerializeField] private bool relativeToPlyaer = false;

        //Private
        private ParticleSystem particle;

        private void Awake() {
            particle = GetComponent<ParticleSystem>();
        }

        private void Start() {
            PlayerHandler.Instance.OnPassedDistance += ResetPosition;
        }

        private void OnEnable() {
            if (inPool) {
                particle.Stop();
                particle.Play();
            }
        }

        private void ResetPosition() {
            if (needResetPosition) {
                var resetPos = new Vector3(transform.position.x, transform.position.y, 0);
                transform.position = resetPos;
            }

            if (needResetPosition && relativeToPlyaer) {
                var resetPos = new Vector3(transform.position.x, transform.position.y, PlayerHandler.Instance.transform.position.z);
                transform.position = resetPos;
            }
        }
    }
}