using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EasyPartBossEffectGenerator : MonoBehaviour, IGenerator
{
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private GameObject rubble = null;
    [SerializeField] private GameObject bulletShootEffect = null;
    [SerializeField] private GameObject rubbleShootEffect = null;
    [SerializeField] private GameObject deathEffect = null;
    private IObjectContainer iobjectContainer = null;
    private GameObject generatedBullet = null;
    private List<GameObject> generatedAttackObjectList = new List<GameObject>();

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
        if (generateObject.activeSelf) 
        {
            iobjectContainer.RegistObject(generateObject);
            generatedAttackObjectList.Add(generateObject);
        }
    }

    //攻撃オブジェクトが呼ぶ。射撃オブジェクト生成。
    public void GenerateBullet(Vector3 generatePos, bool isFlip)
    {
        GameObject generated = Generate(bullet, generatePos, new Vector3(2, 2, 1), isFlip ? 180 : 0);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () =>
        {
            iobjectContainer.RegistObject(generated);
            generatedAttackObjectList.Add(generated);
        };
        icontainedObject.OnRemove += () =>
        {
            iobjectContainer.RemoveObject(generated);
            generatedAttackObjectList.Remove(generated);
        };
        InitRegist(iobjectContainer, generated);
        generatedBullet = generated;

        //弾のエフェクトスクリプトにコンテナを登録
        IGenerator igenerator = generated.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(iobjectContainer);
    }
    public void GenerateShootEffect(Vector3 generatePos, bool isFlip)
    {
        GameObject generated = Generate(bulletShootEffect, generatePos, new Vector3(3, 3, 1), isFlip ? 180 : 0);
        IContainedObject icontainedObject = generated.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generated); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generated); };
        InitRegist(iobjectContainer, generated);
    }

    //死亡時に生成した攻撃をすべて消す。
    public void AttackClear()
    {
        for(int i = 0; i < generatedAttackObjectList.Count; i++)
        {
            GameObject refObject = generatedAttackObjectList[0];
            if(refObject.GetComponent<IRemovableLinkByHitted>()?.hasLinkRemoved() == false)
            {
                generatedAttackObjectList.RemoveAt(0);
                Destroy(generatedBullet);
            }
        }
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
