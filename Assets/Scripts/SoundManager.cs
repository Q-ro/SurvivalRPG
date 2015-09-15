using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {

    public AudioSource sfxSource;                   // Manages the sound effects of the game such as enemy grunts
    public AudioSource musicSource;                 // Manages the background music of the game
    public static SoundManager instance = null;     // A static instance of our sound manager

    public float lowPitchRange = 0.95f;             // The lowest pitch to use for sound variation
    public float highPitchRange = 1.05f;            // The highest pitch to use for sound variation

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Plays a soundeffect
    /// </summary>
    /// <param name="clip"> the clip to be played</param>
    public void PlaySingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    /// <summary>
    /// Plays a random audioclip from a list of auidoclips
    /// </summary>
    /// <param name="clips">list of auidoclips</param>
    public void RandomSFX(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.clip = clips[randomIndex];


        sfxSource.Play();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
