using UnityEngine;

public class LowLevelEnemyEffectGenerator : MonoBehaviour, IGenerator
{
    [SerializeField] private GameObject hittedEffect = null;
    [SerializeField] private GameObject deathEffect = null;
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

    //ダメージスクリプトが呼ぶ。ダウン（ノックバック）時に呼ぶ。
    public void GenerateHittedEffect()
    {
        GameObject generated = Generate(hittedEffect, this.transform.position, new Vector3(5, 5, 1), 0);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(generated);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(generated);
        InitRegist(iobjectContainer, generated);
    }

    //死亡開始時にイベントスクリプトが呼ぶ。
    public void GenerateDeathEffect()
    {
        GameObject generated = Generate(deathEffect, this.transform.position, new Vector3(5, 5, 1), 0);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(generated);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(generated);
        InitRegist(iobjectContainer, generated);
    }
}
