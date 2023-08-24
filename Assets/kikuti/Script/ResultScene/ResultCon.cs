using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCon : MonoBehaviour
{

    [SerializeField] private Text countBUi;
    [SerializeField] private Text countWUi;

    [SerializeField] private Text winTextUi;
    private Animator winAnim;

    private bool countEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Count(MainCon.countB,countBUi));
        //StartCoroutine(Count(MainCon.countW, countWUi));

        winTextUi.gameObject.SetActive(false);
        winAnim = winTextUi.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Count(int countP,Text textUi) {
        int count = 0;
        while (count < countP)
        {
            count++;
            textUi.text = string.Format("{0}",count);
            yield return new WaitForSeconds(0.1f);
        }

        if (countEnd) {
            if (MainCon.countB > MainCon.countW)
            {
                winTextUi.text = "çïÇÃèüÇø";
            }
            else if (MainCon.countB < MainCon.countW)
            {
                winTextUi.text = "îíÇÃèüÇø";
            }
            else {
                winTextUi.text = "à¯Ç´ï™ÇØ";
            }
            winTextUi.gameObject.SetActive(true);
            winAnim.SetTrigger("winAnimStart");
        }

        countEnd = true;
    }
}
