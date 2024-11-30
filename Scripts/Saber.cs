using UnityEngine;

namespace Assets.VRehab.Scripts
{
    public class Saber : MonoBehaviour
    {
        public LayerMask Layer;
        private Vector3 _previousPos;

        void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 1, Layer))
            {
                if (Vector3.Angle(transform.position - _previousPos, hit.transform.up) > 130)
                {
                    GameManager.Instance.IncrementHit();
                    Destroy(hit.transform.gameObject);
                }
            }
            _previousPos = transform.position;
        }
    }
}