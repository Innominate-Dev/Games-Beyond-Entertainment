using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] Music;
    private AudioSource audioSource;
    private int MusicNumber = 0;

    private int MusicLength;

    //private void Awake()
    //{
    //    DontDestroyOnLoad(this);
    //}

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MusicLength = Music.Length;
        if (!audioSource.isPlaying)
        {
            AudioClip nextClip;
            if (MusicNumber <= 4)
            {
                MusicNumber++;
                nextClip = GetNextClip();
                audioSource.clip = nextClip;
                audioSource.Play();

            }
            else
            {
                MusicNumber = 0;
                nextClip = RestartingPlaylist();
                audioSource.clip = nextClip;
                audioSource.Play();
            }

        }


    }
    private AudioClip GetNextClip()
    {
        return Music[(MusicNumber + 1) % Music.Length];
    }
    private AudioClip RestartingPlaylist()
    {
        return Music[(0) % Music.Length];
    }
}