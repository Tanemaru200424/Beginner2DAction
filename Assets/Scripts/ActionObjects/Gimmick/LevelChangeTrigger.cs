using UnityEngine;

public class LevelChangeTrigger : MonoBehaviour
{
    [SerializeField] private LevelChangeEvent levelChangeEvent = null;
    private BoxCollider2D boxCollider2D = null;

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
        levelChangeEvent.LevelChangeTrigger();
    }
}
