using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb2D = null;
    private Collider2D c2D = null; //�{�̂̍U������
    [SerializeField] private Collider2D hitboxC2D = null;
    [SerializeField] private Collider2D hittedAttackC2D = null;

    [SerializeField] private float speed = 0;
    [SerializeField] private float distance = 0;
    [SerializeField] private float hittedDistance = 0;
    private Vector3 startPos = new Vector3 (0, 0, 0);
    private Vector3 hittedPos = new Vector3 (0, 0, 0);

    private bool isHitted = false;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
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
        hittedPos = this.transform.position;
        c2D.enabled = false;
        hitboxC2D.enabled = false;
        hittedAttackC2D.enabled = true;
    }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
    }
}
