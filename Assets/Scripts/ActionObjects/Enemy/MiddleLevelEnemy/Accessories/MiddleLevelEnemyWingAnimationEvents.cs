using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEBaseAnimationEvents : MonoBehaviour
{
    [SerializeField] private SEEvents events = null;

    public void DeathEnd()
    {
        events.DeathEnd();
    }
}
