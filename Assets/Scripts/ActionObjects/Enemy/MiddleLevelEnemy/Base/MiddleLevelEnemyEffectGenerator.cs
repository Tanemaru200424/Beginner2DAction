using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//砲台エネミーの破壊エフェクトと弾発射
public class MiddleLevelEnemyEffectGenerator : MonoBehaviour, IGenerator
{
    private IObjectContainer iobjectContainer = null;
    [SerializeField] private GameObject hittedEffect = null;
    [SerializeField] private GameObject deathEffect = null;
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private Vector2 bulletSize = new Vector2(2, 2);
    private GameObject generatedBullet = null;


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

    public void GenerateHittedEffect()
    {
        GameObject generatedEffect = Generate(hittedEffect, this.transform.position, new Vector3(2, 2, 1), 0);
        IContainedObject icontainedObject = generatedEffect.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedEffect); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedEffect); };
        InitRegist(iobjectContainer, generatedEffect);
    }
    public void GenerateDeathEffect()
    {
        GameObject generatedEffect = Generate(deathEffect, this.transform.position, new Vector3(2, 2, 1), 0);
        IContainedObject icontainedObject = generatedEffect.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedEffect); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedEffect); };
        InitRegist(iobjectContainer, generatedEffect);
    }
    public void GenerateBullet(Vector3 generatePos, float zAngle)
    {
        if (generatedBullet != null) { BulletClear(); }
        GameObject generated = Generate(bullet, generatePos, new Vector3(bulletSize.x, bulletSize.y, 1), zAngle);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () =>
        {
            iobjectContainer.RegistObject(generated);
            generatedBullet = generated;
        };
        icontainedObject.OnRemove += () =>
        {
            iobjectContainer.RemoveObject(generated);
            generatedBullet = null;
        };
        InitRegist(iobjectContainer, generated);
        generatedBullet = generated;

        //エフェクト生成スクリプト等にコンテナ登録。
        IGenerator igenerator = generatedBullet.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(iobjectContainer);
    }
    //死亡時に生成した弾をすべて消す。
    public void BulletClear()
    {
        if (generatedBullet != null)
        {
            if (generatedBullet.GetComponent<IRemovableLinkByHitted>()?.hasLinkRemoved() == false) { Destroy(generatedBullet); }
            generatedBullet = null;
        }
    }
}
