using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCon : MonoBehaviour
{
    //駒の数ui
    [SerializeField] private Text countBUi;
    [SerializeField] private Text countWUi;

    //ターンの方の色ui
    [SerializeField] private Text turnUi;

    //嵐の護符選択ui
    [SerializeField] private Image selectUi;

    //サイコロ
    [SerializeField] private Image dice;
    [SerializeField] private Sprite dice1;
    [SerializeField] private Sprite dice2;
    [SerializeField] private Sprite dice3;

    [SerializeField] private Canvas canvas;


    private MainCon mainCon;

    // Start is called before the first frame update
    void Start()
    {
        mainCon = GetComponent<MainCon>();
        SelectUiF();

        dice.gameObject.SetActive(false);
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
        turnUi.text = "黒のターン";
    }

    public void TurnChangeUiW()
    {
        turnUi.text = "白のターン";
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
}
