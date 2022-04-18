using UnityEngine;

namespace Controllers
{
    public class WeaponCollider : MonoBehaviour
    {
        private WeaponController _weaponController;

        public void Init(WeaponController weaponController)
        {
            _weaponController = weaponController;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            _weaponController.HitSomething(collision);
        }
    }
}
