using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "partsΜΆ¬")]
public class BaseEquipment : ScriptableObject
{
    public string equipmentName;        // νΜΌO
    public EquipmentType equipmentType; // υΜνή
    public int HP;                      // qbg|Cg
    public int ATK;                     // UΝ
    public int DEF;                     // hδΝ
    public int ROT;                     // ρπ¦iHj
    public Sprite sprite = null;        //ζ

    public enum EquipmentType
    {
        RightHand, // Eθij
        LeftHand,  // Άθiνj
        Head,      // ͺ
        Body,      // Μ
        Feet       // «
    }
}



