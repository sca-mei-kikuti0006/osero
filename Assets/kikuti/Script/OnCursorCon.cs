using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCursorCon : MonoBehaviour
{
    private MainCon mainCon;
    private bool on = false;
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

    private void OnMouseEnter()
    {
        if (mainCon.SkillOnTrap) { 
            this.gameObject.GetComponent<Renderer>().material.color += new Color(10, 0,0, 0.1f);
            on = true;
        }
    }
    private void OnMouseExit() {
        if (on){
            this.gameObject.GetComponent<Renderer>().material.color -= new Color(10,0,0,0.1f);
        }
    }
}
