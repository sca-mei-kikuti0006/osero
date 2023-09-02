using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//オセロのみの機能のスクリプト

public class MainCon : MonoBehaviour
{
    [SerializeField] private GameObject piece;
    [SerializeField] private GameObject canShow;//置ける場所
    [SerializeField] private GameObject trapShow;//トラップ置いた場所

    public enum turnBW
    {
        Black = 0,
        White = 180, //pieceをひっくり返すので180
        Not = 2
    }

    private turnBW turn = turnBW.Black;//どっちのターンか
    private turnBW notTurn = turnBW.White;//ターンじゃない方

    //クリック判定用
    bool canCrick = true; //変更
    public bool CanCrick
    {
        get { return canCrick; }
    }

    //盤面データ
    private turnBW[,] picecBoard = new turnBW[8, 8];
    //駒データ
    private GameObject[,] pieceBox = new GameObject[8, 8];

    //置けるとこ
    private bool[,] canBoard = new bool[8, 8];
    private GameObject[,] canBox = new GameObject[8, 8]; //置ける場所に設置する赤マーク

    private bool canPut = false;//お互い置ける所があるか
    private bool end = false;//終わる

    //ひっくり返す駒リスト
    List<int> overListX = new List<int>();
    List<int> overListZ = new List<int>();

    //変更(及川)
    public List<int> OverListX
    { 
        get { return overListX; }
    }
    public List<int> OverListZ
    {
        get { return overListZ; }
    }

    //回転用マネージャー
    [SerializeField] RotationManager rm;

    //駒の数
    public static int countB = 0;
    public static int countW = 0;

    //スキルデータ
    private bool skillOn = false;
    public bool SkillOn{ //SkillButtonReturnに渡す
        get { return this.skillOn; }
    }
    private bool skillOnTrap = false;
    public bool SkillOnTrap { //OnCursorConに渡す
        get { return this.skillOnTrap; }}
    private bool skillOnMagic = false;
    public bool SkillOnMagic { //OnCursorConに渡す
        get { return this.skillOnMagic; } }

    public enum skill
    {
        MgTenm = 1,
        MgStorm,
        MgRever,
        TrSetb,
        TrLand,
        TrLight,
        Not
    }

    private bool canSkill = true;
    private skill playSkill = skill.Not;
    public static skill[] skillB = new skill[3];
    public static skill[] skillW = new skill[3];
    public bool[] canSkillB = { true, true, true };
    public bool[] canSkillW = { true, true, true };

    //トラップデータ
    private skill[,] trapBoard = new skill[8, 8];
    private GameObject[,] trapBox = new GameObject[8,8];

    //避雷針用
    private bool trLightPlay = false;
    private turnBW trLightBW = turnBW.Not;
    private int trLightX = 0, trLightZ = 0;

    //嵐の護符用
    private bool mgStormPlay = false;
    private turnBW mgStormBW = turnBW.Not;
    private bool mgStormMg = false;//マジックに発動を選んだか

    private UiCon uiCon;

    //エフェクト用(テスト中)
    [SerializeField] GameObject storm;
    /*
    public static GameObject BomEfect = GameObject.Find("CFX_Explosion_B_Smoke+Text");
    public static GameObject ThunderEfect = GameObject.Find("CFX3_Hit_Electric_A_Ground");
    public static GameObject HitEfecte = GameObject.Find("CFX_Hit_C White");

    public static bool BomEfectSet = false;
    public static bool ThunderEfectSet = false;
    public static bool HitEfectSet = false;
    */

