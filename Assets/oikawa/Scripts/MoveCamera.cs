using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private GameObject mainCamera; //メインカメラ格納
    private GameObject subCamera; //サブカメラ格納

    void Start()
    {
        //メインカメラとサブカメラをそれぞれ取得
        mainCamera = GameObject.Find("Main Camera");
        subCamera = GameObject.Find("Sub Camera");

        //サブカメラ切る
        subCamera.SetActive(false);
    }

    void Update()
    {
        //スペース押している間、カメラ切り替わり
        if (Input.GetKey("space"))
        {
            //サブカメラ動かす
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
        }
        else
        {
            //メインカメラ動かす
            subCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}
