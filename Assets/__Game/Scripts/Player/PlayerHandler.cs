using __Game.Scripts.Managers;
using System;
using UnityEngine;

namespace __Game.Scripts.Player {
    public sealed class PlayerHandler : MonoBehaviour {
        public static PlayerHandler Instance;

        //Events
        public event Action<PlayerState> OnPlayerStatedUpdate;
        public event Action OnPassedDistance;

        [Header("Movement param's")]
        [SerializeField] private GameObject uiControls;
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [SerializeField] private float movementSpeedIncreaseMult = 0.01f;
        [SerializeField] private float maxMovementSpeed = 20f;
        [SerializeField] private float xMovementSpeed;
        [SerializeField] private float xMovementClamp;
        [SerializeField] private float passedDistanceInterval = 80f;

        //Private
        private PlayerState playerState = PlayerState.None;
        private float lastZ = 0f;

        private Joystick joystick;

        private StickmanCollider stickmanCollider;

        private void Awake() {
            Instance = this;

            joystick = GetComponentInChildren<Joystick>();

            stickmanCollider = GetComponentInChildren<StickmanCollider>();
        }

        private void Start() {
            PickableHolderHandler.Instance.OnZeroPickablesCount += Death;
            stickmanCollider.OnStickmanCollidedWall += Death;
            GameHandler.Instance.OnUpdateState += StartMovement;

            uiControls.SetActive(false);
        }

        private void Update() {
            Movement();
            PassedDistance();
        }

        private void Movement() {
            if (playerState != PlayerState.Move) return;

            //Move character forward
            var forwardMovement = transform.forward * (MovementSpeed * Time.deltaTime);
            transform.Translate(forwardMovement);

            IncreaseSpeedByTime();

            //Horizontal movement
            var sidewaysMovement = transform.right * (joystick.Horizontal * xMovementSpeed * Time.deltaTime);
            transform.Translate(sidewaysMovement);

            //Limit horizontal movement
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xMovementClamp, xMovementClamp),
                transform.position.y,
                transform.position.z);
        }

        private void IncreaseSpeedByTime() {
            if (MovementSpeed < maxMovementSpeed)
                MovementSpeed += Time.deltaTime * movementSpeedIncreaseMult;
            if (MovementSpeed > maxMovementSpeed)
                MovementSpeed = maxMovementSpeed;
        }

        private void StartMovement(GameState state) {
            if (state == GameState.Started)
                UpdateState(PlayerState.Move);

            uiControls.SetActive(true);
        }

        private void PassedDistance() {
            if (transform.localPosition.z - lastZ >= passedDistanceInterval) {
                var resetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
                transform.localPosition = resetPos;

                OnPassedDistance?.Invoke();

                lastZ = transform.localPosition.z;
            }
        }

        private void UpdateState(PlayerState newState) {
            if (newState == playerState) return;

            playerState = newState;
            OnPlayerStatedUpdate?.Invoke(newState);
        }

        private void Death() {
            UpdateState(PlayerState.Dead);
            GameHandler.Instance.EndedGame();
        }
    }

    public enum PlayerState {
        None,
        Idle,
        Move,
        Dead
    }
}