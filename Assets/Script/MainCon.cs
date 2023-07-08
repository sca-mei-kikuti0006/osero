using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCon : MonoBehaviour
{
    [SerializeField] private GameObject piece;
    [SerializeField] private GameObject show;//置ける場所

    private enum turnBW
    {
        Black = 0,
        White = 180, //pieceをひっくり返すので180
        Not = 2
    }
    private turnBW turn = turnBW.Black;//どっちのターンか
    private turnBW notTurn = turnBW.White;//ターンじゃない方

    //おけるか
    bool canCrick = true;

    //盤面データ
    private turnBW[,] board = new turnBW[8, 8];
    //駒データ
    private GameObject[,] pieceBox = new GameObject[8, 8];
    //置けるとこ
    private bool[,] canOver = new bool[8, 8];
    private GameObject[,] overBox = new GameObject[8, 8]; //置ける場所に設置する赤マーク

    private bool canPut = false;//お互い置ける所があるか
    private int end = 0;//終わる

    //ひっくり返す駒リスト
    List<int> overListX = new List<int>();
    List<int> overListZ = new List<int>();
    public List<int> OverListX { //変更(及川)
        get { return overListX;}
    }
    public List<int> OverListZ {
        get { return overListZ;}
    }

    //回転用マネージャー
    [SerializeField] RotationManager rm;

    //駒の数UI
    [SerializeField] private Text BUi;
    [SerializeField] private Text WUi;
    private int countB = 0;
    private int countW = 0;

    //スキルデータ
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
    //トラップデータ
    private skill[,] trap = new skill[8, 8];
    private GameObject[,] trapOb = new GameObject[8, 8];

    //ターンの方の色ui(確認用)
    [SerializeField] private Image CUi;

    // Start is called before the first frame update
    void Start()
    {
        //盤面データ初期
        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((i == 3 && j == 4) || (i == 4 && j == 3)){ //初期黒
                    board[i, j] = turnBW.Black;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 0));

                }else if ((i == 3 && j == 3) || (i == 4 && j == 4)){//初期白
                    board[i, j] = turnBW.White;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 180));
                
                }else{                      //初期配置以外は空
                    board[i, j] = turnBW.Not;
                }

            }
        }

        //トラップデータ初期化
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
        
        //置ける場所あるか
        if (!canPut) {
            if (end == 2) {
                GameEnd();
            }
            else {
                TurnChange();
                end++;
            }
        }

        //駒設置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 13.0f)){
            if (Input.GetMouseButtonDown(0) && canCrick == true){
                canCrick = false;
                int z = (int)hit.transform.position.z * -1;
                int x = (int)hit.transform.position.x;

                if (canOver[z, x] == true){//駒が置ける場所だったら
                    pieceBox[z, x] = Instantiate(piece, new Vector3(hit.transform.position.x, 0.07f, hit.transform.position.z), Quaternion.Euler(0, 0, (int)turn));
                    board[z, x] = turn;
                    TurnOver(z, x,true);
                    end = 0;
                    TurnChange();//ターン交代
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

    //ターン交代
    private void TurnChange()
    {
        if (turn == turnBW.Black){
            turn = turnBW.White;
            notTurn = turnBW.Black;

            //確認用
            CUi.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f);

        }else if (turn == turnBW.White){
            turn = turnBW.Black;
            notTurn = turnBW.White;

            //確認用
            CUi.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 255.0f);
        }

        CanOver();
        CountPiece();
    }

    private void CanOver() {//駒の設置が可能か
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

    private void CountPiece() { //駒の数
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

    //置けるか
    private void TurnOver(int Z,int X,bool over) {
        int[] _X = new int[] { -1, -1,  0,  1,  1,  1,  0, -1 };
        int[] _Z = new int[] {  0,  1,  1,  1,  0, -1, -1, -1 };//石の隣8方向

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

    ////裏返す
    //private void OverPiece(List<int> listX,List<int> listZ){
    //    int x,z;
    //    for(int i = 0; i < listX.Count; i++) { 
    //        x = listX[i];
    //        z = listZ[i];
    //        board[z,x] = turn;
    //        //変更(及川)
    //        //pieceBox[z, x].transform.Rotate(new Vector3(0, 0, 180)); 
    //        DebugBoard();
            
    //    }
    //    StartCoroutine(rm.StartContinuousTurn(listX, listZ, pieceBox));//連続回転
    //}

    //裏返す
    private IEnumerator OverPiece(List<int> listX, List<int> listZ)
    {
        int x, z;
        for (int i = 0; i < listX.Count; i++)
        {
            x = listX[i];
            z = listZ[i];
            board[z, x] = turn;
            //変更(及川)
            //pieceBox[z, x].transform.Rotate(new Vector3(0, 0, 180)); 
            DebugBoard();

        }
        yield return StartCoroutine(rm.StartContinuousTurn(listX, listZ, pieceBox));//連続回転
        canCrick = true;
    }
    private void GameEnd() {
        Debug.Log("end");
        Application.Quit();
    }

    //スキル
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
    //トラップ
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
    //セットバック（トラップがある場所に置くと無効になる）
    private void SkillTrapSetback() {
        
    }

    
}
