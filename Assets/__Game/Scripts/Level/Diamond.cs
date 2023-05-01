using __Game.Scripts.Managers;
using Lean.Pool;
using UnityEngine;

namespace Assets.__Game.Scripts.Level {
    public class Diamond : Collectible, ICollectible {

        public void Collect() {
            GameHandler.Instance.UpdateDiamonds();
            LeanPool.Spawn(collectVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}