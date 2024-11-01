using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }    
    [SerializeField] private AudioSource soundEffectSource;
    [SerializeField]  private AudioSource musicSource;

    [SerializeField] private AudioClip music;

    // These are all Audio clip used in the game
    [SerializeField] private AudioClip test;
    public AudioClip Test { get { return test; }}

    private void Awake()
    {
        Instance = this;
        soundEffectSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        musicSource.clip = music;
        musicSource.Play();

    }

    public void PlaySound(AudioClip sound)
    {
        soundEffectSource.PlayOneShot(sound);
    }
}
