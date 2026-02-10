using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource jetSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource applauseSource;

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    public static SoundManager instance;

    private bool jetSoundPlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        musicSource.clip = audioClips[3];
        musicSource.Play();
    }

    public void PlayJumpSound()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    public void StartJetSound()
    {
        if (!jetSoundPlaying)
        {
            jetSoundPlaying = true;

            jetSource.clip = audioClips[1];
            jetSource.Play();
        }
    }

    public void EndJetSound()
    {
        jetSoundPlaying = false;

        jetSource.clip = audioClips[1];
        jetSource.Stop();
    }

    public void PlayBonkSound()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    public void PlayApplause()
    {
        applauseSource.clip = audioClips[4];
        applauseSource.Play();
    }
}
