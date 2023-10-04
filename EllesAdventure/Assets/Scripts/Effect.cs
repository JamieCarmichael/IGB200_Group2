using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Plays a audio clip and particle system.
/// </summary>
public class Effect : MonoBehaviour 
{
    #region Fields
    [Serializable]
    public struct Sound
    {
        [Tooltip("Audio clip to play.")]
        public AudioClip clip;
        [Tooltip("1 is full volume 0 is no volume.")]
        [Range(0, 1)]
        public float volume;
    }

    [SerializeField] private Sound[] audioSounds;
    [SerializeField] private ParticleSystem particleEffect;

    private AudioSource audioSource;

    private AudioSource AudioSource
    {
        get 
        { 
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            return audioSource; 
        }

        set
        {
            AudioSource = value;
        }
    }
    #endregion


    #region Public Methods
    public void PlayEffect()
    {
        if (AudioSource != null && audioSounds.Length > 0)
        {
            int soundIndex = Random.Range(0, audioSounds.Length);
            AudioSource.PlayOneShot(audioSounds[soundIndex].clip, audioSounds[soundIndex].volume);
        }
        if (particleEffect != null)
        {
            particleEffect.Play();
        }
    }
    #endregion
}