using __Game.Scripts.Level;
using __Game.Scripts.Player;
using CodeMonkey.Utils;
using DG.Tweening;
using UnityEngine;

namespace __Game.Scripts.Item {
    public class Pickable : MonoBehaviour {
        [field: SerializeField] public bool InStack { get; private set; }

        //Private
        private bool canCollide = true;

        private Rigidbody rb;
        private Collider coll;

        private PickableHolderHandler cubeHolderHandler;

        private void Awake() {
            rb = GetComponent<Rigidbody>();
            coll = GetComponent<Collider>();
        }

        private void Start() {
            cubeHolderHandler = PickableHolderHandler.Instance;
        }

        private void OnCollisionEnter(Collision collision) {
            CollideWithObstacle(collision);
        }

        private void OnTriggerEnter(Collider other) {
            Pick(other);
            Collect(other);
        }

        private void Pick(Collider other) {
            if (other.gameObject.TryGetComponent<Pickable>(out var pickable)) {
                if (cubeHolderHandler.GetPickablesList().Count
                    >= cubeHolderHandler.MaxPickablesCapacity) return;

                if (!pickable.InStack)
                    cubeHolderHandler.AddPickableToHolder(pickable);
            }
        }

        private void CollideWithObstacle(Collision collision) {
            if (collision.gameObject.TryGetComponent<ObstacleCube>(out var obstacle) && !obstacle.Collided && canCollide) {
                canCollide = false;
                obstacle.IsCollided(true);
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                cubeHolderHandler.RemovePickable(this);

                //Unfreeze rb           
                FunctionTimer.Create(() => {
                    rb.isKinematic = false;
                    rb.constraints &= ~RigidbodyConstraints.FreezeAll;
                    transform.DOScale(0, 1f).OnComplete(() => { Destroy(gameObject); });
                }, 0.25f);
            }
        }

        private void Collect(Collider other) {
            if (other.TryGetComponent(out ICollectible collectible)) {
                collectible.Collect();
            }
        }


        public void SetInStack(bool inStack) {
            InStack = inStack;
        }

        public void MakePhysical() {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
    }
}