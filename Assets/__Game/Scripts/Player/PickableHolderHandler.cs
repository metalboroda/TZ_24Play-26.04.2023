using __Game.Scripts.Item;
using Lean.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __Game.Scripts.Player {
    public sealed class PickableHolderHandler : MonoBehaviour {
        public static PickableHolderHandler Instance;

        [field: SerializeField]
        public int MaxPickablesCapacity { get; private set; }

        [Header("VFX param's")]
        [SerializeField] private GameObject pickVFX;

        //Events
        public event Action OnPickableStacking;
        public event Action OnPickableRemove;
        public event Action OnZeroPickablesCount;
        public event Action<bool> OnMaxCapacityReached;

        [SerializeField] private GameObject character;
        [SerializeField] private GameObject holder;
        [SerializeField] private List<Pickable> pickables = new();

        //Private
        private float pickableHeight;

        private void Awake() {
            Instance = this;
        }

        public void AddPickableToHolder(Pickable stackedPickable) {
            //Get cube height
            pickableHeight = stackedPickable.transform.localScale.y;

            //Move cubes and character up
            character.transform.localPosition = new Vector3(0, character.transform.localPosition.y + pickableHeight * 2.5f);

            foreach (var pickable in pickables) {
                pickable.transform.localPosition = new Vector3(0, pickable.transform.localPosition.y + pickableHeight * 2);
            }

            //Add cube
            stackedPickable.transform.SetParent(holder.transform);
            stackedPickable.transform.localPosition = new Vector3(holder.transform.localPosition.x,
                holder.transform.localPosition.y);
            stackedPickable.SetInStack(true);
            stackedPickable.MakePhysical();

            AddPickableToList(stackedPickable);
            CheckMaxCapacityReached();
            OnPickableStacking?.Invoke();
            SpawnPickVFX();
        }

        private void AddPickableToList(Pickable pickable) {
            pickables.Add(pickable);
        }

        public void RemovePickable(Pickable pickable) {
            foreach (var unused in pickables.Where(cube => pickable == cube)) {
                pickable.transform.SetParent(null);
            }

            pickables.Remove(pickable);
            OnPickableRemove?.Invoke();
            CheckPickablesCounter();
            CheckMaxCapacityReached();
        }

        private void CheckPickablesCounter() {
            if (GetPickablesList().Count < 1) {
                OnZeroPickablesCount?.Invoke();
            }
        }

        private void CheckMaxCapacityReached() {
            if (GetPickablesList().Count >= MaxPickablesCapacity)
                OnMaxCapacityReached?.Invoke(true);
            else
                OnMaxCapacityReached?.Invoke(false);
        }

        public List<Pickable> GetPickablesList() {
            return pickables;
        }

        private void SpawnPickVFX() {
            var spawnPos = new Vector3(pickables.First().transform.position.x,
                pickables.First().transform.position.y + 1f,
                pickables.First().transform.position.z);

            LeanPool.Spawn(pickVFX, spawnPos, Quaternion.identity);
        }
    }
}