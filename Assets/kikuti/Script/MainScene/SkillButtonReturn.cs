using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonReturn : MonoBehaviour
{
    private SkillPlayCon skillCon;
    private MainCon mainCon;

    [SerializeField] private Sprite spriteTenm;
    [SerializeField] private Sprite spriteStorm;
    [SerializeField] private Sprite spriteRever;
    [SerializeField] private Sprite spriteSetb;
    [SerializeField] private Sprite spriteLand;
    [SerializeField] private Sprite spriteLight;
    private Image image;

    [SerializeField] private Image skillComent;
    [SerializeField] private Text comentText;
    [SerializeField] private Button playButt;
    [SerializeField] private Image comentImage;
    private Image comImage;

    private MainCon.skill skill = MainCon.skill.Not;
    private MainCon.turnBW BW = MainCon.turnBW.Not;
    private string bw;
    private int number;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("GameManager");
        mainCon = obj.GetComponent<MainCon>();
        skillCon = obj.GetComponent<SkillPlayCon>();

        image = GetComponent<Image>();
        comImage = comentImage.GetComponent<Image>();

        skillComent.gameObject.SetActive(false);

        bw = this.gameObject.name.Substring(5, 1);
        number = int.Parse(this.gameObject.name.Substring(6, 1));
        if (bw == "B") {
            skill = MainCon.skillB[number-1];
            BW = MainCon.turnBW.Black;
        }else if (bw == "W")
        {
            skill = MainCon.skillW[number-1];
            BW = MainCon.turnBW.White;
        }

        switch (skill) {
            case MainCon.skill.MgTenm:
                image.sprite = spriteTenm;
                break;
            case MainCon.skill.MgStorm:
                image.sprite = spriteStorm;
                break;
            case MainCon.skill.MgRever:
                image.sprite = spriteRever;
                break;
            case MainCon.skill.TrSetb:
                image.sprite = spriteSetb;
                break;
            case MainCon.skill.TrLand:
                image.sprite = spriteLand;
                break;
            case MainCon.skill.TrLight:
                image.sprite = spriteLight;
                break;
            default:
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        if (mainCon.CanCrick)
        {
            playButt.gameObject.SetActive(true);
            if (!mainCon.SearchSkillUi(skill, BW, number - 1))
            {
                playButt.gameObject.SetActive(false);
            }

            switch (skill)
            {
                case MainCon.skill.MgTenm:
                    comImage.sprite = spriteTenm;
                    comentText.text = "サイコロを振って出た目の数分\nランダムに相手のオセロを\n自分の色に変える\n\n発動条件角\nを2つ以上取られないと\n発動できない";
                    break;
                case MainCon.skill.MgStorm:
                    comImage.sprite = spriteStorm;
                    comentText.text = "相手の\nトラップかマジックを\n無効にする";
                    break;
                case MainCon.skill.MgRever:
                    comImage.sprite = spriteRever;
                    comentText.text = "指定した縦列のオセロを3つ\nランダムに自分の色に変える\n\n発動条件\n角を1つ以上取られないと\n発動できない\n";
                    break;
                case MainCon.skill.TrSetb:
                    comImage.sprite = spriteSetb;
                    comentText.text = "マスを指定し\n相手がそのマスに\nオセロを置いたとき\nその設置を無効にする\n";
                    break;
                case MainCon.skill.TrLand:
                    comImage.sprite = spriteLand;
                    comentText.text = "マスを指定し\nそのマスにオセロを置いたとき\n3×3のマスの\nオセロを取り除く\n";
                    break;
                case MainCon.skill.TrLight:
                    comImage.sprite = spriteLight;
                    comentText.text = "マスを指定し\n次に相手がオセロを置くとき\nこのトラップの場所に\n移動させる\n";
                    break;
                default:
                    break;
            }
            skillComent.gameObject.SetActive(true);
            skillCon.Skill = skill;
            skillCon.SkillBW = bw;
            skillCon.SkillNumber = number;
        }
    }
}
