using __Game.Scripts.Level;
using System;
using UnityEngine;

namespace __Game.Scripts.Player {
    public sealed class StickmanCollider : MonoBehaviour {
        //Events
        public event Action OnStickmanCollidedWall;

        private void OnCollisionEnter(Collision collision) {
            var obstacle = collision.gameObject.GetComponent<ObstacleCube>();

            if (obstacle != null && !obstacle.Collided) {
                OnStickmanCollidedWall?.Invoke();
            }
        }
    }
}