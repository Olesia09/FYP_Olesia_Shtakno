using UnityEngine;

namespace Assets.VRehab.Scripts
{
    public class Spawner : MonoBehaviour
    {
        public GameObject[] Cubes;
        public Transform[] Points;
        public float Speed;

        private float _beatInterval;
        private float _timer;
        private bool _songStarted;
        private bool _songFinished;
        private float _delay;

        private void Start()
        {
            _beatInterval = (60f / GameManager.Instance.BPM * Speed); // Calculate interval based on BPM
            _delay = GameManager.Instance.GetDelay();
            _songStarted = false;
            _songFinished = false;

            // Start checking for song finish after the delay
            Invoke(nameof(StartSongFinishCheck), _delay);
        }

        private void Update()
        {
            if (_songFinished) return; // Stop spawning if the song is finished

            _timer += Time.deltaTime;

            if (_timer >= _beatInterval)
            {
                SpawnCube();
                _timer -= _beatInterval;
            }

            // Only check if the song has finished playing after the delay has passed
            if (_songStarted)
            {
                CheckIfSongFinished();
            }
        }

        private void SpawnCube()
        {
            var cube = Instantiate(Cubes[Random.Range(0, Cubes.Length)], Points[Random.Range(0, Points.Length)]);
            cube.transform.localPosition = Vector3.zero;
            cube.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));
            GameManager.Instance.IncrementActiveCubes();
        }

        private void StartSongFinishCheck()
        {
            _songStarted = true; // Enable song finish checking after the delay
        }

        private void CheckIfSongFinished()
        {
            if (GameManager.Instance.AudioSource.isPlaying) return;
            _songFinished = true;
        }
    }
}