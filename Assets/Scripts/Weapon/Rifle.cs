using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Wixot.AssetManagement.PoolManagement;

namespace Wixot.Weapon
{
    public class Rifle : BasicWeapon
    {
        private CancellationTokenSource _shootingCancellationToken;
        private PoolManager _poolManager;
        public override void StartShooting()
        {
            _poolManager = PoolManager.Instance;
            _shootingCancellationToken = new CancellationTokenSource();
            AutoFire(_shootingCancellationToken.Token);
        }

        public override void StopShooting()
        {
            _shootingCancellationToken.Cancel();
        }
        

        /// <summary>
        /// Spawning bullet from object pool and giving first rotation and position
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="direction"></param>
        private void SpawnBullet(int speed, Vector3 direction)
        {
            var poolObject = _poolManager.GetPoolObject("Bullet");
            poolObject.transform.position = transform.position;
            
            Quaternion rotation;
            rotation = transform.rotation;
            rotation *= Quaternion.Euler(direction);
            
            poolObject.transform.rotation = rotation;
            
            // var bullet = Instantiate<Bullet>(AssetManagement.AssetManager.Instance.GetLoadedAsset<Bullet>("Bullet"));
            poolObject.TryGetComponent<Bullet>(out var bullet);
            bullet.speed = speed;
        }

       

        /// <summary>
        /// Simulating pulling trigger
        /// </summary>
        /// <param name="token"></param>
        private async void AutoFire(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(2000 / Stats.FireRate , token);
                    List<Vector3> bulletDirections = GetSpawnPoints(Stats.NozzleCount);
                    foreach (var direction in bulletDirections)
                    {
                   
                        ThrowBullets(Stats.BulletCount, direction ,token);
                    }
                }
            }
            catch (TaskCanceledException e)
            {
                Debug.Log("Task Canceled" + e);
            }
            catch (Exception ex)
            {
                Debug.Log("Error: " + ex.Message);
            }
           
        }

        /// <summary>
        /// simulating throwing bullet for each nozzle
        /// </summary>
        /// <param name="count"> bullet count </param>
        /// <param name="direction"> direction </param>
        private async void ThrowBullets(int count,Vector3 direction,CancellationToken token)
        {
            try
            {
                for (int i = 0; i < count; i++)
                {
                    await Task.Delay(250,token);
                    SpawnBullet(Stats.BulletSpeed, direction);
                }
            }
            catch (TaskCanceledException e)
            {
                Debug.Log("Task Canceled" +e);
            }
            catch (Exception ex)
            {
                Debug.Log("Error: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Creating Bullet directions for nozzle count modifier
        /// </summary>
        /// <param name="nozzleCount"></param>
        /// <returns></returns>
        private List<Vector3> GetSpawnPoints(int nozzleCount)
        {
            List<Vector3> vectors = new List<Vector3>();

            float angleStep = 45f; 
            float currentAngle = 0f; //startAngel

            for (int i = 0; i < nozzleCount; i++)
            {
                if (i == 0)
                {
                    vectors.Add(new Vector3(0, 0, currentAngle));
                }
                else if (i % 2 == 1)
                {
                    vectors.Add(new Vector3(0, 0, -angleStep)); 
                }
                else
                {
                    vectors.Add(new Vector3(0, 0, angleStep)); 
                    angleStep += 45f;
                }
            }

            return vectors;
        }

        private void OnDestroy()
        {
            _shootingCancellationToken.Cancel();
        }
        
    }
    
}
