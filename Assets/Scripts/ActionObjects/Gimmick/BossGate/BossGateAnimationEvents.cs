using UnityEngine;

public class BossGateAnimationEvents : MonoBehaviour
{
    [SerializeField] private BossGate bossGate = null;

    public void CloseEnd()
    {
        bossGate.CloseEnd();
    }
}
