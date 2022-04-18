using System;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class GameUiView: MonoBehaviour
{
    [Header("Text fields")]
    [SerializeField] internal Text timeText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text winText;
    [SerializeField] private Text levelText;
    
    [Header("Buttons")]
    [SerializeField] private Button tryAgain;
    [SerializeField] private Button returnToMainMenu;
    [SerializeField] private Button continueToNextLevel;
    
    [Header("Misc")]
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private AudioSource musicPlayer;

    public void Init(IGameController controller)
    {
        SetupButtons(controller);
        GameController.OnLevelStarted += OnLevelStarted;
        GameController.OnLevelLost += OnLevelLost;
        GameController.OnLevelWon += OnLevelWon;
        GameController.OnGameWon += OnGameWon;
    }
    
    private void SetupButtons(IGameController controller)
    {
        tryAgain.onClick.AddListener(controller.TryAgain);
        returnToMainMenu.onClick.AddListener(controller.ReturnToMainMenu);
        continueToNextLevel.onClick.AddListener(controller.ContinueToNextLevel);
        tryAgain.gameObject.SetActive(false);
        returnToMainMenu.gameObject.SetActive(false);
        continueToNextLevel.gameObject.SetActive(false);
    }

    #region UI Handling Methods

    private void ToggleEndScreen(bool toggle)
    {
        winText.gameObject.SetActive(toggle);
        returnToMainMenu.gameObject.SetActive(toggle);
        continueToNextLevel.gameObject.SetActive(toggle);
    }

    private void ToggleRestartScreen(bool toggle)
    {
        timeText.color = toggle ? Color.red : Color.white;
        winText.gameObject.SetActive(!toggle);
        gameOverText.gameObject.SetActive(toggle);
        tryAgain.gameObject.SetActive(toggle);
        returnToMainMenu.gameObject.SetActive(toggle);
    }

    #endregion
    
    internal void SetLevelView(Sprite newBackground, AudioClip music)
    {
        background.sprite = newBackground;
        musicPlayer.clip = music;
        musicPlayer.Play();
    }
    
    #region Event Listeners

    private void OnGameWon()
    {
        Debug.Log("No implementation yet");
    }

    private void OnLevelWon()
    {
        ToggleEndScreen(true);
    }

    private void OnLevelLost()
    { 
        ToggleRestartScreen(true);
    }

    private void OnLevelStarted(int level)
    {
        ToggleRestartScreen(false);
        ToggleEndScreen(false);
        levelText.text = "Level " + (level+1);
    }

    #endregion
    
    private void OnDestroy()
    {
        tryAgain.onClick.RemoveAllListeners();
        returnToMainMenu.onClick.RemoveAllListeners();
        continueToNextLevel.onClick.RemoveAllListeners();
        GameController.OnLevelStarted -= OnLevelStarted;
        GameController.OnLevelLost -= OnLevelLost;
        GameController.OnLevelWon -= OnLevelWon;
        GameController.OnGameWon -= OnGameWon;
    }
}