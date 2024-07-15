using UnityEngine;
using Wixot.AssetManagement.PoolManagement;

namespace Wixot.Weapon
{
    /// <summary>
    /// Basic Collide Script
    /// Todo upgrade rigidbody movement 
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public int speed;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Border"))
            {
                PoolManager.Instance.DestroyObject(this.gameObject, "Bullet");
            }
        }
        
        void Update()
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }
    }
}
