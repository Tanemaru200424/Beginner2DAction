using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitted : MonoBehaviour, IHittable
{
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private BulletController controller = null;
    [SerializeField] private BulletEffectsGenerator effectsGenerator = null;
    private bool isHitted = false;
    private bool isPause = false;

    public void PauseSwitch(bool ispause) { isPause = ispause; }
    public bool CanHitted() { return !isHitted && !isPause; }
    public void Hitted(float angle)
    {
        if (!isPause && !isHitted)
        {
            isHitted = true;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            bullet.transform.rotation = rotation;
            controller.HittedStart();
            effectsGenerator.GenerateHittedEffect();
        }
    }
}
