using UnityEngine;

namespace Assets.VRehab.Scripts
{
    public class Cube : MonoBehaviour
    {
        private float _speed = 2.5f;

        void Update()
        {
            transform.position += _speed * Time.deltaTime * transform.forward;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Wall")) return;
            GameManager.Instance.IncrementMissed();
            Destroy(gameObject);
        }
    }
}