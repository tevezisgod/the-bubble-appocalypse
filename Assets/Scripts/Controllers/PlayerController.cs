using System;
using System.Threading.Tasks;
using Data.EventsHandler;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private PlayerView playerView;
        private bool MoveButtonPressed { get; set; }
        private bool m_CanMove;
        private float m_MovementOrientation;

        internal void Init()
        {
            GameController.GameControls.Player1.Move.started += OnPlayerMoved;
            GameController.GameControls.Player1.Move.canceled += OnPlayerStopped;
            GameController.GameControls.Player1.Fire.started += OnFirePressed;
            GameController.GameControls.Player1.Quit.performed += OnPlayerQuit;
            
            EventsHandler.StartListening(EventsHandler.EventNames.ONLevelStarted,OnLevelStarted);
            EventsHandler.StartListening(EventsHandler.EventNames.ONLevelLost,OnLevelLost);
            EventsHandler.StartListening(EventsHandler.EventNames.ONLevelWon,OnLevelWon);
            EventsHandler.StartListening(EventsHandler.EventNames.ONGameWon,OnGameWon);
            
            playerView.Init(this);
        }

        #region Player Actions Handling

        private void OnPlayerMoved(InputAction.CallbackContext callbackContext)
        {
            if (!m_CanMove) return;
            if (MoveButtonPressed) return;
            MoveButtonPressed = true;
            var move = callbackContext.ReadValue<Vector2>();
            MovePlayer(move);
        }
        private async void MovePlayer(Vector2 move)
        {
            while (MoveButtonPressed && playerView._playerRigidbody !=null)
            {
                m_MovementOrientation = move.x * GameController.Config.PlayerSpeed;
                var playerViewPlayerRigidbody = playerView._playerRigidbody;
                playerViewPlayerRigidbody.MovePosition(playerViewPlayerRigidbody.position + Vector2.right * (m_MovementOrientation * Time.fixedDeltaTime));
                await Task.Yield();
            }
        }

        private void OnPlayerStopped(InputAction.CallbackContext callbackContext)
        {
            if(MoveButtonPressed) MoveButtonPressed = false;
        }
        
        private void OnFirePressed(InputAction.CallbackContext obj)
        {
            if (!m_CanMove) return;
            var pos = playerView._playerRigidbody.position;
            weaponController.HandleFire(pos);
        }
        
        internal void PlayerWasHit()
        {
            EventsHandler.TriggerEvent(EventsHandler.EventNames.ONPlayerHit);
        }

        #endregion

        #region Event Listeners

        private void OnLevelWon()
        {
            m_CanMove = false;
        }

        private void OnLevelLost()
        {
            m_CanMove = false;
        }

        private void OnLevelStarted()
        {
            m_CanMove = true;
            playerView.Animator.SetTrigger(GameController.Config.PlayerLiveAnimation);
        }
        
        private void OnGameWon()
        {
            m_CanMove = false;
        }
        private void OnPlayerQuit(InputAction.CallbackContext obj)
        {
            Application.Quit();
        }

        #endregion

        private void OnDisable()
        {
            GameController.GameControls.Player1.Move.started -= OnPlayerMoved;
            GameController.GameControls.Player1.Move.canceled -= OnPlayerStopped;
            GameController.GameControls.Player1.Fire.started -= OnFirePressed;
            EventsHandler.StopListening(EventsHandler.EventNames.ONLevelStarted,OnLevelStarted);
            EventsHandler.StopListening(EventsHandler.EventNames.ONLevelLost,OnLevelLost);
            EventsHandler.StopListening(EventsHandler.EventNames.ONLevelWon,OnLevelWon);
            EventsHandler.StopListening(EventsHandler.EventNames.ONGameWon,OnGameWon);
        }
    }
}
