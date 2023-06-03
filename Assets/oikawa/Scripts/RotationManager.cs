using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    List<PieceRotation> pieces; //オセロのピース

    void Start()
    {
        pieces = new List<PieceRotation>(16);//オセロのピースを入れる

        for (int i = 0; i < 16; ++i)
        {
            pieces[i] = new PieceRotation();
            pieces[i].row = i;
            pieces[i].col = i;
        }

    }
    void Update()
    {
        //Aが押されたら（左回転）＝colが3のやつのみひっくり返す
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 16; ++i)
            {
                if (pieces[i].col == 3)
                {
                    pieces[i].StartToss(PieceRotation.RotationDirection.Left);
                }
            }
        }
    }
}
