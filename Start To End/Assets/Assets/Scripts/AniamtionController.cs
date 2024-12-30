using System.Linq.Expressions;
using UnityEngine;

public class AniamtionController : MonoBehaviour
{
    [SerializeField]
    private float blendTime;
    [SerializeField]
    private float fallOutTime;

    private Animator animator;

    private float _speed;

    private Vector2 _inputDirection;
    private PlayerMovement player;
    private bool _run;

    private float _fallOutTimer;

    private bool inAction;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Inputs();
        MovementAnimator();
        FallAnimator();
        SetAction();
    }
    private void FallAnimator()
    {
        if (player.Grounded)
        {
            animator.SetBool("Fall", false);
        }
        else
        {
            _fallOutTimer += Time.deltaTime;
            if (_fallOutTimer > fallOutTime)
            {
                animator.SetBool("Fall", true);
                _fallOutTimer = 0;
            }
        }
    }
    private void MovementAnimator()
    {
        _speed = _inputDirection != Vector2.zero ? (_run ? 1 : 0.2f) : 0;
        animator.SetFloat("Speed", _speed, blendTime, Time.deltaTime);
    }
    private void Inputs()
    {
        _inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _run = Input.GetKey(KeyCode.LeftShift);
    }
    private void SetAction()
    {
        bool animationIsAction = animator.GetNextAnimatorStateInfo(0).IsTag("Action") ||
                                 animator.GetCurrentAnimatorStateInfo(0).IsTag("Action");
        if (animationIsAction)
        {
            inAction = true;
        }
        if (!animationIsAction)
        {
            inAction = false;
        }
    }
    public bool GetAction()
    {
        return inAction;
    }
    public void CrossFadeAnimation(int animID , float blendTime )
    {
        animator.CrossFade(animID, blendTime * Time.deltaTime);
    }
    public bool GetCurrentAnimation(string animName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    public float GetNextAnimLenght(string AnimName)
    {
        return animator.GetNextAnimatorStateInfo(0).length;
    }
    public void MatchTarget(Vector3 matchPos,Quaternion matchRot,MatchTargetWeightMask weight,AvatarTarget target, 
        float startTime,float targetTime)
    {
        if(animator.isMatchingTarget) return;
        animator.MatchTarget(matchPos, matchRot,target,weight, startTime, targetTime);
    }
    public void SetRootMotion(bool applyRootMotion)
    {
        animator.applyRootMotion = applyRootMotion;
    }
}

