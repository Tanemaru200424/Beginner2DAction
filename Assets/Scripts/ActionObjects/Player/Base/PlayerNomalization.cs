using UnityEngine;

//レベル切り替え時、無敵やため状態をすべてリセットし、通常状態にする。
public class PlayerNomalization : MonoBehaviour
{
    private PlayerState state = null;
    private PlayerAttack attack = null;
    [SerializeField] private PlayerAnimation pAnimation = null;
    [SerializeField] private PlayerDamage damage = null;

    void Awake()
    {
        state = GetComponent<PlayerState>();
        attack = GetComponent<PlayerAttack>();
    }

    public void Nomalization()
    {
        state.Nomalization();
        pAnimation.SetStand();
        damage.IncredibleOff();
    }
}
