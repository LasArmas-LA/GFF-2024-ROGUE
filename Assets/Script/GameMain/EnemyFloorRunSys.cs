using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BaseEquipment;
using static TestEncount;

public class EnemyFloorRunSys : MonoBehaviour
{
    //メインカメラ
    public Camera maincamera = null;

    //敵の場所まで歩くフラグ
    bool runStratFlag = false;
    //敵を倒して扉まで歩く時のフラグ
    public bool battleEndFlag = false;
    //扉に着いてその階が終了する時のフラグ
    bool floorEndFlag = false;
    //フェードアウト用
    [SerializeField]
    Image fade = null;
    //最初に1回だけ呼び出したい処理
    bool fast = false;
    //ボタンの多段押し防止
    bool button = false;

    //どのドロップしたパーツを選択しているかの確認用
    [SerializeField]
    bool[] partsSlect;

    //パーツの名前
    string[] partsName = { "RightHand", "LeftHand", "Head", "Body", "Feet" };

    //ドロップしたパーツの情報表示用
    [SerializeField]
    TextMeshProUGUI[] slectText;

    //現在装備しているパーツの情報表示用
    [SerializeField]
    TextMeshProUGUI[] slectNowText;

    //パーツを選択後確定させた時の判断
    bool allPartsSlect;

    //最初に1回だけ呼び出したい処理
    bool fastMove = true;

    //ゲームオーバーフラグ
    [NonSerialized]
    public bool gameOverFlag = false;

    //パーツを選択するウィンドウ
    [SerializeField]
    GameObject partsSlectWin = null;

    //コマンドを選択するウィンドウ
    [SerializeField]
    GameObject commandWin = null;

    //カメラの動く速度
    [SerializeField]
    Vector3 cameraMoveSpeed = Vector3.zero;

    //チェストのオブジェクト
    [SerializeField]
    GameObject chestObj = null;

    //階層データ管理システム
    FloorNoSys floorNoSys = null;
    GameObject floorNoSysObj = null;

    //エンカウントを管理するシステム
    [SerializeField]
    TestEncount encountSys = null;

    //装備をランダムで入手するロジック組みのシステム
    [SerializeField]
    EquipmentManager equipmentManager = null;

    //ディアのステータス
    [SerializeField]
    Status dhiaStatus = null;

    [SerializeField]
    GameObject enemyObj = null;
    [SerializeField]
    GameObject restObj = null;
    [SerializeField]
    GameObject doorObj = null;


    public TextMeshProUGUI windowMes = null;

    void Start()
    {
        Init();
    }

    void Init()
    {
        windowMes.text = "探索中";
        floorNoSysObj = GameObject.Find("FloorNo");
        floorNoSys = floorNoSysObj.GetComponent<FloorNoSys>();
        commandWin.SetActive(false);
        equipmentManager.Start();
    }
    void Update()
    {
        CameraMove();

        //フェードアウト処理
        if (floorEndFlag)
        {
            Invoke("LoadScene", 1.0f);
            if (fade != null)
            {

                floorEndFlag = false;
            }
        }

        if(gameOverFlag)
        {
            GameOver();
        }
    }

