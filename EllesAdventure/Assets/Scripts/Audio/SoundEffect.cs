using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Aa audio source for playing sound effects.
/// </summary>
public class SoundEffect : MonoBehaviour
{
    /// <summary>
    /// A audio clip with a volume and pitch range. 
    /// </summary>
    [Serializable]
    public struct SoundEffectClip
    {
        public AudioClip clip;
        public float volume;
        public Vector2 pitch;
    }

    #region Fields
    /// <summary>
    /// The audio source on this object.
    /// </summary>
    private AudioSource thisAudioSource;

    /// <summary>
    /// The audio source on this object. 
    /// </summary>
    private AudioSource ThisAudioSource
    {
        get
        {
            if (thisAudioSource == null)
            {
                thisAudioSource = GetComponent<AudioSource>();
            }
            return thisAudioSource;
        }
    }
    #endregion


    #region Public Methods
    /// <summary>
    /// Play the sound clip from the sound effect at the volume and between the pitch values.
    /// </summary>
    /// <param name="soundEffectClip"></param>
    public void PlayAudio(SoundEffectClip soundEffectClip)
    {
        ThisAudioSource.pitch = Random.Range(soundEffectClip.pitch.x, soundEffectClip.pitch.y);
        ThisAudioSource.PlayOneShot(soundEffectClip.clip, soundEffectClip.volume);

        Invoke("ReturnToPool", soundEffectClip.clip.length);
    }
    #endregion
}