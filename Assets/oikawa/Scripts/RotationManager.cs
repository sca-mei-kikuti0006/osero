using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    [SerializeField] List<PieceRotation> pieces; //�I�Z���̃s�[�X
    //List<PieceRotation> pieces; //�I�Z���̃s�[�X
    PieceRotation p;
    Queue<PieceRotation> pieceRotations = new Queue<PieceRotation>();
    void Start()
    {
        //pieces = new List<PieceRotation>(16);//�I�Z���̃s�[�X������
    }


    void Update()
    {
    }


    public IEnumerator StartContinuousTurn(List<int> listX, List<int> listZ, GameObject[,] pieceBox)
    {
        int x, z;
        for (int i = 0; i < listX.Count; i++)
        {
            x = listX[i];
            z = listZ[i];
            pieceRotations.Enqueue(pieceBox[z, x].GetComponent<PieceRotation>());
            Debug.Log("Enqueued");
           //yield return new WaitForSeconds(1);
        }
        int pieceCount = pieceRotations.Count;
        for (int i = 0; i < pieceCount; i++)
        {
            PieceRotation p = pieceRotations.Dequeue();
            yield return StartCoroutine(p.StartToss(PieceRotation.RotationDirection.Left));
        }

    }
}
