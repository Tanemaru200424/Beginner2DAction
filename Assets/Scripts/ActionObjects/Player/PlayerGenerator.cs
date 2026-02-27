using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//プレイヤーオブジェクトを生成し、様々な管理者に設定するなどを行う。
//ステージ処理の影響下に置くためのスクリプト。
//プレイヤーの登場、死亡に関わるイベントも行う。
public class PlayerGenerator : MonoBehaviour, IGenerator
{
    [SerializeField] private GameObject originPlayer = null;
    private GameObject generatedPlayer = null; //生成したプレイヤー本体。
    private bool isDead = false; //ボス戦で相打ちになったときに使うフラグ。
    private PlayerInputHandler playerInputHandler = null;

    [SerializeField] private ActionObjectContainer actionObjectContainer = null;
    private IObjectContainer iobjectContainer = null;

    [SerializeField] private ActionUIController actionUIController = null;
    [SerializeField] private CameraBrain cameraBrain = null;
    [SerializeField] private PauseManager pauseManager = null;
    [SerializeField] private AimPlayerManager aimPlayerManager = null;
    [SerializeField] private StageManager stageManager = null;

    public bool isEntry { get; private set; } = false;
    public bool isRetire { get; private set; } = false;
    public event Action OnPlayerDeath;

    void Awake()
    {
        SetObjectContainer(actionObjectContainer.GetComponent<IObjectContainer>());
    }

    public void PlayerInputActiveSwitch(bool isactive)
    {
        if (generatedPlayer != null) { playerInputHandler?.ActiveSwitch(isactive); }
    }
    public void SetPlayerPos(Vector3 newPos) //プレイヤーの位置を直接変更する権利を持つ。
    {
        if (generatedPlayer != null) { generatedPlayer.transform.position = newPos; }
    }
    public void SetPlayerFlip(bool isFlip) //プレイヤーの向きを指定する。
    {
        if (generatedPlayer != null)
        {
            if ((generatedPlayer.transform.localScale.x > 0 && isFlip) ||
                (generatedPlayer.transform.localScale.x < 0 && !isFlip))
            {
                generatedPlayer.transform.localScale = Vector3.Scale(generatedPlayer.transform.localScale, new Vector3(-1, 1, 1));
            }
        }
    }
    public void PlayerIncredibleSwitch(bool isIncredible) { generatedPlayer.GetComponent<PlayerState>()?.IncredibleSwitch(isIncredible); }
    public void SetPlayerNomal() { generatedPlayer.GetComponent<PlayerNomalization>()?.Nomalization(); }

    public void SetObjectContainer(IObjectContainer iobjectContainer) { this.iobjectContainer = iobjectContainer; }
    public GameObject Generate(GameObject gameObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject player = Instantiate(gameObject, generatePos, newRotation);
        player.transform.localScale = generateScale;
        return player;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf) { iobjectContainer.RegistObject(generateObject); }
    }

    public void GeneratePlayer()
    {
        if (generatedPlayer != null) { return; }
        generatedPlayer = Generate(originPlayer, stageManager.PlayerGeneratePos, originPlayer.transform.localScale, 0);
        SetPlayerFlip(stageManager.IsPlayerStartFlip);
        cameraBrain.SetPlayerTrans(generatedPlayer.transform);

        //プレイヤーをゲームオブジェクトコンテナに登録
        IContainedObject icontainedObject = generatedPlayer.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(generatedPlayer);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(generatedPlayer);
        InitRegist(iobjectContainer, generatedPlayer);

        //プレイヤーのエフェクト生成にコンテナ登録。
        IGenerator igenerator = generatedPlayer.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(actionObjectContainer);

        //一時停止を入力で行えるようにする
        playerInputHandler = generatedPlayer.GetComponent<PlayerInputHandler>();
        playerInputHandler.AddPausePressed(pauseManager.PlayerPause);

        //UIに与えるイベント設定。
        PlayerDataForUI dataForUI = generatedPlayer.GetComponent<PlayerDataForUI>();
        dataForUI.OnHpChanged += actionUIController.SetPlayerHpRate;
        dataForUI.OnChargeChanged += actionUIController.SetPlayerChargeRate;

        //プレイヤーの登場退場イベントを設定。
        ICharactorEvents playerEvents = generatedPlayer.GetComponent<ICharactorEvents>();
        playerEvents.OnBirthStart += () => BirthStart();
        playerEvents.OnBirthEnd += () => BirthEnd();
        playerEvents.OnDeathStart += () => DeathStart();
        playerEvents.OnDeathEnd += () => DeathEnd();
    }
    //相打ちなどイベント中にプレイヤーが死んだときに使う。
    public void ReGeneratePlayer()
    {
        if (!isDead) { return; }
        if (generatedPlayer != null) 
        {
            Destroy(generatedPlayer);
            generatedPlayer = null;
        }
        GeneratePlayer();
    }

    public void PlayerBirthStart() { generatedPlayer.GetComponent<ICharactorEvents>().BirthStart(); }
    private void BirthStart()
    {
        PlayerInputActiveSwitch(false); //入力無向
        isDead = false;
        isEntry = true;
    }
    private void BirthEnd()
    {
        PlayerInputActiveSwitch(true); //入力有効
        isEntry = false;
    }

    public void DeathStart()
    {
        aimPlayerManager.AllCancelPlayerTrans();
        PlayerInputActiveSwitch(false); //入力無向
        isDead = true;
        isRetire = true;
        OnPlayerDeath?.Invoke();
    }
    public void DeathEnd()
    {
        generatedPlayer = null;
        cameraBrain.CancelPlayerTrans();
        playerInputHandler = null;
        isRetire = false;
    }
}
