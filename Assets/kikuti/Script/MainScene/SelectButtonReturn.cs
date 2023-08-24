using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonReturn : MonoBehaviour
{
    private MainCon mainCon;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("GameManager");
        mainCon = obj.GetComponent<MainCon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        string select = this.gameObject.name.Substring(6, 2);
        if(select == "Mg") { 
            mainCon.SkillMagicStorm(true);
        }
        else if(select == "Tr") {
            if (mainCon.SearchMagicStormTr())
            {
                mainCon.SkillMagicStorm(false);
            }
        }
    }
}
