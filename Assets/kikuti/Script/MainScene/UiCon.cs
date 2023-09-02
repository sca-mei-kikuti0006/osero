using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCon : MonoBehaviour
{
    //��̐�ui
    [SerializeField] private Text countBUi;
    [SerializeField] private Text countWUi;

    //�^�[���̕��̐Fui
    [SerializeField] private Text turnUi;

    //���̌아�I��ui
    [SerializeField] private Image selectUi;

    //�T�C�R��
    [SerializeField] private Image dice;
    [SerializeField] private Sprite dice1;
    [SerializeField] private Sprite dice2;
    [SerializeField] private Sprite dice3;


    [SerializeField] private GameObject imageB;
    [SerializeField] private GameObject imageW;

    [SerializeField] private Canvas canvas;


    private MainCon mainCon;

    // Start is called before the first frame update
    void Start()
    {
        mainCon = GetComponent<MainCon>();
        SelectUiF();

        dice.gameObject.SetActive(false);

        ImageT(MainCon.turnBW.Black);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountPieceUi() {
        countBUi.text = string.Format("{00}", MainCon.countB);
        countWUi.text = string.Format("{00}", MainCon.countW);
    }

    public void TurnChangeUiB() {
        turnUi.text = "���̃^�[��";
    }

    public void TurnChangeUiW()
    {
        turnUi.text = "���̃^�[��";
    }

    public void SelectUiT() {
        selectUi.gameObject.SetActive(true);
    }
    public void SelectUiF()
    {
        selectUi.gameObject.SetActive(false);
    }

    public void DiceUi(int ran)
    {
        dice.gameObject.SetActive(true);
        GameObject image = dice.transform.Find("Image").gameObject;
        Image im = image.GetComponent<Image>();
        if (ran == 1)
        {
            im.sprite = dice1;
        }
        else if (ran == 2)
        {
            im.sprite = dice2;
        }
        else if (ran == 3)
        {
            im.sprite = dice3;
        }

    }

    public void ImageT(MainCon.turnBW turn) {
        if(turn == MainCon.turnBW.Black) {
            for (int i = 0; i < 3; i++)
            {
                GameObject imagew = imageW.transform.GetChild(i).gameObject;
                imagew.gameObject.SetActive(true);

            }
            for (int i = 0; i < 3; i++)
            {
                if (!mainCon.canSkillB[i])
                {
                    GameObject imageb = imageB.transform.GetChild(i).gameObject;
                    imageb.gameObject.SetActive(true);
                }
                else
                {
                    GameObject imageb = imageB.transform.GetChild(i).gameObject;
                    imageb.gameObject.SetActive(false);
                }
            }
        }
        else if (turn == MainCon.turnBW.White)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject imageb = imageB.transform.GetChild(i).gameObject;
                imageb.gameObject.SetActive(true);

            }
            for (int i = 0; i < 3; i++)
            {
                if (!mainCon.canSkillW[i])
                {
                    GameObject imagew = imageW.transform.GetChild(i).gameObject;
                    imagew.gameObject.SetActive(true);
                }
                else
                {
                    GameObject imagew = imageW.transform.GetChild(i).gameObject;
                    imagew.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ImageS(){
        for (int i = 0; i < 3; i++)
        {
            GameObject imageb = imageB.transform.GetChild(i).gameObject;
            imageb.gameObject.SetActive(true);
            GameObject imagew = imageW.transform.GetChild(i).gameObject;
            imagew.gameObject.SetActive(true);

        }

    }
}
