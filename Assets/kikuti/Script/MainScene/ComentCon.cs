using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComentCon : MonoBehaviour
{
    private SkillPlayCon skillCon;

    [SerializeField] private Image skillComent;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("GameManager");
        skillCon = obj.GetComponent<SkillPlayCon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickComentOn()
    {
        if (skillCon.SkillPlayOn())
        {
            skillComent.gameObject.SetActive(false);
        }
    }

    public void OnClickComentOff()
    {
        skillComent.gameObject.SetActive(false);
    }
}
