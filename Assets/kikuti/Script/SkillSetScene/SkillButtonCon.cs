using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButtonCon : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private SkillSetCon setCon;

    [SerializeField] private Image skillComent;

    [SerializeField] private Image coverB;
    [SerializeField] private Image coverW;

    private bool can = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("SkillSetManager");
        setCon = obj.GetComponent<SkillSetCon>();

        skillComent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (can)
        {
            if (setCon.SkillTurn == MainCon.turnBW.Black)
            {
                Instantiate(coverB, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform);
            }
            else
            {
                Instantiate(coverW, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform);
            }
            string skillName = this.gameObject.name;
            setCon.SkillSet(skillName);
            can = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillComent.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        skillComent.gameObject.SetActive(false);
    }
}
