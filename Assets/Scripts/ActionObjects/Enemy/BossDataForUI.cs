using UnityEngine;
using System;

public class BossDataForUI : MonoBehaviour
{
    public event Action<float> OnHpChanged;

    public void HpChanged(float rate) { OnHpChanged?.Invoke(rate); }
}
