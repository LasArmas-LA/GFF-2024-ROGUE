using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestEncount : MonoBehaviour
{
    public enum MainTurn
    {
        WAIT,

        RIRIMOVE,
        RIRIANIM,

        DHIAMOVE,
        DHIAANIM,

        ENEMYMOVE,
        ENEMYANIM,
        
        GAMEOVER,

        END
    }
    public MainTurn mainTurn;

    //バトルコマンドのテキスト
    [Header("バトルコマンドのテキスト")]
    [SerializeField]
    public TextMeshProUGUI windowsMes = null;
    [SerializeField]
    public TextMeshProUGUI command1Text = null;
    [SerializeField]
    public TextMeshProUGUI command2Text = null;
    [SerializeField]
    public TextMeshProUGUI command3Text = null;

    //スクリプト参照
    [SerializeField]
    Riri ririScript = null;
    [SerializeField]
    Dhia dhiaScript = null;
    [SerializeField]
    EnemyManager enemyScript = null;

    //待機時間
    [SerializeField]
    float waitTime = 0;
    float timer = 0;

    FloorNoSys floorNoSys = null;
    GameObject floorNoSysObj = null;
    public EnemyManager rndEnemy = null;
    [SerializeField]
    EnemyFloorRunSys enemyFloorRunSysObj = null;

    [Space(10)]

    [Header("体力ゲージ")]
    [SerializeField, Tooltip("リリーの体力ゲージ")]
    Slider ririSlider = null;
    [SerializeField, Tooltip("ディアの体力ゲージ")]
    Slider dhiaSlider = null;

    [Space(10)]
    [Header("各キャラクターの死亡フラグ")]
    bool ririDeath = false;
    bool dhiaDeath = false;
    bool enemyDeath = false;


    [Space(10)]

    [Header("各キャラクターのオブジェクト")]
    //リリー,ディア,エネミーのObj
    [SerializeField]
    GameObject ririObj;
    [SerializeField]
    GameObject dhiaObj;

    [Space(10)]

    [Header("各キャラクターのコマンドUI")]
    [SerializeField]
    GameObject ririCommand = null;
    [SerializeField]
    GameObject dhiaCommand = null;


    [Space(10)]

    //休憩階のフラグ
    [NonSerialized]
    public bool restFlag = false;

    //ボス階のフラグ
    [NonSerialized]
    public bool bossFlag = false;

    [SerializeField]
    GameObject[] enemyObj = null;

    public int rnd = 0;

    void Start()
    {
        Init();
    }

    void Init()
    {
        //ステータスを待機状態に変更
        mainTurn = MainTurn.WAIT;

        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

        //エネミーのランダム抽選用
        rnd = UnityEngine.Random.Range(0, enemyObj.Length);

        //ランダムで選ばれたエネミーオブジェクトの表示
        enemyObj[rnd].transform.localScale = new Vector3(1,1,1);

        //その情報を格納
        //rndEnemy = rndEnemy.enemy[rnd].GetComponentInParent<EnemyManager>();

        

        //MaxHPの格納
        ririSlider.maxValue = ririScript.maxhp;
        dhiaSlider.maxValue = dhiaScript.maxhp;

        //MinHPの格納
        ririSlider.minValue = 0;
        dhiaSlider.minValue = 0;

        //MaxのHPを現在のHPに格納
        ririSlider.value = ririSlider.maxValue;
        dhiaSlider.value = dhiaSlider.maxValue;
        
        //MaxのHPを現在のHPに格納
        ririSlider.value *= (ririScript.hp / ririScript.maxhp);
        dhiaSlider.value *= (dhiaScript.hp / dhiaScript.maxhp);
    }

    bool fast = true;
    float ririhpdf = 0;
    float dhiahpdf = 0;

    void Update()
    {
        switch (mainTurn)
        {
            case MainTurn.WAIT:
                break;
            case MainTurn.RIRIMOVE:
                //リリー死亡時ターンをスキップ
                if(ririScript.deathFlag)
                {
                    mainTurn = MainTurn.GAMEOVER;
                }
                if(fast)
                {
                    //コマンド部分の表示切り替え
                    dhiaCommand.SetActive(false);
                    ririCommand.SetActive(true);

                    //ダメージを受けた時を判別できるように格納
                    ririhpdf = ririScript.hp;
                    fast = false;
                }
                break;
            case MainTurn.RIRIANIM:
                if (!fast)
                {
                    fast = true;
                }

                break;
            case MainTurn.DHIAMOVE:
                //ディア死亡時ターンをスキップ
                if (dhiaScript.deathFlag)
                {
                    mainTurn = MainTurn.ENEMYMOVE;
                }
                if (fast)
                {
                    //コマンド部分の表示切り替え
                    ririCommand.SetActive(false);
                    dhiaCommand.SetActive(true);

                    //ダメージを受けた時を判別できるように格納
                    dhiahpdf = dhiaScript.hp;
                    fast = false;
                }
                break;
            case MainTurn.DHIAANIM:
                if (rndEnemy.deathFlag)
                {
                    //タイマー開始
                    timer += Time.deltaTime;

                    if(timer >= 3)
                    {
                        mainTurn = MainTurn.END;
                        timer = 0;
                    }
                }
                if(!fast)
                {
                    fast = true;
                }
                break;
            case MainTurn.ENEMYMOVE:
                enemyScript.enemyHpDef = enemyScript.hp;
                break;
            case MainTurn.ENEMYANIM:
                break;
            case MainTurn.GAMEOVER:
                timer += Time.deltaTime;

                if (timer >= 2)
                {
                    SceneManager.LoadScene("GameOver");
                    timer = 0;
                }

                break;
            case MainTurn.END:
                break;
        }
        
        RiriMove();
        DhiaMove();
        EnemyMove();

        //リリーのHPが削られた時
        if (ririhpdf > ririScript.hp && ririSlider.value >= (ririSlider.maxValue * (ririScript.hp / ririScript.maxhp)))
        {
            Debug.Log("リリーが攻撃を受けた");
            ririSlider.value -= (ririSlider.maxValue * (ririScript.hp / ririScript.maxhp))* 1.5f * Time.deltaTime;
            ririhpdf = ririScript.hp;
            
        }

        //リリーのHPが回復された時
        if (ririhpdf < ririScript.hp && ririSlider.value >= (ririSlider.maxValue * (ririScript.hp / ririScript.maxhp)))
        {
            ririSlider.value += (ririSlider.maxValue * (ririScript.hp / ririScript.maxhp)) * Time.deltaTime;
            ririhpdf = ririScript.hp;
        }

        //ディアのHPが削られた時
        if (dhiahpdf > dhiaScript.hp && dhiaSlider.value >= (dhiaSlider.maxValue * (dhiaScript.hp / dhiaScript.maxhp)))
        {
            Debug.Log("ディアが攻撃を受けた");
            dhiaSlider.value -= (dhiaSlider.maxValue * (dhiaScript.hp / dhiaScript.maxhp)) * 1.5f * Time.deltaTime;
            dhiahpdf = dhiaScript.hp;
        }

        //ディアのHPが回復された時
        if (dhiahpdf < dhiaScript.hp && dhiaSlider.value >= (dhiaSlider.maxValue * (dhiaScript.hp / dhiaScript.maxhp)))
        {
            dhiaSlider.value += (dhiaSlider.maxValue * (dhiaScript.hp / dhiaScript.maxhp)) * Time.deltaTime;
            dhiahpdf = dhiaScript.hp;
        }
    }

    //コマンド処理
    public bool command1 = false;
    public bool command2 = false;
    public bool command3 = false;
    bool button = false;
    bool coLock = false;

    public void Command1()
    {
        //多段押し防止
        if(!button)
        {
            button = true;
            command1 = true;
        }
    }
    public void Command2()
    {
        //多段押し防止
        if (!button)
        {
            button = true;
            command2 = true;
        }
    }
    public void Command3()
    {
        //多段押し防止
        if (!button)
        {
            button = true;
            command3 = true;
        }
    }

    void RiriMove()
    {
        if (mainTurn == MainTurn.RIRIMOVE || mainTurn == MainTurn.RIRIANIM)
        {
            command1Text.text = "ヒール";
            command2Text.text = "オールヒール";
            command3Text.text = "バイキルト";

            Debug.Log("リリーのターン");
            if (command1 || command2 || command3)
            {
                //タイマー開始
                timer += Time.deltaTime;

                //ステータスを変更
                mainTurn = MainTurn.RIRIANIM;

                if (!coLock)
                {
                    if (command1)
                    {
                        ririScript.Skil1();
                    }
                    if (command2)
                    {
                        ririScript.Skil2();
                    }
                    if (command3)
                    {
                        ririScript.Skil3();
                    }
                    coLock = true;
                }
            }
            else
            {
                windowsMes.text = "リリーの行動をにゅうりょくしてください";
            }
            //待機時間を超えたら
            if (timer >= waitTime)
            {
                //ステータスを変更
                mainTurn = MainTurn.DHIAMOVE;
                timer = 0;
                button = false;
                command1 = false;
                command2 = false;
                command3 = false;
                coLock = false;
                ririScript.button = false;
            }
        }
    }
    void DhiaMove()
    {
        if (mainTurn == MainTurn.DHIAMOVE || mainTurn == MainTurn.DHIAANIM)
        {
            Debug.Log("ディアのターン");
            command1Text.text = "殴る";
            command2Text.text = "防御体制";
            command3Text.text = "守る";

            if (command1 || command2 || command3)
            {
                //タイマー開始
                timer += Time.deltaTime;

                //ステータスを変更
                mainTurn = MainTurn.DHIAANIM;

                if (!coLock)
                {
                    if (command1)
                    {
                        dhiaScript.Skil1();
                    }
                    if (command2)
                    {
                        dhiaScript.Skil2();
                    }
                    if (command3)
                    {
                        dhiaScript.Skil3();
                    }
                    coLock = true;
                }
            
                //待機時間を超えて敵が生きている時
                if (timer >= waitTime && !enemyDeath)
                {
                    //ステータスを変更
                    mainTurn = MainTurn.ENEMYMOVE;
                    timer = 0;
                    button = false;
                    coLock = false;
                    command1 = false;
                    command2 = false;
                    command3 = false;
                }
            }
            else
            {
                windowsMes.text = "ディアの行動をにゅうりょくしてください";
            }
        }
    }
    void EnemyMove()
    {
        if (mainTurn == MainTurn.ENEMYMOVE)
        {
            //タイマー開始
            timer += Time.deltaTime;

            //ステータスを変更
            //mainTurn = MainTurn.ENEMYANIM;

            if (!coLock)
            {
                //Init時に選択されたエネミーのスキル関数を呼び出す
                rndEnemy.Move();
                coLock = true;
            }

            //待機時間を超えたら
            if (timer >= waitTime)
            {
                //ステータスを変更
                mainTurn = MainTurn.RIRIMOVE;
                coLock = false;
                timer = 0;
            }
        }
    }
}
