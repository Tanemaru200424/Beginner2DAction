using UnityEngine;

//アクションオブジェクトのうちエリア外に出たものの破棄を行う。
public class AreaOverBorder : MonoBehaviour
{
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IAreaOverExtinctionable areaOverExtinctionable = collision.gameObject.GetComponent<IAreaOverExtinctionable>();
        if (areaOverExtinctionable != null && areaOverExtinctionable.CanExtinction())
        {
            areaOverExtinctionable.Extinction();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IAreaOverExtinctionable areaOverExtinctionable = collision.gameObject.GetComponent<IAreaOverExtinctionable>();
        if (areaOverExtinctionable != null && areaOverExtinctionable.CanExtinction())
        {
            areaOverExtinctionable.Extinction();
        }
    }
}
