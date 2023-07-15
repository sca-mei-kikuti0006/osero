using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonReturn : MonoBehaviour
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

    public void OnClick() {
        if (!mainCon.SkillOn) { 
            string skillBW= this.gameObject.name.Substring(5, 1);
            int skillNumber = int.Parse(this.gameObject.name.Substring(6, 1));
            mainCon.SkillPlay(skillBW,skillNumber);
        }
    }
}
