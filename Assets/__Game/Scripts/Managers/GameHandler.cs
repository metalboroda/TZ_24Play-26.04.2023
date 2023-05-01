using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __Game.Scripts.Managers {
    public sealed class GameHandler : MonoBehaviour {
        public static GameHandler Instance;

        //Events
        public event Action<GameState> OnUpdateState;

        [Header("Screens")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject endScreen;

        [Header("Screen buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button restartButton;

        [Header("Diamond game param's")]
        [SerializeField] private int diamondCounter = 0;
        [SerializeField] private TextMeshProUGUI diamondGameText;

        [Header("Diamond end param's")]
        [SerializeField] private TextMeshProUGUI diamondEndText;

        //Private
        private GameState gameState;
        public GameState GameState { get; private set; }

        private GameObject[] screens;

        private void Awake() {
            Instance = this;

            screens = new[] { startScreen, gameScreen, endScreen };
        }

        private void Start() {
            startButton.onClick.AddListener(StartGame);
            restartButton.onClick.AddListener(RestartGame);

            AtStartGame();
        }

        private void UpdateState(GameState newState) {
            if (newState != gameState) {
                gameState = newState;
                OnUpdateState?.Invoke(newState);
            }
        }

        private void AtStartGame() {
            UpdateState(GameState.AtStart);
            SwitchScreen(startScreen);

            diamondCounter = 0;
        }

        private void StartGame() {
            UpdateState(GameState.Started);
            SwitchScreen(gameScreen);
        }

        public void EndedGame() {
            UpdateState(GameState.Ended);
            SwitchScreen(endScreen);

            diamondEndText.SetText(diamondCounter.ToString());
        }

        private void RestartGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SwitchScreen(GameObject screen) {
            foreach (var i in screens) {
                if (i == screen)
                    i.SetActive(true);
                else
                    i.SetActive(false);
            }
        }
        private void DisableAllScreens() {
            foreach (var i in screens) {
                i.SetActive(false);
            }
        }

        public void UpdateDiamonds() {
            diamondCounter++;

            diamondGameText.SetText(diamondCounter.ToString());
        }
    }
}

public enum GameState {
    None,
    AtStart,
    Started,
    Ended
}