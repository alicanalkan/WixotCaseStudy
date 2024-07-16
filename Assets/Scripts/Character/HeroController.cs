using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Wixot.Character
{
    public class HeroController : MonoBehaviour
    {
        public bool characterCanMove = false;
        public int speed;
        
        [SerializeField] private InputAction position,press,release;
        [SerializeField] private float swipeResistance = 100;
        [SerializeField] private int touchThresholdTime = 150;

        private Vector2 _initialPos; 
        private Vector2 CurrentPos => position.ReadValue<Vector2>();
        private CancellationTokenSource _cancellationTokenSource;
        private Rigidbody2D _rigidbody2D;
    
        void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            position.Enable();
            press.Enable();
            release.Enable();
            
            press.performed += pressed => DetectPress();
            press.canceled += canceled => DetectSwipe(canceled.duration);
            release.performed += released => DetectRelease();
        }

        void DetectPress()
        {
            
            _initialPos = CurrentPos;
            
            _cancellationTokenSource = new CancellationTokenSource();
            if(characterCanMove)
                MoveCharacter(_cancellationTokenSource.Token);
        }

        void DetectRelease()
        {
            _initialPos = CurrentPos;
            _cancellationTokenSource.Cancel();
        }
        void DetectSwipe(double duration)
        {
            if(duration * 1000 > touchThresholdTime || !characterCanMove)
                return;
            
            Vector2 delta = CurrentPos - _initialPos;
            Vector2 direction = Vector2.zero;
            
            //Hoirizontal swipe
            if(Mathf.Abs(delta.x) > swipeResistance)
            {
                direction.x = Mathf.Clamp(delta.x, -1, 1);

                if (direction.x >= 1)
                {
                    transform.Rotate(new Vector3(0, 0, -90));
                }

                if (direction.x <= -1)
                {
                    transform.Rotate(new Vector3(0, 0, 90));
                }
            }
            
            // Verticle swipe 
            // if(Mathf.Abs(delta.y) > swipeResistance)
            // {
            //     direction.y = Mathf.Clamp(delta.y, -1, 1);
            //     Debug.Log(direction);
            // }
        }

        private async void MoveCharacter(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(touchThresholdTime,token);
                    _rigidbody2D.velocity = transform.up * speed;
                }
            }
            catch (TaskCanceledException)
            {
                _rigidbody2D.velocity = Vector2.zero;
             
            }
        }
    }
}