    void CameraMove()
    {
        if (encountSys.mainTurn == MainTurn.WAIT)
        {
            if (maincamera.transform.position.x >= enemyObj.transform.position.x - 50)
            {
                //1回だけ呼び出す
                if (!fast)
                {
                    //ステータスの変更
                    encountSys.mainTurn = TestEncount.MainTurn.RIRIMOVE;

                    commandWin.SetActive(true);
                    fast = true;
                    runStratFlag = false;
                }
                //StartCoroutine(ChestWait());
            }
            else
            {
                commandWin.SetActive(false);
                //カメラの移動処理
                maincamera.transform.position += cameraMoveSpeed * Time.deltaTime;
            }
        }
        if (encountSys.mainTurn == MainTurn.END)
        {
            if (encountSys.restFlag)
            {
                if (maincamera.transform.position.x <= restObj.transform.position.x - 50)
                {
                    windowMes.text = "探索中";
                    commandWin.SetActive(false);
                    maincamera.transform.position += cameraMoveSpeed * Time.deltaTime;
                }
                else
                {
                    windowMes.text = "休憩中";
                    commandWin.SetActive(false);
                    StartCoroutine(RestStay());
                }
            }
            else
            {
                if (!allPartsSlect)
                {
                    if (fastMove)
                    {
                        slectText[0].text = equipmentManager.randomEquip[equipmentManager.rnd[0]].equipmentName + "\nATK :" + equipmentManager.randomEquip[equipmentManager.rnd[0]].ATK;
                        slectText[1].text = equipmentManager.randomEquip[equipmentManager.rnd[1]].equipmentName + "\nATK :" + equipmentManager.randomEquip[equipmentManager.rnd[1]].ATK;
                        slectText[2].text = equipmentManager.randomEquip[equipmentManager.rnd[2]].equipmentName + "\nATK :" + equipmentManager.randomEquip[equipmentManager.rnd[2]].ATK;

                        //ドロップパーツの表示処理
                        for (int i = 0; i < slectNowText.Length;)
                        {
                            //右手
                            if (equipmentManager.randomEquip[equipmentManager.rnd[i]].equipmentType == EquipmentType.RightHand)
                            {
                                if (dhiaStatus.righthandPartsData != null)
                                {
                                    slectNowText[i].text = dhiaStatus.righthandPartsData.equipmentName + "\nATK :" + dhiaStatus.righthandPartsData.ATK;
                                }
                                else
                                {
                                    slectNowText[i].text = "";
                                }
                            }
                            //左手
                            if (equipmentManager.randomEquip[equipmentManager.rnd[i]].equipmentType == EquipmentType.LeftHand)
                            {
                                if (dhiaStatus.lefthandPartsData != null)
                                {
                                    slectNowText[i].text = dhiaStatus.lefthandPartsData.equipmentName + "\nATK :" + dhiaStatus.lefthandPartsData.ATK;
                                }
                                else
                                {
                                    slectNowText[i].text = "";
                                }
                            }
                            //足
                            if (equipmentManager.randomEquip[equipmentManager.rnd[i]].equipmentType == EquipmentType.Feet)
                            {
                                if (dhiaStatus.legPartsData != null)
                                {
                                    slectNowText[i].text = dhiaStatus.legPartsData.equipmentName + "\nATK :" + dhiaStatus.legPartsData.ATK;
                                }
                                else
                                {
                                    slectNowText[i].text = "";
                                }
                            }
                            //体
                            if (equipmentManager.randomEquip[equipmentManager.rnd[i]].equipmentType == EquipmentType.Body)
                            {
                                if (dhiaStatus.bodyPartsData != null)
                                {
                                    slectNowText[i].text = dhiaStatus.bodyPartsData.equipmentName + "\nATK :" + dhiaStatus.bodyPartsData.ATK;
                                }
                                else
                                {
                                    slectNowText[i].text = "";
                                }
                            }
                            //頭
                            if (equipmentManager.randomEquip[equipmentManager.rnd[i]].equipmentType == EquipmentType.Head)
                            {
                                if (dhiaStatus.headPartsData != null)
                                {
                                    slectNowText[i].text = dhiaStatus.headPartsData.equipmentName + "\nATK :" + dhiaStatus.headPartsData.ATK;
                                }
                                else
                                {
                                    slectNowText[i].text = "";
                                }
                            }
                            i++;
                        }
                        fastMove = false;
                    }

                    partsSlectWin.SetActive(true);
                }
                else
                {
                    if (maincamera.transform.position.x <= doorObj.transform.position.x - 50)
                    {
                        windowMes.text = "探索中";
                        commandWin.SetActive(false);
                        maincamera.transform.position += cameraMoveSpeed * Time.deltaTime;
                    }
                    else
                    {
                        windowMes.text = "扉を見つけた！ \n次の階に進もう";
                        commandWin.SetActive(false);
                        StartCoroutine(FloorEnd());
                    }
                }
            }
        }

    }

    void GameOver()
    {

    }

    public void PartsSlect1()
    {
        if(!button)
        {
            partsSlect[0] = true;
            partsSlect[1] = false;
            partsSlect[2] = false;
        }
    }
    public void PartsSlect2()
    {
        if (!button)
        {
            partsSlect[0] = false;
            partsSlect[1] = true;
            partsSlect[2] = false;
        }
    }
    public void PartsSlect3()
    {
        if (!button)
        {
            partsSlect[0] = false;
            partsSlect[1] = false;
            partsSlect[2] = true;
        }
    }

