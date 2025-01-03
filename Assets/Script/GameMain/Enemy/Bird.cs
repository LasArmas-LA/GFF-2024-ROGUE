using UnityEngine;
using UnityEngine.UI;

public class Bird : EnemyManager
{
    [SerializeField]
    Status enemyStatus = null;

    [Header("クラス参照")]
    [SerializeField]
    Riri riri = null;
    [SerializeField]
    Dhia dhia = null;

    [SerializeField]
    EnemyManager enemySys = null;

    //攻撃力の補正値
    float powerValue = 0f;


    public override void InitBird()
    {
        Debug.Log("初期化");

        deathFlag = false;

        this.gameObject.transform.localScale = new Vector3(1,1,1);

        maxhp = enemyStatus.MAXHP;
        maxmp = enemyStatus.MAXMP;
        power = enemyStatus.ATK;
        def = enemyStatus.DEF;


        hp = maxhp;
        mp = maxmp;
    }

    void Update()
    {

    }

    public override void SkilBird()
    {
        int skilRnd = 0;
        int slectNo = 0;
        for (int i = 0; i < 1; i++)
        {
            //0から100の乱数
            skilRnd = UnityEngine.Random.Range(1, 100);
        }

        //スキル1
        if (skilRnd <= 100)
        {
            int ririDamage = DamageCalculation(power, riri.def);
            float dhiaDamage = DamageCalculation(power, dhia.def);

            //リリーの方がHP多い時
            if (riri.hp > dhia.hp)
            {
                slectNo = 1;
            }
            //ディアの方がHP多い時
            else
            {
                slectNo = 0;
            }

            //ディアが死んでいる時攻撃対象をリリーに上書き
            if (dhia.deathFlag)
            {
                slectNo = 0;
            }

            //攻撃対象リリー
            if (slectNo == 0)
            {
                //70%軽減
                if (dhia.ririDefenseFlag)
                {
                    encountSys.windowsMes.text = "ふくろうのこうげき！ディアがリリーを守った！ディアに" + (ririDamage * 0.3f) + "のダメージ!";
                    dhia.hp -= (ririDamage * 0.3f);
                }
                else
                {
                    encountSys.windowsMes.text = "ふくろうのこうげき！リリーに" + (ririDamage) + "のダメージ!";
                    riri.hp -= (ririDamage);
                }
            }
            //攻撃対象ディア
            else if (slectNo == 1)
            {
                if (dhia.defenseFlag)
                {
                    encountSys.windowsMes.text = "ふくろうのこうげき！ディアに" + (dhiaDamage * 0.5f) + "のダメージ!";
                    dhia.hp -= (dhiaDamage * 0.5f);
                }
                else
                {
                    encountSys.windowsMes.text = "ふくろうのこうげき！ディアに" + (dhiaDamage) + "のダメージ!";
                    dhia.hp -= (dhiaDamage);
                }
            }

        }

        //スキル2
        if (skilRnd >= 1000)
        {

        }
    }

    //ダメージ計算用
    int DamageCalculation(int attack, int defense)
    {
        //シード値の変更
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

        //素のダメージ計算
        int damage = (attack + (attack * (int)powerValue) / 2) - (defense / 4);

        //ダメージ振幅の計算
        int width = damage / 16 + 1;

        //ダメージ振幅値を加味した計算
        damage = UnityEngine.Random.Range(damage - width, damage + width);

        //呼び出し側にダメージ数を返す
        return damage;
    }

}