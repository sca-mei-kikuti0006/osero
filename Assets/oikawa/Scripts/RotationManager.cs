using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    //[SerializeField] List<PieceRotation> pieces; //�I�Z���̃s�[�X
    List<PieceRotation> pieces; //�I�Z���̃s�[�X
    PieceRotation p;

    void Start()
    {
        pieces = new List<PieceRotation>(16);//�I�Z���̃s�[�X������
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
