using UnityEngine;
using System;

public class PlayerDataForUI : MonoBehaviour
{
    public event Action<float> OnHpChanged;
    public event Action<float> OnChargeChanged;

    public void HpChanged(float rate) {  OnHpChanged?.Invoke(rate); }
    public void ChargeChanged(float rate) { OnChargeChanged?.Invoke(rate); }
}
