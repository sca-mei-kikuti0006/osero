using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCon : MonoBehaviour
{
    [SerializeField] private GameObject piece;
    [SerializeField] private GameObject show;//�u����ꏊ

    private enum turnBW
    {
        Black = 0,
        White = 180, //piece���Ђ�����Ԃ��̂�180
        Not = 2
    }
    private turnBW turn = turnBW.Black;//�ǂ����̃^�[����
    private turnBW notTurn = turnBW.White;//�^�[������Ȃ���

    //�����邩
    bool canCrick = true;

    //�Ֆʃf�[�^
    private turnBW[,] board = new turnBW[8, 8];
    //��f�[�^
    private GameObject[,] pieceBox = new GameObject[8, 8];
    //�u����Ƃ�
    private bool[,] canOver = new bool[8, 8];
    private GameObject[,] overBox = new GameObject[8, 8]; //�u����ꏊ�ɐݒu����ԃ}�[�N

    private bool canPut = false;//���݂��u���鏊�����邩
    private int end = 0;//�I���

    //�Ђ�����Ԃ���X�g
    List<int> overListX = new List<int>();
    List<int> overListZ = new List<int>();
    public List<int> OverListX { //�ύX(�y��)
        get { return overListX;}
    }
    public List<int> OverListZ {
        get { return overListZ;}
    }

    //��]�p�}�l�[�W���[
    [SerializeField] RotationManager rm;

    //��̐�UI
    [SerializeField] private Text BUi;
    [SerializeField] private Text WUi;
    private int countB = 0;
    private int countW = 0;

    //�X�L���f�[�^
    private enum skill {
        Mg1 = 1,
        Mg2,
        Mg3,
        TrSetb,
        Tr2,
        Tr3,
        Not
    }
    private skill[] skillB = new skill[3];
    private bool[] skillBUse = new bool[3];
    private skill[] skillW = new skill[3];
    private bool[] skillWUse = new bool[3];
    //�g���b�v�f�[�^
    private skill[,] trap = new skill[8, 8];
    private GameObject[,] trapOb = new GameObject[8, 8];

    //�^�[���̕��̐Fui(�m�F�p)
    [SerializeField] private Image CUi;

    // Start is called before the first frame update
    void Start()
    {
        //�Ֆʃf�[�^����
        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((i == 3 && j == 4) || (i == 4 && j == 3)){ //������
                    board[i, j] = turnBW.Black;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 0));

                }else if ((i == 3 && j == 3) || (i == 4 && j == 4)){//������
                    board[i, j] = turnBW.White;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 180));
                
                }else{                      //�����z�u�ȊO�͋�
                    board[i, j] = turnBW.Not;
                }

            }
        }

        //�g���b�v�f�[�^������
        for(int i = 0;i < 8; i++) {
            for(int j = 0;j < 8;j++){
                trap[i,j] = skill.Not;
            }
        }

        CanOver();
    }

    // Update is called once per frame
    void Update()
    {
        
        //�u����ꏊ���邩
        if (!canPut) {
            if (end == 2) {
                GameEnd();
            }
            else {
                TurnChange();
                end++;
            }
        }

        //��ݒu
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 13.0f)){
            if (Input.GetMouseButtonDown(0) && canCrick == true){
                canCrick = false;
                int z = (int)hit.transform.position.z * -1;
                int x = (int)hit.transform.position.x;

                if (canOver[z, x] == true){//��u����ꏊ��������
                    pieceBox[z, x] = Instantiate(piece, new Vector3(hit.transform.position.x, 0.07f, hit.transform.position.z), Quaternion.Euler(0, 0, (int)turn));
                    board[z, x] = turn;
                    TurnOver(z, x,true);
                    end = 0;
                    TurnChange();//�^�[�����
                }
            }
        }
    }
    void DebugBoard()
    {
        string str = "";
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if(pieceBox[x, y] == null) str += "O";
                else str += "X";
            }
            str += "\n";
        }
        Debug.Log(str);
    }

    //�^�[�����
    private void TurnChange()
    {
        if (turn == turnBW.Black){
            turn = turnBW.White;
            notTurn = turnBW.Black;

            //�m�F�p
            CUi.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f);

        }else if (turn == turnBW.White){
            turn = turnBW.Black;
            notTurn = turnBW.White;

            //�m�F�p
            CUi.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 255.0f);
        }

        CanOver();
        CountPiece();
    }

    private void CanOver() {//��̐ݒu���\��
        canOver = new bool[8, 8];
        canPut = false;

        foreach (GameObject show in overBox){
            Destroy(show);
        }

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if(board[i,j] == turnBW.Not) {
                    TurnOver(i, j, false);
                }else {
                    canOver[i,j] = false;
                }
                if (canOver[i, j]){
                    overBox[i,j] = Instantiate(show, new Vector3(j, 0.07f, -i), Quaternion.Euler(90.0f, 0, 0));
                    canPut = true;
                }
            }
        }
    }

    private void CountPiece() { //��̐�
        countB = 0;
        countW = 0;
        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if(board[i,j] == turnBW.Black) { 
                    countB++;
                }else if(board[i, j] == turnBW.White) { 
                    countW++;
                }
            }
        }
        BUi.text = string.Format("{00}", countB);
        WUi.text = string.Format("{00}", countW);

    }

    //�u���邩
    private void TurnOver(int Z,int X,bool over) {
        int[] _X = new int[] { -1, -1,  0,  1,  1,  1,  0, -1 };
        int[] _Z = new int[] {  0,  1,  1,  1,  0, -1, -1, -1 };//�΂̗�8����

        for (int i = 0;i < _X.Length; i++){
            int x = X;
            int z  = Z;
            int _x = _X[i];
            int _z = _Z[i];

            bool firstF = true;
            bool isOver = false;

            overListX = new List<int>();
            overListZ = new List<int>();

            while (!isOver){
                x += _x;
                z += _z;

                if (x < 0 || 7 < x || z < 0 || 7 < z) {
                    break;
                }

                if (firstF){
                    if(board[z,x] != notTurn) {
                        break;
                    }
                    firstF = false;
                }

                if (board[z,x] == notTurn) {
                    overListX.Add(x);
                    overListZ.Add(z);

                }
                else if(board[z,x] == turn) {
                    canOver[Z,X] = true;
                    if(over == true) StartCoroutine(OverPiece(overListX, overListZ));
                    isOver = true;

                }else {
                    break;
                }
            }
        }

    }

    ////���Ԃ�
    //private void OverPiece(List<int> listX,List<int> listZ){
    //    int x,z;
    //    for(int i = 0; i < listX.Count; i++) { 
    //        x = listX[i];
    //        z = listZ[i];
    //        board[z,x] = turn;
    //        //�ύX(�y��)
    //        //pieceBox[z, x].transform.Rotate(new Vector3(0, 0, 180)); 
    //        DebugBoard();
            
    //    }
    //    StartCoroutine(rm.StartContinuousTurn(listX, listZ, pieceBox));//�A����]
    //}

    //���Ԃ�
    private IEnumerator OverPiece(List<int> listX, List<int> listZ)
    {
        int x, z;
        for (int i = 0; i < listX.Count; i++)
        {
            x = listX[i];
            z = listZ[i];
            board[z, x] = turn;
            //�ύX(�y��)
            //pieceBox[z, x].transform.Rotate(new Vector3(0, 0, 180)); 
            DebugBoard();

        }
        yield return StartCoroutine(rm.StartContinuousTurn(listX, listZ, pieceBox));//�A����]
        canCrick = true;
    }
    private void GameEnd() {
        Debug.Log("end");
        Application.Quit();
    }

    //�X�L��
    private void Skill(skill s) {
        switch (s) {
            case skill.Mg1:
                break;
            case skill.Mg2:
                break;
            case skill.Mg3:
                break;
            case skill.TrSetb:
                SkillTrapSetback();
                break;
            case skill.Tr2:
                break;
            case skill.Tr3:
                break;
            default:
                break;
        }
    }
    //�g���b�v
    private void SkillTrap() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 13.0f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                int z = (int)hit.transform.position.z * -1;
                int x = (int)hit.transform.position.x;

                if (board[z,x] == turnBW.Not)
                {
                }
            }
        }
    }
    //�Z�b�g�o�b�N�i�g���b�v������ꏊ�ɒu���Ɩ����ɂȂ�j
    private void SkillTrapSetback() {
        
    }

    
}
