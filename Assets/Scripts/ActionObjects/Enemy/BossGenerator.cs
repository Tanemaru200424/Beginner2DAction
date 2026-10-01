using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//プレイヤーが入ったら実行
//ボスを生成し登場させるイベントを実行させる。
public class BossGenerator : MonoBehaviour, IGenerator, IObjectContainer
{
    [SerializeField] private GameObject originBoss = null;
    [SerializeField] private Transform bossGenerateTrans = null;
    private GameObject generatedBoss = null;

    private IObjectContainer iobjectContainer = null;

    public bool isEntry { get; private set; } = false;
    public bool isRetire { get; private set; } = false;
    public event Action OnBossDeath;

    [SerializeField] private ActionUIController actionUIController = null;
    //[SerializeField] private AimPlayerManager aimPlayerManager = null;

    public void SetObjectContainer(IObjectContainer iobjectContainer) { this.iobjectContainer = iobjectContainer; }
    public GameObject Generate(GameObject gameObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject boss = Instantiate(gameObject, generatePos, newRotation);
        boss.transform.localScale = generateScale;
        return boss;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf) 
        { 
            iobjectContainer.RegistObject(generateObject);
        }
    }

    public void RegistObject(GameObject obj)
    {
        if (obj != null && generatedBoss == null) { generatedBoss = obj; }
    }
    public void RemoveObject(GameObject obj)
    {
        if (generatedBoss == obj) { generatedBoss = null; }
    }

    /*
    private void BossStartrFlip(bool isFlip) //生成時の向きを指定する。
    {
        if (generatedBoss != null)
        {
            if ((generatedBoss.transform.localScale.x > 0 && isFlip) ||
                (generatedBoss.transform.localScale.x < 0 && !isFlip))
            {
                generatedBoss.transform.localScale = Vector3.Scale(generatedBoss.transform.localScale, new Vector3(-1, 1, 1));
            }
        }
    }
    */

    public void GenerateBoss()
    {
        //すでに生成されているなら新しいものに変える。Destroyは遅れて実行されるので明示的にここでリストからのける。
        if (generatedBoss != null) 
        { 
            Destroy(generatedBoss);
            RemoveObject(generatedBoss);
        }

        //ボスを生成しコンテナに登録
        GameObject boss = Generate(originBoss, bossGenerateTrans.position, originBoss.transform.localScale, 0);
        //BossStartrFlip(bossGenerateTrans.localScale.x < 0);
        IContainedObject icontainedObject = boss.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(boss);
        icontainedObject.OnRegist += () => RegistObject(boss);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(boss);
        icontainedObject.OnRemove += () => RemoveObject(boss);
        InitRegist(iobjectContainer, boss);
        InitRegist(this, boss);

        //ボスの攻撃生成スクリプトにコンテナを登録
        IGenerator igenerator = boss.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(iobjectContainer);

        IAimPlayer iaimPlayer = boss.GetComponent<IAimPlayer>();
        if (iaimPlayer != null) 
        {
            // aimPlayerManager.InitSetPlayerTrans(iaimPlayer); 
            iaimPlayer.SetPlayerTrans(GetComponent<IAimPlayer>().GetPlayerTrans());
        }

        //UIに与えるイベント設定。
        BossDataForUI dataForUI = boss.GetComponent<BossDataForUI>();
        dataForUI.OnHpChanged += actionUIController.SetBossHpRate;

        //ボスの登場退場イベントを設定。
        ICharactorEvents bossEvents = boss.GetComponent<ICharactorEvents>();
        bossEvents.OnBirthStart += () => BirthStart();
        bossEvents.OnBirthEnd += () => BirthEnd();
        bossEvents.OnDeathStart += () => DeathStart();
        bossEvents.OnDeathEnd += () => DeathEnd();
    }

    public void BossBirthStart() { generatedBoss.GetComponent<ICharactorEvents>().BirthStart(); }
    private void BirthStart()
    {
        isEntry = true;
    }
    private void BirthEnd()
    {
        isEntry = false;
    }

    private void DeathStart()
    {
        isRetire = true;
        OnBossDeath.Invoke();
    }
    private void DeathEnd()
    {
        isRetire = false;
    }
}
