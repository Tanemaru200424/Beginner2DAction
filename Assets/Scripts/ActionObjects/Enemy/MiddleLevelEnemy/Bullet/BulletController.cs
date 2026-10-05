using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IRemovableLinkByHitted
{
    private Rigidbody2D rb2D = null;
    private Animator animator = null;
    private Collider2D c2D = null; //�{�̂̍U������
    [SerializeField] private Collider2D hitboxC2D = null;
    [SerializeField] private Collider2D hittedAttackC2D = null;

    [SerializeField] private float speed = 0;
    [SerializeField] private float distance = 0;
    [SerializeField] private float hittedDistance = 0;
    private Vector3 startPos = new Vector3 (0, 0, 0);
    private Vector3 hittedPos = new Vector3 (0, 0, 0);

    private bool isHitted = false;
    private bool isLinkRemoved = false;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        c2D = GetComponent<Collider2D>();
        hittedAttackC2D.enabled = false;
        startPos = this.transform.position;
    }

    void Update()
    {
        if ((!isHitted && Vector2.Distance(this.transform.position, startPos) > distance) ||
            (isHitted && (Vector2.Distance(this.transform.position, hittedPos) > hittedDistance)))
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        rb2D.linearVelocity = transform.right * speed;
    }

    public void HittedStart()
    {
        isHitted = true;
        RemoveLinkByHitted();
        hittedPos = this.transform.position;
        c2D.enabled = false;
        hitboxC2D.enabled = false;
        hittedAttackC2D.enabled = true;
    }
    public bool hasLinkRemoved() { return isLinkRemoved; }
    public void RemoveLinkByHitted() { isLinkRemoved = true; }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        if (ispause) { rb2D.Sleep(); }
        else { rb2D.WakeUp(); }
        animator.speed = ispause ? 0 : 1;
    }
}