    // Start is called before the first frame update
    void Start()
    {
        uiCon = GetComponent<UiCon>();

        //盤面データ初期
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i == 3 && j == 4) || (i == 4 && j == 3))
                { //初期黒
                    picecBoard[i, j] = turnBW.Black;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 0));

                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4))
                {//初期白
                    picecBoard[i, j] = turnBW.White;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 180));

                }
                else
                {                      //初期配置以外は空
                    picecBoard[i, j] = turnBW.Not;

                }

            }
        }

        //トラップデータ初期化
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                trapBoard[i, j] = skill.Not;
            }
        }

        CanPut();
    }

    // Update is called once per frame
    void Update()
    {
        //駒設置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 13.0f))
        {
            if (Input.GetMouseButtonDown(0) && canCrick == true) //変更
            {
                int z = (int)hit.transform.position.z * -1;
                int x = (int)hit.transform.position.x;

                if (skillOnTrap == true)
                { //トラップ設置
                    PutSkillTrap(z, x, hit);
                }
                else if (skillOnMagic == true) {//マジック選択
                    PutSkillMagic(z,x);
                }
                else
                {//駒設置
                    PutPiece(z, x, hit);
                }
            }
        }

        //置ける場所あるか
        if (!canPut)
        {
            if (end)
            {//お互いおけなくなったら終了
                GameEnd();
            }
            else
            {
                TurnChange();
                end = true;
            }
        }
    }

    //トラップ設置
    private void PutSkillTrap(int z, int x, RaycastHit hit)
    {
        if (picecBoard[z, x] == turnBW.Not && trapBoard[z, x] == skill.Not)
        {
            trapBox[z,x] = Instantiate(trapShow, new Vector3(hit.transform.position.x, 0.07f, hit.transform.position.z), Quaternion.Euler(90.0f, 0, 0));
            trapBoard[z, x] = playSkill;
            if(playSkill == skill.TrLight) {
                trLightZ = z;
                trLightX = x;
            }
            skillOnTrap = false;
            skillOn = false;
        }
    }

    //マジック選択
    private void PutSkillMagic(int z, int x) { 
        if(playSkill == skill.MgRever) {
            SkillMagicReversal(x);
        }
        else if (playSkill == skill.MgStorm) {
            SkillMagicStormTr(z,x);
        }
    }

    //駒設置
    private void PutPiece(int z, int x, RaycastHit hit)
    {

        if (canBoard[z, x] == true && skillOn == false)
        {
            canCrick = false;
            pieceBox[z, x] = Instantiate(piece, new Vector3(hit.transform.position.x, 0.07f, hit.transform.position.z), Quaternion.Euler(0, 0, (int)turn));
            if (trLightPlay == true && turn != trLightBW) {
                skillOn = true;
                SkillTrapPlay(skill.TrLight, z, x);
            }
            else if(trapBoard[z,x] != skill.Not) {
                skillOn = true;
                SkillTrapPlay(trapBoard[z, x], z, x);
            }
            else {
                picecBoard[z, x] = turn;
                SearchTurnOver(z, x, true);
            }
            end = false;
            TurnChange();//ターン交代
        }
    }

    //ターン交代
    private void TurnChange()
    {
        canSkill = true;
        if (turn == turnBW.Black)//黒から白
        {
            turn = turnBW.White;
            notTurn = turnBW.Black;

            uiCon.TurnChangeUiW();
            uiCon.ImageT(turn);
        }
        else if (turn == turnBW.White)//白から黒
        {
            turn = turnBW.Black;
            notTurn = turnBW.White;

            uiCon.TurnChangeUiB();
            uiCon.ImageT(turn);
        }

        CountPiece();
        CanPut();
    }

    //駒の設置が可能か
    private void CanPut()
    {
        canBoard = new bool[8, 8];
        canPut = false;

        foreach (GameObject show in canBox)
        {
            Destroy(show);
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (picecBoard[i, j] == turnBW.Not)
                {
                    SearchTurnOver(i, j, false);
                }
                else
                {
                    canBoard[i, j] = false;
                }
                if (canBoard[i, j])
                {
                    canBox[i, j] = Instantiate(canShow, new Vector3(j, 0.07f, -i), Quaternion.Euler(90.0f, 0, 0));
                    canPut = true;
                }
            }
        }
    }

    //駒の数
    private void CountPiece()
    { 
        countB = 0;
        countW = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (picecBoard[i, j] == turnBW.Black)
                {
                    countB++;
                }
                else if (picecBoard[i, j] == turnBW.White)
                {
                    countW++;
                }
            }
        }
        
        uiCon.CountPieceUi();
    }

    //裏返る検索
    private void SearchTurnOver(int Z, int X, bool over)
    {
        int[] _X = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
        int[] _Z = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };//石の隣8方向

        for (int i = 0; i < _X.Length; i++)
        {
            int x = X;
            int z = Z;
            int _x = _X[i];
            int _z = _Z[i];

            bool firstF = true;
            bool isOver = false;

            overListX = new List<int>();
            overListZ = new List<int>();

            while (!isOver)
            {
                x += _x;
                z += _z;

                if (x < 0 || 7 < x || z < 0 || 7 < z)
                {
                    break;
                }

                if (firstF)
                {
                    if (picecBoard[z, x] != notTurn)
                    {
                        break;
                    }
                    firstF = false;
                }

                if (picecBoard[z, x] == notTurn)
                {
                    overListX.Add(x);
                    overListZ.Add(z);

                }
                else if (picecBoard[z, x] == turn)
                {
                    canBoard[Z, X] = true;
                    if (over == true) StartCoroutine(OverPiece(overListX, overListZ));
                    isOver = true;

                }
                else
                {
                    break;
                }
            }
        }

    }

    /*
    //裏返す
    private void OverPiece(List<int> listX, List<int> listZ)
    {
        int x, z;
        for (int i = 0; i < listX.Count; i++)
        {
            x = listX[i];
            z = listZ[i];
            picecBoard[z, x] = turn;
            pieceBox[z, x].transform.Rotate(new Vector3(0, 0, 180));
        }
    }
    */

    //裏返す
    private IEnumerator OverPiece(List<int> listX, List<int> listZ)
    {
        int x, z;
        for (int i = 0; i < listX.Count; i++)
        {
            x = listX[i];
            z = listZ[i];
            picecBoard[z, x] = turn;
        }
        yield return StartCoroutine(rm.StartContinuousTurn(listX, listZ, pieceBox));//連続回転
        StartCoroutine(WaitTime()); //待ち時間追加
        canCrick = true;
    }

    //待ち時間
    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1);
    }

    //ゲーム終了
    private void GameEnd()
    {
        Debug.Log("end");
        SceneManager.LoadScene("resultScene");
    }

    //スキル発動できるか
    public bool SearchSkillUi(skill skill,turnBW skillBW,int number) {
        if(skillBW != turn || !canSkill) {
            return false;
        }

        if(skillBW == turnBW.Black) {
            if (!canSkillB[number]) { 
                return false;
                }
        }
        else if (skillBW == turnBW.White)
        {
            if (!canSkillW[number])
            {
                return false;
            }
        }

        switch (skill)
        {
            case skill.MgTenm:
                if (!SearchSkill(2)) { 
                    return false;
                }
                break;
            case skill.MgStorm:
                break;
            case skill.MgRever:
                if (!SearchSkill(1)){
                    return false;
                }
                break;
            case skill.TrSetb:
            case skill.TrLand:
            case skill.TrLight:
            default:
                break;
        }
        return true;

    }

    //スキル発動
    public bool SkillPlay(skill skill, string skillBW)
    {
        bool play = false;

        Debug.Log(skillBW + " " + turn);
        if (((skillBW == "B" && turn == turnBW.Black) || (skillBW == "W" && turn == turnBW.White)) && canSkill)
        {
            playSkill = skill;
            skillOn = true;
            switch (playSkill)
            {
                case skill.MgTenm:
                    if (mgStormPlay && mgStormBW == notTurn)
                    {
                        Debug.Log("使えません");
                        mgStormPlay = false;
                        skillOn = false;
                    }
                    else if (SearchSkill(2))
                    {//角を２つ以上取られていないと発動出来ない
                        SkillMagicTenma();
                        play = true;
                    }
                    else
                    {
                        Debug.Log("使えません");
                        skillOn = false;
                    }
                    break;
                case skill.MgStorm:
                    mgStormBW = turn;
                    uiCon.SelectUiT();
                    play = true;
                    break;
                case skill.MgRever:
                    if (mgStormPlay && mgStormBW == notTurn)
                    {
                        Debug.Log("使えません");
                        mgStormPlay = false;
                        skillOn = false;
                    }
                    else if (SearchSkill(1))
                    { //角を１つ以上取られていないと発動出来ない
                        skillOnMagic = true;
                        play = true;
                    }
                    else
                    {
                        Debug.Log("使えません");
                        skillOn = false;
                    }
                    break;
                case skill.TrSetb:
                case skill.TrLand:
                    skillOnTrap = true;
                    play = true;
                    break;
                case skill.TrLight:
                    trLightPlay = true;
                    trLightBW = turn;
                    skillOnTrap = true;
                    play = true;
                    break;
                default:
                    break;
            }
        }

        if (play) {
            canSkill = false;
            uiCon.ImageS();
        }
        return play;
    }

    //スキル発動条件調べ
    private bool SearchSkill(int corner) {
        for(int z = 0; z < 8; z += 7) {
            for (int x = 0; x < 8; x += 7)
            {
                if (picecBoard[z,x] == notTurn) { 
                    corner--;       
                }
            }
        }

        if(corner <= 0) {
            return true;
        }
        else { 
            return false;
        }
    }

    //トラップのスキル関数呼び出し
    private void SkillTrapPlay(skill playSkill,int z,int x) {
        switch (playSkill)
        {
            case skill.TrSetb:
                SkillTrapSetback(z,x);
                break;
            case skill.TrLand:
                SkillTrapLandmine(z,x);
                break;
            case skill.TrLight:
                SkillTrapLightningrod(z,x);
                break;
            default:
                break;
        }
    }

    //スキル関数↓↓↓
    //マジック天魔のサイコロ
    private void SkillMagicTenma() {
        int ran = Random.Range(1,4);
        Debug.Log(ran);

        int pieceCount = 0;
        List<int> zList = new List<int>();
        List<int> xList = new List<int>();
        for (int z = 0; z < 8; z++){
            for (int x = 0; x < 8; x++){
                if (picecBoard[z, x] == notTurn)
                {
                    pieceCount++;
                    zList.Add(z);
                    xList.Add(x);
                }
            }
        }

        if (pieceCount <= ran)
        {
            ran = pieceCount;
            for (int c = 0; c < zList.Count; c++)
            {
                picecBoard[zList[c], xList[c]] = turn;
                StartCoroutine(PieceMagicTenma(zList[c], xList[c]));

                //及川エフェクト追加
            }
        }
        else
        {
            int count = 0;
            while (count < ran)
            {
                int raX = Random.Range(0, 8);
                int raZ = Random.Range(0, 8);
                if (picecBoard[raZ, raX] == notTurn)
                {
                    picecBoard[raZ, raX] = turn;
                    StartCoroutine(PieceMagicTenma(raZ, raX));

                    //及川エフェクト追加

                    count++;
                }
            }
        }

        uiCon.DiceUi(ran);

        CanPut();
        skillOn = false;
    }

    private IEnumerator PieceMagicTenma(int z,int x) {
        float move = pieceBox[z, x].transform.position.y;
        while (move < 0.5f)
        {
            move += Time.deltaTime * 4.0f;
            if (move > 0.5f)
            {
                move = 0.5f;
            }
            pieceBox[z, x].transform.position = new Vector3(pieceBox[z, x].transform.position.x, move, pieceBox[z, x].transform.position.z);
            yield return null;
        }

        move = 0;
        while (move < 180)
        {
            float mo = Time.deltaTime * 500.0f;
            move += mo;
            if (move > 180)
            {
                mo -= move - 180;
                move = 180;
            }
            pieceBox[z, x].transform.Rotate(new Vector3(0, 0, mo));
            yield return null;
        }

        move = pieceBox[z, x].transform.position.y;
        while (move > 0.07f)
        {
            move -= Time.deltaTime * 4.3f;
            if (move < 0.07f)
            {
                move = 0.07f;
            }
            pieceBox[z, x].transform.position = new Vector3(pieceBox[z, x].transform.position.x, move, pieceBox[z, x].transform.position.z);
            yield return null;
        }

    }

    //マジック嵐の護符
    //選択後
    public void SkillMagicStorm(bool select) {
        uiCon.SelectUiF();
        if (select) { 
            mgStormPlay = true;
            skillOn = false;
        }
        else {
            skillOnMagic = true;
        }
    }

    //トラップがあるか
    public bool SearchMagicStormTr() {
        for (int z = 0; z < 8; z++) {
            for (int x = 0; x < 8; x++)
            {
                if (trapBoard[z, x] != skill.Not)
                {
                    return true;
                }
            }
        }
        return false;
    }


    //嵐の護符トラップに発動
    private void SkillMagicStormTr(int z,int x){
        skillOnMagic = false;
        if (trapBoard[z,x] != skill.Not) { 
            trapBoard[z,x] = skill.Not;
            Destroy(trapBox[z, x]);

            //及川エフェクト追加
            GameObject PrefabStorm = Instantiate(storm);

            skillOn = false;
        }
    }

    //マジック逆転への布石
    private void SkillMagicReversal(int x) {
        skillOnMagic = false;

        int pieceCount = 0;
        List<int> zList = new List<int>();
        for (int z = 0; z < 8; z++){
            if(picecBoard[z,x] == notTurn) { 
                pieceCount++;
                zList.Add(z);
            }
        }

        if(pieceCount <= 3) { 
            for(int c = 0;c < zList.Count; c++) { 
                picecBoard[zList[c],x] = turn;
                pieceBox[zList[c], x].transform.Rotate(new Vector3(0, 0, 180));

                //及川エフェクト追加
            }
        }
        else {
            int count = 0;
            while (count < 3) { 
                int ran = Random.Range(0,8);
                if(picecBoard[ran,x] == notTurn) {
                    picecBoard[ran, x] = turn;
                    pieceBox[ran, x].transform.Rotate(new Vector3(0, 0, 180));
                    count++;

                    //及川エフェクト追加
                }
            }
        }

        CanPut();
        skillOn = false;
    }

    //トラップを置いた場所に駒を置いた時の関数↓↓
    //トラップセットバック
    private void SkillTrapSetback(int z,int x) {
        Destroy(pieceBox[z, x]);
        Destroy(trapBox[z, x]);

        //及川エフェクト追加

        trapBoard[z,x] = skill.Not;
        skillOn = false;
        canCrick = true;
    }

    //トラップ地雷
    private void SkillTrapLandmine(int z, int x)
    {
        for(int _z = z - 1; _z <= z + 1; _z++)
        {
            if(0 <= _z && _z <= 7) {
                for (int _x = x - 1; _x <= x + 1; _x++)
                {
                    if (0 <= _x && _x <= 7)
                    {
                        Destroy(pieceBox[_z, _x]);
                        picecBoard[_z, _x] = turnBW.Not;
                    }
                }
            }
        }
        Destroy(trapBox[z, x]);
        trapBoard[z, x] = skill.Not;

        //及川エフェクト追加

        skillOn = false;
        canCrick = true;
    }

    //トラップ避雷針
    private void SkillTrapLightningrod(int z, int x)
    {
        Destroy(pieceBox[z, x]);
        pieceBox[trLightZ, trLightX] = Instantiate(piece, new Vector3(trLightX, 0.07f, trLightZ * -1), Quaternion.Euler(0, 0, (int)turn));
        picecBoard[trLightZ, trLightX] = turn;
        SearchTurnOver(trLightZ, trLightX, true);
        Destroy(trapBox[z, x]);
        trapBoard[z, x] = skill.Not;

        //及川エフェクト追加

        trLightPlay = false;
        skillOn = false;
        canCrick = true;
    }

}