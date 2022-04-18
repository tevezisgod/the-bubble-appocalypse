using System.Threading.Tasks;
using UnityEngine;

namespace Controllers
{
    public class WeaponController : MonoBehaviour {

        private bool _isActive;
        private float _weaponSpeed;
        private Vector3 _playerPosition;
        [SerializeField] private WeaponCollider weaponCollider;

        public void Init()
        {
            _isActive = false;
            _weaponSpeed = GameController.Config.WeaponSpeed;
        }

        #region Weapon Usage handling

        public async void HandleFire(Vector3 position)
        {
            _isActive = true;
            _playerPosition = position;
            StartFiring();
            weaponCollider.Init(this);
            while (_isActive)
            {
                DoProgress();
                await Task.Yield();
            }
            transform.localScale = new Vector3(0f, 0f, 0f);
        }
        
        private void StartFiring()
        {
            transform.position = _playerPosition;
            transform.localScale = new Vector3(1f, 0f, 1f);
        }
        
        private void DoProgress()
        {
            if (!_isActive) return;
            transform.localScale += Vector3.up * (Time.deltaTime * _weaponSpeed);
        }
        
        #endregion

        //this method needs to be refactored to fire an event
        internal void HitSomething(Collider2D collision)
        {
            _isActive = false;
            if (collision.CompareTag(GameController.Config.BallTagName))
            {
                collision.GetComponent<IBallController>().PopBall();
            }    
        }
    }
}
