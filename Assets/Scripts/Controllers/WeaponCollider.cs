using UnityEngine;

namespace Controllers
{
    public class WeaponCollider : MonoBehaviour
    {
        private WeaponController _weaponController;

        public delegate void WeaponHitSomething(Collider2D col);
        public event WeaponHitSomething OnWeaponHitSomething;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnOnWeaponHitSomething(collision);
        }

        protected virtual void OnOnWeaponHitSomething(Collider2D col)
        {
            OnWeaponHitSomething?.Invoke(col);
        }
    }
}
