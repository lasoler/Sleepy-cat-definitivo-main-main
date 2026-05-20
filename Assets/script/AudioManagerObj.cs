using UnityEngine;

public class AudioManagerObj : MonoBehaviour
{
    public AudioClip bandaSonora;
    public AudioClip fxButton;
    
    // Arrastra tu efecto de sonido aquí desde el Inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    AudioSource _audioSource;

    void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.clip = bandaSonora;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
