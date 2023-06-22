using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    [SerializeField] List<PieceRotation> pieces; //オセロのピース
    //List<PieceRotation> pieces; //オセロのピース
    PieceRotation p;

    void Start()
    {
       //pieces = new List<PieceRotation>(16);//オセロのピースを入れる
    }


    void Update()
    {
    }
    public IEnumerator StartContinuousTurn(List<int> listX, List<int> listZ, GameObject[,] pieceBox) {
        int x, z;
        for (int i = 0; i < listX.Count; i++)
        {
            x = listX[i];
            z = listZ[i];
            pieceBox[z,x].GetComponent<PieceRotation>().StartToss(PieceRotation.RotationDirection.Left);
            yield return new WaitForSeconds(1);
        }      
    }
}