    public void PartsSlecteEnd()
    {
        button = true;
        allPartsSlect = true;
        partsSlectWin.SetActive(false);

        
        if (partsSlect[0])
        {
            //右手
            if(equipmentManager.randomEquip[equipmentManager.rnd[0]].equipmentType == EquipmentType.RightHand)
            {
                dhiaStatus.righthandPartsData = equipmentManager.randomEquip[equipmentManager.rnd[0]];
            }
            //左手
            if (equipmentManager.randomEquip[equipmentManager.rnd[0]].equipmentType == EquipmentType.LeftHand)
            {
                dhiaStatus.lefthandPartsData = equipmentManager.randomEquip[equipmentManager.rnd[0]];
            }
            //足
            if (equipmentManager.randomEquip[equipmentManager.rnd[0]].equipmentType == EquipmentType.Feet)
            {
                dhiaStatus.legPartsData = equipmentManager.randomEquip[equipmentManager.rnd[0]];
            }
            //体
            if (equipmentManager.randomEquip[equipmentManager.rnd[0]].equipmentType == EquipmentType.Body)
            {
                dhiaStatus.bodyPartsData = equipmentManager.randomEquip[equipmentManager.rnd[0]];
            }
            //頭
            if (equipmentManager.randomEquip[equipmentManager.rnd[0]].equipmentType == EquipmentType.Head)
            {
                dhiaStatus.headPartsData = equipmentManager.randomEquip[equipmentManager.rnd[0]];
            }
        }
        if (partsSlect[1])
        {
            //右手
            if (equipmentManager.randomEquip[equipmentManager.rnd[1]].equipmentType == EquipmentType.RightHand)
            {
                dhiaStatus.righthandPartsData = equipmentManager.randomEquip[equipmentManager.rnd[1]];
            }
            //左手
            if (equipmentManager.randomEquip[equipmentManager.rnd[1]].equipmentType == EquipmentType.LeftHand)
            {
                dhiaStatus.lefthandPartsData = equipmentManager.randomEquip[equipmentManager.rnd[1]];
            }
            //足
            if (equipmentManager.randomEquip[equipmentManager.rnd[1]].equipmentType == EquipmentType.Feet)
            {
                dhiaStatus.legPartsData = equipmentManager.randomEquip[equipmentManager.rnd[1]];
            }
            //体
            if (equipmentManager.randomEquip[equipmentManager.rnd[1]].equipmentType == EquipmentType.Body)
            {
                dhiaStatus.bodyPartsData = equipmentManager.randomEquip[equipmentManager.rnd[1]];
            }
            //頭
            if (equipmentManager.randomEquip[equipmentManager.rnd[1]].equipmentType == EquipmentType.Head)
            {
                dhiaStatus.headPartsData = equipmentManager.randomEquip[equipmentManager.rnd[1]];
            }
        }
        if (partsSlect[2])
        {
            //右手
            if (equipmentManager.randomEquip[equipmentManager.rnd[2]].equipmentType == EquipmentType.RightHand)
            {
                dhiaStatus.righthandPartsData = equipmentManager.randomEquip[equipmentManager.rnd[2]];
            }
            //左手
            if (equipmentManager.randomEquip[equipmentManager.rnd[2]].equipmentType == EquipmentType.LeftHand)
            {
                dhiaStatus.lefthandPartsData = equipmentManager.randomEquip[equipmentManager.rnd[2]];
            }
            //足
            if (equipmentManager.randomEquip[equipmentManager.rnd[2]].equipmentType == EquipmentType.Feet)
            {
                dhiaStatus.legPartsData = equipmentManager.randomEquip[equipmentManager.rnd[2]];
            }
            //体
            if (equipmentManager.randomEquip[equipmentManager.rnd[2]].equipmentType == EquipmentType.Body)
            {
                dhiaStatus.bodyPartsData = equipmentManager.randomEquip[equipmentManager.rnd[2]];
            }
            //頭
            if (equipmentManager.randomEquip[equipmentManager.rnd[2]].equipmentType == EquipmentType.Head)
            {
                dhiaStatus.headPartsData = equipmentManager.randomEquip[equipmentManager.rnd[2]];
            }
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene("LoadScene");
    }

    IEnumerator FloorEnd()
    {
        yield return new WaitForSeconds(1.0f);
        if (battleEndFlag)
        {
            floorNoSys.floorNo += 1;
        }
        battleEndFlag = false;
        floorEndFlag = true;
    }
    IEnumerator RestStay()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        encountSys.restFlag = false;
    }
}
