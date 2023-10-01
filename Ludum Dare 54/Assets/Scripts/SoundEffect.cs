using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Sound Effect")]
public class SoundEffect : ScriptableObject
{
    [Header("Clips")]
    [SerializeField] internal AudioClip[] Clips;

    [Header("Volume")]
    [SerializeField] internal float MinVolume = 0.5f;
    [SerializeField] internal float MaxVolume = 0.5f;

    [Header("Pitch")]
    [SerializeField] internal float MinPitch = 1f;
    [SerializeField] internal float MaxPitch = 1f;
	
    [Header("Other")]
    [SerializeField] internal bool Loop;
    [SerializeField] internal bool IsAffectedByTimeScale = true;

    public void Play()
    {
	    AudioHandler.Instance.PlaySoundEffect(this);
    }
}
