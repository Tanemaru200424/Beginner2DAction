using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossEffectGenerator : MonoBehaviour, IGenerator
{
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private GameObject shootEffect = null;
    [SerializeField] private GameObject downEffect = null;
    [SerializeField] private GameObject deathEffect = null;
    private IObjectContainer iobjectContainer = null;
    private GameObject generatedBullet = null;

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

    //攻撃オブジェクトが呼ぶ。射撃オブジェクト生成。
    public void GenerateBullet(Vector3 generatePos, bool isFlip)
    {
        if(generatedBullet != null) { BulletClear(); }
        GameObject generated = Generate(bullet, generatePos, new Vector3(2, 2, 1), isFlip ? 180 : 0);
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

        //弾のエフェクトスクリプトにコンテナを登録
        IGenerator igenerator = generated.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(iobjectContainer);
    }
    public void GenerateShootEffect(Vector3 generatePos, bool isFlip)
    {
        GameObject generated = Generate(shootEffect, generatePos, new Vector3(3, 3, 1), isFlip ? 180 : 0);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generated); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generated); };
        InitRegist(iobjectContainer, generated);
    }
    //死亡時に生成した弾をすべて消す。
    public void BulletClear()
    {
        if (generatedBullet != null)
        {
            Destroy(generatedBullet);
        }
    }

    //ダメージスクリプトが呼ぶ。ダウン（ノックバック）時に呼ぶ。
    public void GenerateDownEffect()
    {
        GameObject generated = Generate(downEffect, this.transform.position, new Vector3(5, 5, 1), 0);
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
