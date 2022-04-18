
using Controllers;
using Data;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameDataModel gameDataModel;
    [SerializeField] private GameUiView gameUiView;

    private void Awake()
    {
        gameController.Init(gameDataModel,gameUiView);
    }
}
