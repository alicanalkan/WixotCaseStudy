using Unity.Mathematics;
using UnityEngine;
using Wixot.AssetManagement;
using Wixot.AssetManagement.PoolManagement;

namespace Wixot.Environment.Grid
{
    public class WorldGeneration : MonoBehaviour
    {
        [Tooltip("Tile Texture size 256x256")]
        [SerializeField] private Vector2 tileSize;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private string groundObjectTag;
        
        private Transform[,] grid;
        private AssetManager _assetManager;
        private PoolManager _poolManager;
        
        /// <summary>
        /// Creating Grid base platform Generation 
        /// </summary>
        void Start()
        {
            _assetManager = AssetManager.Instance;
            _poolManager = PoolManager.Instance;
            
            grid = new Transform[gridSize.x, gridSize.y];
            
            GenerateGround();
            _assetManager.LoadAsset<GameObject>("Boundary",GenerateBoundaryBox);
            
            GameManager.Instance.StartLoad();
        }


        /// <summary>
        /// Instantiate tile objects
        /// </summary>
        private void GenerateGround()
        {
            Vector2 centerOffset = new Vector2(tileSize.x * gridSize.x / 2, tileSize.y * gridSize.y / 2);
            
            for (int x = 1; x < gridSize.x; x++)
            {
                for (int y = 1; y < gridSize.y; y++)
                {
                    var ground = _poolManager.GetPoolObject(groundObjectTag);
                    ground.transform.position = new Vector2(x * tileSize.x, y *tileSize.y) - centerOffset;
                    grid[x, y] = ground.transform;
                }
            }
        }

        
        /// <summary>
        /// Generating Border for Bullet and character movement
        /// </summary>
        /// <param name="edgeCollider2D"></param>
        private void GenerateBoundaryBox(GameObject edgeCollider2D)
        {
            Vector2 centerOffset = new Vector2(tileSize.x * gridSize.x / 2, tileSize.y * gridSize.y / 2);

            var edgeCollider =  _assetManager.Instantiate<EdgeCollider2D>("Boundary", Vector3.zero,quaternion.identity, null);

            float halfTileWidth = tileSize.x / 2;
            float halfTileHeight = tileSize.y / 2;

            Vector2[] points = new Vector2[5];

            points[0] = new Vector2(halfTileWidth, halfTileHeight) - centerOffset;
            points[1] = new Vector2(gridSize.x * tileSize.x - halfTileWidth, halfTileHeight) - centerOffset;
            points[2] = - points[0]; //opposite side
            points[3] = - points[1]; //opposite side
            points[4] = points[0]; // Turn first position
            
            edgeCollider.points = points;
        }
    }
}
