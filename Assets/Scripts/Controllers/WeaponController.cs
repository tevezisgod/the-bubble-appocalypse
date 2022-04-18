using System.Threading.Tasks;
using UnityEngine;

namespace Controllers
{
    public class WeaponController : MonoBehaviour {

        private bool _isFiring;
        private float _weaponSpeed;
        private Vector3 _playerPosition;
        
        [SerializeField] private WeaponCollider weaponCollider;

        public void Init()
        {
            _weaponSpeed = GameController.Config.WeaponSpeed;
            weaponCollider.OnWeaponHitSomething += OnWeaponHitSomething;
        }

        #region Weapon Usage handling

        private void StartFiring()
        {
            transform.position = _playerPosition;
            transform.localScale = new Vector3(1f, 0f, 1f);
        }
        
        public async void HandleFire(Vector3 position)
        {
            _isFiring = true;
            _playerPosition = position;
            StartFiring();
            while (_isFiring)
            {
                Proceed();
                await Task.Yield();
            }
            transform.localScale = new Vector3(0f, 0f, 0f);
        }

        private void Proceed()
        {
            if (!_isFiring) return;
            
            transform.localScale += Vector3.up * (Time.deltaTime * _weaponSpeed);
        }
        
        #endregion
        
        private void OnWeaponHitSomething(Collider2D col)
        {
            HitSomething(col);
        }

        //this method needs to be refactored to fire an event
        private void HitSomething(Collider2D collision)
        {
            _isFiring = false;
            
            if (collision.CompareTag(GameController.Config.BallTagName))
            {
                collision.GetComponent<IBallController>().PopBall();
            }    
        }
    }
}
