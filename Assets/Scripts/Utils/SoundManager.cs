using UnityEngine;
using UnityEngine.Audio;

namespace Europa.Utils
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioClip[] musicClips;
        private AudioSource musicSource;

        private int clip;

        private void Start()
        {
            musicSource = GetComponent<AudioSource>();

            SetMasterVolume(PlayerPrefs.GetFloat("masterVolume"));
            SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
            SetEffectsVolume(PlayerPrefs.GetFloat("effectsVolume"));

            clip = Random.Range(0, musicClips.Length - 1);
            PlayMusic();
        }

        private void PlayMusic()
        {
            musicSource.clip = musicClips[clip];
            musicSource.Play();
            Invoke(nameof(PlayMusic), musicClips[clip].length);
            if (clip + 1 < musicClips.Length) clip++;
            else clip = 0;
        }

        public void SetMasterVolume(float volume)
        {
            if (volume == 0) volume = 0.01f;
            volume = Mathf.Log10(volume / 100) * 20;
            audioMixer.SetFloat("master", volume);
        }

        public void SetMusicVolume(float volume)
        {
            if (volume == 0) volume = 0.01f;
            volume = Mathf.Log10(volume / 100) * 20;
            audioMixer.SetFloat("music", volume);
        }

        public void SetEffectsVolume(float volume)
        {
            if (volume == 0) volume = 0.01f;
            volume = Mathf.Log10(volume / 100) * 20;
            audioMixer.SetFloat("effects", volume);
        }
    }
}