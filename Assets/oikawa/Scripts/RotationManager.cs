using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    List<PieceRotation> pieces; //�I�Z���̃s�[�X

    void Start()
    {
        pieces = new List<PieceRotation>(16);//�I�Z���̃s�[�X������

        for (int i = 0; i < 16; ++i)
        {
            pieces[i] = new PieceRotation();
            pieces[i].row = i;
            pieces[i].col = i;
        }

    }
    void Update()
    {
        //A�������ꂽ��i����]�j��col��3�̂�݂̂Ђ�����Ԃ�
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
