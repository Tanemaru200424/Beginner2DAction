using UnityEngine;

public class EasyPartBossAnimationEvents : MonoBehaviour
{
    [SerializeField] private EasyPartBossState state = null;
    //[SerializeField] private TutorialBossEvents events = null;
    [SerializeField] private EasyPartBossAttack attack = null;
    //[SerializeField] private TutorialBossMove move = null;
    //[SerializeField] private TutorialBossDamage damage = null;

    //アニメーションイベントで呼ぶ。
    public void Shoot()
    {
        attack.Shoot();
    }

    //ステートマシンビヘイビアで呼ぶ。
    public void BirthEnd()
    {
        //events.BirthEnd();
    }
    public void DeathEnd()
    {
        //events.DeathEnd();
    }
    //射撃を一定回数ループした後に呼ばれる。
    public void ShootEnd()
    {
        attack.ShootEnd();
    }
}
