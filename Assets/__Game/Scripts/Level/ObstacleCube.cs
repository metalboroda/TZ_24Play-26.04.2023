using UnityEngine;

namespace __Game.Scripts.Level {
    public class ObstacleCube : MonoBehaviour {
        public bool Collided { get; private set; }

        public void IsCollided(bool change) {
            Collided = change;
        }
    }
}