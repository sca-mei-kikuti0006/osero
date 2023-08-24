using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPlayCon : MonoBehaviour
{
    private MainCon mainCon;

    private MainCon.skill skill = MainCon.skill.Not;
    public MainCon.skill Skill 
    { get { return skill; }
      set { skill = value; }
    }

    private string skillBW = "";
    public string SkillBW
    {
        get { return skillBW; }
        set { skillBW = value; }
    }

    private int skillNumber = 0;
    public int SkillNumber
    {
        get { return skillNumber; }
        set { skillNumber = value; }
    }

    private bool[] canSkillB = { true, true, true };
    private bool[] canSkillW = { true, true, true };

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

    public bool SkillPlayOn() {
        bool can = false;
        if (skillBW == "B")
        {
            can = canSkillB[skillNumber - 1];
        }
        else if (skillBW == "W")
        {
            can = canSkillW[skillNumber - 1];
        }

        bool play = false;
        if (!mainCon.SkillOn && can)
        {
            play = mainCon.SkillPlay(skill,skillBW);
            if (play)
            {
                if (skillBW == "B")
                {
                    canSkillB[skillNumber - 1] = false;
                }
                else if (skillBW == "W")
                {
                    canSkillW[skillNumber - 1] = false;
                }
            }
        }
        return play;
    }
}
