using UnityEngine;

//特定範囲にプレイヤーが入ったら敵群を生成するトリガー。
public class EnemyGenerateTrigger : MonoBehaviour
{
    private BoxCollider2D boxCollider2D = null;
    private EnemyGenerator enemyGenerator = null;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyGenerator = GetComponent<EnemyGenerator>();
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
    void OnTriggerEnter2D(Collider2D playerCol)
    {
        boxCollider2D.enabled = false;
        enemyGenerator.GenerateEnemy();
    }
}
