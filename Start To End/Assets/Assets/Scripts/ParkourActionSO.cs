using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PrkourAction",menuName = "Scriptable/ParkourAction")]
public class ParkourActionSO : ScriptableObject
{
    public string animName;
    public float minDistance;
    public float maxDistance;

    public AudioClip clip;

    public float waitBeforeRelease;

    public string obsticalTag;

    [Space(10)]
    [Header("Target Matching")]
    public AvatarTarget bodyPart;
    public Vector3 positionWeight;
    public float startTimeNormalized;
    public float targetTimeNormalized;

    private int animID;

    public void SetAnimaID()
    {
        animID = Animator.StringToHash(animName);
    }
    public int GetAnimID()
    {
        return animID;
    }
}
