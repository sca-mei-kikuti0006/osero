using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private GameObject mainCamera; //���C���J�����i�[
    private GameObject subCamera; //�T�u�J�����i�[

    void Start()
    {
        //���C���J�����ƃT�u�J���������ꂼ��擾
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("Sub Camera");

        //�T�u�J�����؂�
        subCamera.SetActive(false);
    }

    void Update()
    {
        //�X�y�[�X�����Ă���ԁA�J�����؂�ւ��
        if (Input.GetKey("space"))
        {
            //�T�u�J����������
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
        }
        else
        {
            //���C���J����������
            subCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}
