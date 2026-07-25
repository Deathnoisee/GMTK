using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        private static SoundManager instance = null;
        private AudioSource audioSource;

        private SoundType? currentMusic = null;


        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }
        }
        private void Update()
        {
            if (currentMusic != null && !audioSource.isPlaying)
            {
                PlayMusic(currentMusic.Value, audioSource);
            }
        }

        private void Start()
        {
            //StartCoroutine(StartMusic());
        }

        IEnumerator StartMusic()
        {
            yield return new WaitForSeconds(0.1f);
            PlayMusic(SoundType.Music, audioSource, 0.2f); // Start with a lower volume
        }
        private void PlayMusic(SoundType sound, AudioSource source, float volume = 1)
        {
            currentMusic = sound;
            SoundList soundList = SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            source.outputAudioMixerGroup = soundList.mixer;
            source.clip = randomClip;
            source.volume = volume * soundList.volume;
            source.loop = false; // 🔁 No looping
            source.Play();
        }
        public static void StopMusic()
        {
            if (instance != null && instance.audioSource != null && instance.audioSource.isPlaying)
            {
                instance.audioSource.Stop();
                instance.currentMusic = null;
            }
        }


        public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            if (source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.clip = randomClip;
                source.volume = volume * soundList.volume;
                source.Play();
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
        }
    }



    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}