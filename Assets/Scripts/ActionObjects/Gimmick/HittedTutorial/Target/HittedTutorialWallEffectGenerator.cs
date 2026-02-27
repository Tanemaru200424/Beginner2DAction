using UnityEngine;

public class HittedTutorialWallEffectGenerator : MonoBehaviour, IGenerator
{
    private IObjectContainer iobjectContainer = null;
    [SerializeField] private GameObject targetDestroyEffect = null;
    [SerializeField] private GameObject destroyEffect = null;

    public void GenerateTargetDestroyEffect(Vector3 hitPos)
    {
        GameObject generatedEffect = Generate(targetDestroyEffect, hitPos, new Vector3(2, 2, 1), 0);
        IContainedObject icontainedObject = generatedEffect.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedEffect); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedEffect); };
        InitRegist(iobjectContainer, generatedEffect);
    }
    public void GenerateDestroyEffect()
    {
        GameObject generatedEffect = Generate(destroyEffect, this.transform.position, new Vector3(10, 10, 1), 0);
        IContainedObject icontainedObject = generatedEffect.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedEffect); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedEffect); };
        InitRegist(iobjectContainer, generatedEffect);
    }
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
}
