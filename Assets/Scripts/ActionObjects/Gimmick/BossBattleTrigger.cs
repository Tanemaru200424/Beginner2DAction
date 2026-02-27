using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    private BoxCollider2D boxCollider2D = null;
    [SerializeField] private BossBattleEvents bossBattleEvents = null;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            boxCollider2D.enabled = true;
        };
        iareaObject.OnDeactive += () =>
        {
            boxCollider2D.enabled = false;
        };
    }

    //プレイヤーについているイベントセンサーとだけ引っ掛かる。
    void OnTriggerStay2D(Collider2D playerCol)
    {
        boxCollider2D.enabled = false;
        bossBattleEvents.BattleStartTrigger();
    }
}
