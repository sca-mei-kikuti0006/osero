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
    [SerializeField] private Image turnUi;

    //���̌아�I��ui
    [SerializeField] private Image selectUi;

    //�X�L�����O



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
