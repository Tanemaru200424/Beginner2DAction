using UnityEngine;
using System.Collections;

public class BossGate : MonoBehaviour
{
    [SerializeField] private Animator animator = null; //門のアニメーション
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    private BoxCollider2D boxCollider2D = null;
    private bool isClose = false;

    void Awake()
    {
        isClose = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            //起動により当たり判定だけ出る。見た目は演出挟んでから出るようにしたい。
            animator.enabled = false;
            spriteRenderer.enabled = false;
            boxCollider2D.enabled = true;
        };
        iareaObject.OnDeactive += () =>
        {
            animator.enabled = false;
            boxCollider2D.enabled = false;
        };
    }

    void OnEnable()
    {
        animator.enabled = false;
        spriteRenderer.enabled = false;
    }

    //ボスイベントに呼んでもらう。
    public void CloseStart()
    {
        isClose = false;
        animator.enabled = true;
        spriteRenderer.enabled = true;
        animator.Play("Close");
    }
    //アニメーションイベントに呼んでもらう。
    public void CloseEnd() { isClose = true; }

    //ボスイベント側で扉が閉まったことを確認するため。
    public bool IsClose() {  return isClose; }
}