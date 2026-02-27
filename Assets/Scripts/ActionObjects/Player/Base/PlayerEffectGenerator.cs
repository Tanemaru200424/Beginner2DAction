using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectGenerator : MonoBehaviour, IGenerator
{
    [SerializeField] private GameObject hitObject = null;
    [SerializeField] private GameObject chargeHitObject = null;
    private IObjectContainer iobjectContainer = null;

    public void SetObjectContainer(IObjectContainer iobjectContainer) { this.iobjectContainer = iobjectContainer; }
    public GameObject Generate(GameObject gameObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject generated = Instantiate(gameObject, generatePos, newRotation);
        generated.transform.localScale = generateScale;
        return generated;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf) { iobjectContainer.RegistObject(generateObject); }
    }

    public void GenerateNomalHitEffect(Vector3 generatePos)
    {
        GameObject generated = Generate(hitObject, generatePos, new Vector3(4, 4, 1), 0);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(generated);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(generated);
        InitRegist(iobjectContainer, generated);
    }

    public void GenerateChargeHitEffect(Vector3 generatePos)
    {
        GameObject generated = Generate(chargeHitObject, generatePos, new Vector3(4, 4, 1), 0);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(generated);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(generated);
        InitRegist(iobjectContainer, generated);
    }
}
