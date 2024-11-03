using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Inspire by https://www.youtube.com/watch?v=rAX_r0yBwzQ&t=242s
    public static SoundManager Instance { get; private set; }       // Singleton

    public List<Sound> sounds  = new List<Sound>();     // All the sound used in the game
    private Dictionary<string, Sound> soundDictionary; // Dictionary of all the songs in the game

    [SerializeField] private AudioSource soundEffectSource;

    [SerializeField] private AudioSource musicSource; // We use another audio source to loop the background music
    [SerializeField] private AudioClip backgroundMusic; // Song used for background music

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep music playing when we reload a scene
            //soundEffectSource = GetComponent<AudioSource>();
            InitializeSoundDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void InitializeSoundDictionary()
    {
        soundDictionary = new Dictionary<string, Sound>();
        foreach(Sound sound in sounds)
        {
            soundDictionary[sound.name] = sound;
        }
    }

    // Play sound effect
    public void PlaySound(string songName)
    {
        
        if (soundDictionary.ContainsKey(songName))
        {
            Sound sound = soundDictionary[songName];
            soundEffectSource.PlayOneShot(sound.audioClip, sound.volume);
        }
    }

    // Play background music on repeat
    public void PlayBackgroundMusic()
    {
        if(musicSource != null &&  backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Class to keep data for each sounds
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audioClip;
        public float volume = 1f;
    }
}
