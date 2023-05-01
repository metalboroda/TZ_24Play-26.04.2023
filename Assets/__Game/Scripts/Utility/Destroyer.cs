using Lean.Pool;
using UnityEngine;

namespace Assets.__Game.Scripts.Utility {
    public class Destroyer : MonoBehaviour {
        [SerializeField] private DestroyType destroyType;
        [SerializeField] private float destroyTime;

        private void Start() {
            switch (destroyType) {
                case DestroyType.Destroy:
                    Destroy(gameObject, destroyTime);
                    break;
                case DestroyType.LeanPool:
                    LeanPool.Despawn(gameObject, destroyTime);
                    break;
                default:
                    break;
            }
        }
    }

    public enum DestroyType {
        Destroy,
        LeanPool
    }
}