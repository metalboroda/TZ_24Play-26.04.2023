using __Game.Scripts.Item;
using UnityEngine;

namespace Assets.__Game.Scripts.Level {
    public class PickableVisual : MonoBehaviour {

        [SerializeField] protected Pickable pickable;

        [Header("Material change param's")]
        [SerializeField] protected Renderer rend;
        [SerializeField] protected Material firstMat;
        [SerializeField] protected Material secondMat;
    }
}