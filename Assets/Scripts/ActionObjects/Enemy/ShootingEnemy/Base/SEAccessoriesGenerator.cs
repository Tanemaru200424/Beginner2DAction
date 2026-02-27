using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//砲台エネミーの破壊エフェクトと弾発射
public class SEAccessoriesGenerator : MonoBehaviour, IGenerator
{
    private IObjectContainer iobjectContainer = null;
    [SerializeField] private GameObject hitEffect = null;
    [SerializeField] private GameObject retireEffect = null;
    [SerializeField] private GameObject bullet = null;

    public void GenerateHitEffect(Vector3 hitPos)
    {
        GameObject generatedEffect = Generate(hitEffect, hitPos, new Vector3(2, 2, 1), 0);
        IContainedObject icontainedObject = generatedEffect.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedEffect); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedEffect); };
        InitRegist(iobjectContainer, generatedEffect);
    }
    public void GenerateRetireEffect()
    {
        GameObject generatedEffect = Generate(retireEffect, this.transform.position, new Vector3(2, 2, 1), 0);
        IContainedObject icontainedObject = generatedEffect.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedEffect); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedEffect); };
        InitRegist(iobjectContainer, generatedEffect);
    }
    public void GenerateBullet(Vector3 generatePos, float zAngle)
    {
        GameObject generatedBullet = Generate(bullet, generatePos, new Vector3(1, 1, 1), zAngle);
        IContainedObject icontainedObject = generatedBullet.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedBullet); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedBullet); };
        InitRegist(iobjectContainer, generatedBullet);

        //エフェクト生成スクリプト等にコンテナ登録。
        IGenerator igenerator = generatedBullet.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(iobjectContainer);
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
