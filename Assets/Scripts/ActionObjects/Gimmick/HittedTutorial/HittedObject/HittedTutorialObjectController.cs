using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�`���[�g���A���Ő�����΂��p�̃I�u�W�F�N�g�B
public class HittedTutorialObjectController : MonoBehaviour, IHittable, IPausable
{
    [SerializeField] private HittedTutorialObjectHittedAttack attack = null;
    [SerializeField] private AccessoriesLoopEffect attentionMark = null;
    private HittedTutorialObjectEffectGenerator effectGenerator = null;

    [SerializeField] private Animator animator = null;
    [SerializeField] private float hittedSpeed = 0;
    [SerializeField] private float fallSpeed = 0;
    [SerializeField] private float hittedDistance = 0;
    [SerializeField] private GroundChecker groundChecker = null;

    private Rigidbody2D rb2D = null;
    private Collider2D col2D = null;
    private bool isHitted = false;
    private bool isEffectOn = false; //エフェクト表示のフラグ。
    private Vector2 hittedVector = new Vector2(0, 0);
    private Vector2 hittedStartPos = new Vector2(0, 0);
    private bool isPause = false;

    void Awake()
    {
        effectGenerator = GetComponent<HittedTutorialObjectEffectGenerator>();
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        rb2D.linearVelocity = new Vector2(0, 0);
        attentionMark.Init();
        isEffectOn = false;
    }

    void FixedUpdate()
    {
        if (isHitted)
        {
            rb2D.linearVelocity = hittedVector * hittedSpeed;
            if(Vector2.Distance(this.transform.position, hittedStartPos) > hittedDistance || attack.IsHitSomething()) 
            { 
                Destroy(this.gameObject); 
            }
        }
        else
        {
            if (groundChecker.IsGround())
            {
                rb2D.linearVelocity = new Vector2(0, 0);
            }
            else
            {
                rb2D.linearVelocity = new Vector2(0, -fallSpeed);
            }
        }

        if (CanHitted() && !isEffectOn)
        {
            attentionMark.EffectSwitch(true);
            isEffectOn = true;
        }
        else if(!CanHitted() && isEffectOn)
        {
            attentionMark.EffectSwitch(false);
            isEffectOn = false;
        }
    }

    public bool CanHitted() { return !isHitted && !isPause && groundChecker.IsGround(); }
    public void Hitted(float angle)
    {
        if (!isHitted && !isPause)
        {
            isHitted = true;
            col2D.enabled = false;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            hittedVector = rotation * Vector2.right.normalized;
            hittedStartPos = this.transform.position;
            animator.SetTrigger("hitted");
            attack.AttackSwitch(true);
            effectGenerator.GenerateHittedEffect();
            attentionMark.EffectSwitch(false);
            isEffectOn = false;

            //������ъJ�n�������ɂ���Ĕ��]�B
            Vector3 currentScale = this.transform.localScale;
            if (currentScale.x * hittedVector.x > 0)
            {
                this.transform.localScale = Vector3.Scale(currentScale, new Vector3(-1, 1, 1));
            }
        }
    }

    public void Paused()
    {
        isPause = true;
        this.enabled = false;
        rb2D.Sleep();
        animator.speed = 0;
        attack.PauseSwitch(true);
        attentionMark.PauseSwitch(true);
    }
    public void Resumed()
    {
        isPause = false;
        this.enabled = true;
        rb2D.WakeUp();
        animator.speed = 1;
        attack.PauseSwitch(false);
        attentionMark.PauseSwitch(false);
    }
}
