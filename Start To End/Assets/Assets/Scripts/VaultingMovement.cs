using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultingMovement : MonoBehaviour
{
    public event EventHandler<OnClimbEventArgs> OnClimb;
    public class OnClimbEventArgs
    {
        public AudioClip clip;
    }

    [SerializeField]
    private float forwordrayLenght;
    [SerializeField]
    private float rayOffset;

    [SerializeField]
    private float depth;

    [SerializeField]
    private float blendTime;

    [SerializeField]
    private LayerMask parkourLayer;

    [SerializeField]
    private float heightRayStartOffset;

    [SerializeField]
    private float heightRaylenght;

    [SerializeField]
    private float rotaionSmothing;
    [SerializeField]
    private List<ParkourActionSO> parkourActions;

    private PlayerMovement player;
    private AniamtionController aniamtionController;


    private HitData hitData;
    private void Start()
    {
        GetComponents();
        SetAnimIDs();
    }
    private void GetComponents()
    {
        player = GetComponent<PlayerMovement>();
        aniamtionController = GetComponent<AniamtionController>();
    }
    private void SetAnimIDs()
    {
        for (int i = 0; i < parkourActions.Count; i++)
        {
            parkourActions[i].SetAnimaID();
        }
    }
    private void EnvironmentInfo()
    {
        hitData.obstical = false;

        Ray forwordRay = new Ray(transform.position + (Vector3.up * rayOffset), transform.forward);
        RaycastHit forwordHitInfo;

        Debug.DrawRay(forwordRay.origin,forwordRay.direction * forwordrayLenght,new Color(0,1,0,0.2f));

        if (Physics.Raycast(forwordRay,out forwordHitInfo,forwordrayLenght,parkourLayer))
        {
            if(!aniamtionController.GetAction())
                hitData.obstical = true;

            hitData.forwordHit = forwordHitInfo;
            transform.forward = -hitData.forwordHit.normal;

            Ray heightRay = new Ray(forwordHitInfo.point + (transform.forward * depth) + (Vector3.up * heightRayStartOffset),
                Vector3.down);
            RaycastHit heightHitInfo;

            Debug.DrawRay(heightRay.origin, heightRay.direction * heightRaylenght,Color.red);

            if(Physics.Raycast(heightRay,out heightHitInfo, heightRaylenght, parkourLayer))
            {
                hitData.heightHit.point = new Vector3(forwordHitInfo.point.x, heightHitInfo.point.y,forwordHitInfo.point.z);

                hitData.objectHeight = heightHitInfo.point.y - transform.position.y;
                hitData.objectWidth = heightHitInfo.point.z - transform.position.z;
            }
        }
    }
    private void PlayCorrectAnimation()
    {
        if (hitData.obstical)
        {
            for (int i = 0; i < parkourActions.Count; i++)
            {

                if (hitData.objectHeight > parkourActions[i].minDistance && hitData.objectHeight < parkourActions[i].maxDistance)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && !aniamtionController.GetAction())
                    {
                        if (!string.IsNullOrEmpty(parkourActions[i].obsticalTag))
                        {
                            if (hitData.forwordHit.transform.tag == parkourActions[i].obsticalTag)
                            {
                                StartCoroutine(Parkour(parkourActions[i]));
                                break;
                            }
                        }
                        StartCoroutine(Parkour(parkourActions[i]));
                    }
                    break;
                }
            }
        }
    }
    private IEnumerator Parkour(ParkourActionSO action)
    {
        OnClimb?.Invoke(this, new OnClimbEventArgs
        {
            clip = action.clip
        });

        aniamtionController.CrossFadeAnimation(action.GetAnimID(), blendTime);
        yield return null;
        float timer = 0;
        while (timer <= aniamtionController.GetNextAnimLenght(action.animName))
        {
            MatchTargetWeightMask weight = new MatchTargetWeightMask(action.positionWeight, 0);
            aniamtionController.MatchTarget(hitData.heightHit.point, Quaternion.identity, weight, action.bodyPart,
                                            action.startTimeNormalized, action.targetTimeNormalized);
            timer += Time.deltaTime;
        }
    }
    private void CheckAction()
    {
        if (aniamtionController.GetAction())
        {
            aniamtionController.SetRootMotion(true);
            player.SetControl(false);
        }
        else
        {
            aniamtionController.SetRootMotion(false);
            player.SetControl(true);
        }
    }
    private void Update()
    {
        EnvironmentInfo();
        PlayCorrectAnimation();
        CheckAction();
    }
    private void OnDrawGizmos()
    {
        if(hitData.heightHit.point != null)
            Gizmos.DrawWireSphere(hitData.heightHit.point, 0.2f);
    }

    public struct HitData
    {
        public bool obstical;

        public RaycastHit forwordHit;
        public RaycastHit heightHit;

        public float objectHeight;
        public float objectWidth;
    }
}

