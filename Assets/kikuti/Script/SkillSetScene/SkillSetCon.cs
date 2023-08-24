using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkillSetCon : MonoBehaviour
{
    private int count = 0;
    private MainCon.turnBW skillTurn = MainCon.turnBW.Black;
    public MainCon.turnBW SkillTurn { get { return skillTurn; } }

    [SerializeField] private Text skillTurnUi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkillSet(string skilName) {
        MainCon.skill skill = (MainCon.skill)Enum.Parse(typeof(MainCon.skill), skilName);
        if (skillTurn == MainCon.turnBW.Black)
        {
            MainCon.skillB[count] = skill;
            skillTurn = MainCon.turnBW.White;
            skillTurnUi.text = "白が選ぶターン";
        }
        else {
            MainCon.skillW[count] = skill;
            skillTurn = MainCon.turnBW.Black;
            skillTurnUi.text = "黒が選ぶターン";
            count++;
            if (count >= 3) {
                SceneManager.LoadScene("mainScene");
            }
        }
    }
}
