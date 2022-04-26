using System;
using System.Threading.Tasks;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public interface IGameController
    {
        void TryAgain();
        void ReturnToMainMenu();
        void ContinueToNextLevel();
    }

    public class GameController : MonoBehaviour, IGameController
    {
        [SerializeField] private BallsController ballsController;
        [SerializeField] private PlayerController player;

        //Static global data holders
        public static Controls GameControls { get; private set; }
        public static GameConfig Config { get; private set; } 
        public static LevelConfig CurrentLevelConfig { get; private set;}
        
        //private internal variables
        private bool _gameIsFinished;
        
        //current level data
        private int _currentPlayerLevel;
        private int _levelAmount;
        internal static float remainingTimeInLevel { get; private set; }
        
        //reference to view
        private GameUiView _gameUiView;

        #region Events And Delegates

        //events
        
        public delegate void LevelStarted(int level);
        public static event LevelStarted OnLevelStarted;

        public delegate void LevelLost();
        public static event LevelLost OnLevelLost;

        public delegate void LevelWon();
        public static event LevelWon OnLevelWon;

        public delegate void GameWon();
        public static event GameWon OnGameWon;
        
        #endregion
        
        public void Init(GameDataModel model,GameUiView view)
        {
            _gameUiView = view;
            Config = model.gameConfig;
            _gameUiView.Init(this);
            SetResolution();
            SetInputControls();
            GenerateGame();
        }

        #region Game Setup and Initiation
        
        private void SetInputControls()
        {
            GameControls = new Controls();
            GameControls.Enable();
        }

        private void SetResolution()
        {
            Application.targetFrameRate = Config.DesiredFrameRate;
        }

        private void GenerateGame()
        {
            player.Init();
            player.OnPlayerHit += PlayerWasHit;
            GameControllerHelper.GenerateCollidersAcrossScreen();
            if (PlayerPrefs.HasKey("CurrentPlayerLevel"))
            {
                _currentPlayerLevel = PlayerPrefs.GetInt("CurrentPlayerLevel");
            }
            else PlayerPrefs.SetInt("CurrentPlayerLevel", _currentPlayerLevel);
            GetLevelData();
        }

        #endregion
        
        #region Level Setup and Flow

        private void GetLevelData()
        {
            CurrentLevelConfig = Config.Levels[_currentPlayerLevel];
            RunLevel();
        }
        
        private async void RunLevel()
        {
            OnOnLevelStarted(_currentPlayerLevel);
            var levelTime = CurrentLevelConfig.Time;
            while (levelTime > 0)
            {
                if (_gameIsFinished) return;
                levelTime -= Time.deltaTime;
                remainingTimeInLevel = levelTime;
                await Task.Yield();
            }
            LevelFailed();
        }
        
        private void LevelEnded()
        {
            _gameIsFinished = true;
            OnOnLevelWon();
        }

        #endregion

        #region Public User Input Methods

        public void TryAgain()
        {
            if (!_gameIsFinished) return;
            _gameIsFinished = false;
            GetLevelData();
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(Config.MenuSceneName);
        }

        public void ContinueToNextLevel()
        {
            if (!_gameIsFinished) return;
            _gameIsFinished = false;
            if (_currentPlayerLevel < Config.Levels.Length-1)
            {
                _currentPlayerLevel++;
                GetLevelData();
            }
            else
            {
                OnOnGameWon();
                SceneManager.LoadScene(Config.EndSceneName);
            }
        }

        #endregion

        #region Event Listeners
        
        private void NoMoreBallsOnScreen()
        {
            LevelEnded();
        }

        private void LevelFailed()
        {
            _gameIsFinished = true;
            OnOnLevelLost();
        }
        
        private void PlayerWasHit()
        {
            LevelFailed();
        }

        #endregion

        #region Event Invokers

        private static void OnOnLevelStarted(int level)
        {
            OnLevelStarted?.Invoke(level);
        }
        
        private static void OnOnLevelLost()
        {
            OnLevelLost?.Invoke();
        }

        private static void OnOnLevelWon()
        {
            OnLevelWon?.Invoke();
        }

        private static void OnOnGameWon()
        {
            OnGameWon?.Invoke();
        }

        #endregion

        private void OnDisable()
        {
            ballsController.OnBallsListEmpty -= NoMoreBallsOnScreen;
            player.OnPlayerHit -= PlayerWasHit;
        }
    }
}
