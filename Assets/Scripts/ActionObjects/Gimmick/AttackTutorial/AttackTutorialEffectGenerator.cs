using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorialEffectGenerator : MonoBehaviour, IGenerator
{
    [SerializeField] private IObjectContainer iobjectContainer = null;
    [SerializeField] private GameObject originEffect = null;

    public void SetObjectContainer(IObjectContainer iobjectContainer)
    {
        this.iobjectContainer = iobjectContainer;
    }
    public GameObject Generate(GameObject generateObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject generated = Instantiate(generateObject, generatePos, newRotation);
        generated.transform.localScale = generateScale;
        return generated;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf)
        {
            iobjectContainer.RegistObject(generateObject);
        }
    }

    public void GenerateDestroyEffect()
    {
        GameObject generatedEffect = Generate(originEffect, this.transform.position, new Vector3(8, 8, 1), 0);
        IContainedObject icontainedObject = generatedEffect.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedEffect); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedEffect); };
        InitRegist(iobjectContainer, generatedEffect);
    }
}