using __Game.Scripts.Player;

namespace Assets.__Game.Scripts.Level {
    public sealed class CubeVisual : PickableVisual {

        private void Start() {
            PickableHolderHandler.Instance.OnMaxCapacityReached += ChangeMaterials;

            pickable = GetComponent<Cube>();
        }

        private void ChangeMaterials(bool change) {
            if (pickable.InStack) return;

            if (rend != null) {
                if (!change)
                    rend.material = firstMat;
                else
                    rend.material = secondMat;
            }

        }
    }
}
