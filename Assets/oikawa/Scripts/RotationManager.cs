using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    public IEnumerator StartContinuousTurn(List<int> listX, List<int> listZ, GameObject[,] pieceBox)
    {
        int x, z;
        Queue<PieceRotation> pieceRotations = new Queue<PieceRotation>();
        for (int i = 0; i < listX.Count; i++)
        {
            x = listX[i];
            z = listZ[i];
            pieceRotations.Enqueue(pieceBox[z, x].GetComponent<PieceRotation>());
            Debug.Log("Enqueued");
        }
        int pieceCount = pieceRotations.Count;
        Debug.Log(pieceCount);
        for (int i = 0; i < pieceCount; i++)
        {
            PieceRotation p = pieceRotations.Dequeue();
            yield return StartCoroutine(p.StartToss(PieceRotation.RotationDirection.Left));
        }
    }
}