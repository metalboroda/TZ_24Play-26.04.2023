using __Game.Scripts.Animation;
using CodeMonkey.Utils;
using UnityEngine;

namespace __Game.Scripts.Player {
    public sealed class CharacterAnimation : MonoBehaviour {
        [Header("Param's")]
        [SerializeField] private float crossfadeDuration = 0.15f;
        [SerializeField] private float jumpLandRecoveryTime = 0.25f;
        [SerializeField] private PlayerHandler playerHandler;

        [Header("Ragdoll param's")]
        [SerializeField] private Rigidbody charRb;
        [SerializeField] private Collider charColl;
        [SerializeField] private Rigidbody[] rigidbodies;
        [SerializeField] private Collider[] colliders;

        //Private
        private Animator animator;

        private readonly AnimationHash animationHash = new();

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Start() {
            PickableHolderHandler.Instance.OnPickableStacking += JumpAnim;
            playerHandler.OnPlayerStatedUpdate += UpdateAnimation;

            SwitchRagdoll(false);
        }

        private void UpdateAnimation(PlayerState state) {
            switch (state) {
                case PlayerState.Idle or PlayerState.Move:
                    IdleAnim();
                    break;
                case PlayerState.Dead:
                    DeathAnim();
                    break;
            }
        }

        private void IdleAnim() {
            animator.CrossFade(animationHash.idleAnim, crossfadeDuration);
        }

        private void JumpAnim() {
            animator.StopPlayback();
            animator.CrossFade(animationHash.jumpAnim, crossfadeDuration);
            FunctionTimer.Create(() => {
                IdleAnim();
            }, jumpLandRecoveryTime);
        }

        private void DeathAnim() {
            SwitchRagdoll(true);
            charRb.isKinematic = true;
            charColl.enabled = false;
            animator.enabled = false;
        }

        private void SwitchRagdoll(bool change) {
            foreach (var rb in rigidbodies) {
                rb.isKinematic = !change;
            }

            foreach (var coll in colliders) {
                coll.enabled = change;
            }
        }
    }
}