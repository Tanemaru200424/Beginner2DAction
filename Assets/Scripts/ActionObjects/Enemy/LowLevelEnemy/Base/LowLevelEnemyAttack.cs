using UnityEngine;

public class LowLevelEnemyAttack : MonoBehaviour
{
    [SerializeField] private Collider2D bodyAttack2D = null;
    [SerializeField] private Collider2D hittedAttack2D = null;

    private void Awake()
    {
        BodyAttackSwitch(true);
        HittedAttackSwitch(false);
    }

    //体の攻撃判定オンオフ。ダメージスクリプトとイベントスクリプトが使う。
    public void BodyAttackSwitch(bool isactive) { bodyAttack2D.enabled = isactive; }
    public void HittedAttackSwitch(bool isactive) { hittedAttack2D.enabled = isactive; }
}
