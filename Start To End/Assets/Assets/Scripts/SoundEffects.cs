using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] footstepSounds;

    private VaultingMovement valutingMovement;

    private int index = 0;
    private void Awake()
    {
        valutingMovement = GetComponent<VaultingMovement>();
    }
    private void Start()
    {
        valutingMovement.OnClimb += ValutingMovement_OnClimb;
    }

    private void ValutingMovement_OnClimb(object sender, VaultingMovement.OnClimbEventArgs e)
    {
        PlayClipAtPoint(e.clip, transform.position,1);
    }

    private void PlayClipAtPoint(AudioClip clip,Vector3 position,float volume = 0)
    {
        AudioSource.PlayClipAtPoint(clip,position,volume);
    }

    private void OnStep(float volume)
    {
        if (index >= footstepSounds.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        PlayClipAtPoint(footstepSounds[index],transform.position,volume);
    }
}
