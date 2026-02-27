using UnityEngine;

//ゲームを停止せずにエリアを切り替える。ステージスクロールの機能を持つ。
public class AreaChangeTrigger : MonoBehaviour
{
    [SerializeField] private AreaChangeEvent areaChangeEvent = null;
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
        areaChangeEvent.AreaChangeTrigger();
    }
}
