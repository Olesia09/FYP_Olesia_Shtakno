using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.VRehab.Scripts
{
    public class VolumeManager : MonoBehaviour
    {
        public AudioMixer AudioMixer; 
        public Slider VolumeSlider;  

        private const string VolumeKey = "MasterVolume";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            
            var savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            VolumeSlider.value = savedVolume;
            SetMasterVolume(savedVolume);

            // Add a listener to handle slider changes
            VolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        public void SetMasterVolume(float volume)
        {
            // Convert slider value to decibels
            var db = Mathf.Lerp(-80f, 0f, volume);
            AudioMixer.SetFloat("MasterVolume", db);

            PlayerPrefs.SetFloat(VolumeKey, volume);
            PlayerPrefs.Save();
        }
    }
}