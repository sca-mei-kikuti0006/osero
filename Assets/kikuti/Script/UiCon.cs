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
    [SerializeField] private Image turnUi;

    //嵐の護符選択ui
    [SerializeField] private Image selectUi;

    //スキル名前



    private MainCon mainCon;

    // Start is called before the first frame update
    void Start()
    {
        mainCon = GetComponent<MainCon>();
        SelectUiF();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountPieceUi() {
        countBUi.text = string.Format("{00}", mainCon.CountB);
        countWUi.text = string.Format("{00}", mainCon.CountW);
    }

    public void TurnChangeUiB() {
        turnUi.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 255.0f);
    }

    public void TurnChangeUiW()
    {
        turnUi.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f);
    }

    public void SelectUiT() {
        selectUi.gameObject.SetActive(true);
    }
    public void SelectUiF()
    {
        selectUi.gameObject.SetActive(false);
    }
}
