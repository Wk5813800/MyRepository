using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;
    public AudioSource audioSource;

    // This is where you'll assign the voice tone or other audio clips in the inspector
    public AudioClip voiceTone;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy the new instance if one already exists
        }

        // Get the AudioSource component
        // audioSource = GetComponent<AudioSource>();

        // PlayVoiceTone();
    }
    void Start()
    {
       
    }

    // Method to play the voice tone
    public void PlayVoiceTone()
    {
        if (audioSource != null && voiceTone != null)
        {
            audioSource.clip = voiceTone;
            audioSource.Play();
        }
    }

    // Method to stop the voice tone
    public void StopVoiceTone()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
