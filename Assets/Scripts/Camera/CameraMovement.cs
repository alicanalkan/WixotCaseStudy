using UnityEngine;

namespace Wixot.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        private readonly float _smoothSpeed = 0.15f;
        private readonly Vector3 _offset = new Vector3(0,0,-10);
        public Transform _transform;
        

        private void FixedUpdate()
        {
            Vector3 desiredPosition = _transform.position + _offset;

         
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
            
            transform.position = smoothedPosition;

          
            Quaternion desiredRotation = _transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, _smoothSpeed);
        }
    }
}
