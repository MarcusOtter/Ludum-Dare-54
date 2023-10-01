using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHandler : MonoBehaviour
{
    public static AudioHandler Instance;
    
    [SerializeField] private AudioSource musicSource;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.pitch = Time.timeScale;
    }
    
    public void PlaySoundEffect(SoundEffect sound)
    {
        var volume = Random.Range(sound.MinVolume, sound.MaxVolume);
        // var pitch = Random.Range(sound.MinPitch, sound.MaxPitch);
        var clip = sound.Clips[Random.Range(0, sound.Clips.Length)];
        
        _audioSource.PlayOneShot(clip, volume);
    }
}
