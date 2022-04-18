using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private PlayerView playerView;
        
        private bool _moveButtonPressed;
        private bool _canMove;
        private float _movementOrientation;

        #region Events and delegates

        //events
        public delegate void PlayerHit();
        public event PlayerHit OnPlayerHit;

        #endregion
        
        internal void Init()
        {
            GameController.GameControls.Player1.Move.started += OnPlayerMoved;
            GameController.GameControls.Player1.Move.canceled += OnPlayerStopped;
            GameController.GameControls.Player1.Fire.started += OnFirePressed;
            GameController.GameControls.Player1.Quit.performed += OnPlayerQuit;
            GameController.OnLevelStarted += OnLevelStarted;
            GameController.OnLevelLost += OnLevelLost;
            GameController.OnLevelWon += OnLevelWon;
            GameController.OnGameWon += OnGameWon;
            playerView.Init(this);
        }

        #region Player Actions Handling

        private void OnPlayerMoved(InputAction.CallbackContext callbackContext)
        {
            if (!_canMove) return;
            _moveButtonPressed = true;
            var move = callbackContext.ReadValue<Vector2>();
            MovePlayer(move);
        }
        private async void MovePlayer(Vector2 move)
        {
            while (_moveButtonPressed && playerView.PlayerRigidbody !=null)
            {
                _movementOrientation = move.x * GameController.Config.PlayerSpeed;
                var playerRigidbody = playerView.PlayerRigidbody;
                playerRigidbody.MovePosition(playerRigidbody.position + Vector2.right * (_movementOrientation * Time.fixedDeltaTime));
                await Task.Yield();
            }
        }

        private void OnPlayerStopped(InputAction.CallbackContext callbackContext)
        {
            if(_moveButtonPressed) _moveButtonPressed = false;
        }
        
        private void OnFirePressed(InputAction.CallbackContext obj)
        {
            if (!_canMove) return;
            var pos = playerView.transform.position;
            weaponController.HandleFire(pos);
        }

        #endregion

        #region Player Damage

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(GameController.Config.BallTagName))
            {
                PlayerWasHit();
            }
        }
        internal void PlayerWasHit()
        {
            OnOnPlayerHit();
        }

        #endregion

        #region Event Listeners

        private void OnLevelWon()
        {
            _canMove = false;
        }

        private void OnLevelLost()
        {
            _canMove = false;
        }

        private void OnLevelStarted(int level)
        {
            _canMove = true;
            weaponController.Init();
        }
        
        private void OnGameWon()
        {
            _canMove = false;
        }
        private void OnPlayerQuit(InputAction.CallbackContext obj)
        {
            Application.Quit();
        }

        #endregion

        #region Event Invokers

        protected virtual void OnOnPlayerHit()
        {
            OnPlayerHit?.Invoke();
        }

        #endregion

        private void OnDisable()
        {
            GameController.GameControls.Player1.Move.started -= OnPlayerMoved;
            GameController.GameControls.Player1.Move.canceled -= OnPlayerStopped;
            GameController.GameControls.Player1.Fire.started -= OnFirePressed;
            GameController.OnLevelStarted -= OnLevelStarted;
            GameController.OnLevelLost -= OnLevelLost;
            GameController.OnLevelWon -= OnLevelWon;
            GameController.OnGameWon -= OnGameWon;
        }
    }
}
