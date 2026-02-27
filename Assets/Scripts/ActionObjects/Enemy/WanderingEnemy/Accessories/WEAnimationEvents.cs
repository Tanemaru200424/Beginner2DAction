using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEAnimationEvents : MonoBehaviour
{
    [SerializeField] private WEController controller = null;
    [SerializeField] private WEEvents events = null;

    public void DamageEnd()
    {
        controller.DamageEnd();
    }

    public void DeathEnd()
    {
        events.DeathEnd();
    }
}
