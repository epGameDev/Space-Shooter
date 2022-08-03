using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance {get; private set;}

    private AudioSource _audio;
    [SerializeField] private AudioClip _powerUpSFX;

    private void Awake() 
    {
        if (Instance)
        {
            Destroy(gameObject);
        }else{
            Instance = this;
        }
    }


    void Start()
    {
        _audio = this.gameObject.GetComponent<AudioSource>();
        _audio.Play();
    }

    public void PlayPowerUpSFX(AudioClip sfx)
    {
        _audio.PlayOneShot(sfx);
    }
}
