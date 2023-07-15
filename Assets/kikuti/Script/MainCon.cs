using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�I�Z���݂̂̋@�\�̃X�N���v�g

public class MainCon : MonoBehaviour
{
    [SerializeField] private GameObject piece;
    [SerializeField] private GameObject canShow;//�u����ꏊ
    [SerializeField] private GameObject trapShow;//�g���b�v�u�����ꏊ

    private enum turnBW
    {
        Black = 0,
        White = 180, //piece���Ђ�����Ԃ��̂�180
        Not = 2
    }

    private turnBW turn = turnBW.Black;//�ǂ����̃^�[����
    private turnBW notTurn = turnBW.White;//�^�[������Ȃ���

    //�Ֆʃf�[�^
    private turnBW[,] picecBoard = new turnBW[8, 8];
    //��f�[�^
    private GameObject[,] pieceBox = new GameObject[8, 8];

    //�u����Ƃ�
    private bool[,] canBoard = new bool[8, 8];
    private GameObject[,] canBox = new GameObject[8, 8]; //�u����ꏊ�ɐݒu����ԃ}�[�N

    private bool canPut = false;//���݂��u���鏊�����邩
    private bool end = false;//�I���

    //�Ђ�����Ԃ���X�g
    List<int> overListX = new List<int>();
    List<int> overListZ = new List<int>();

    //��̐�
    private int countB = 0;
    public int CountB{ //UiCon�ɓn��
        get { return this.countB; }
    }
    private int countW = 0;
    public int CountW { //UiCon�ɓn��
        get { return this.countW; }
    }

    //�X�L���f�[�^
    private bool skillOn = false;
    public bool SkillOn{ //SkillButtonReturn�ɓn��
        get { return this.skillOn; }
    }
    private bool skillOnTrap = false;
    public bool SkillOnTrap { //OnCursorCon�ɓn��
        get { return this.skillOnTrap; }}
    private bool skillOnMagic = false;
    public bool SkillOnMagic { //OnCursorCon�ɓn��
        get { return this.skillOnMagic; } }

    private enum skill
    {
        MgTenm = 1,
        MgStorm,
        MgRever,
        TrSetb,
        TrLand,
        TrLight,
        Not
    }

    private skill playSkill = skill.Not;
    //private skill[] skillB = new skill[3];
    private skill[] skillB = { skill.MgTenm, skill.MgStorm, skill.MgRever };//����
    private bool[] skillBUse = new bool[3];
    //private skill[] skillW = new skill[3];
    private skill[] skillW = { skill.TrSetb, skill.TrLand, skill.TrLight };//����
    private bool[] skillWUse = new bool[3];

    //�g���b�v�f�[�^
    private skill[,] trapBoard = new skill[8, 8];
    private GameObject[,] trapBox = new GameObject[8,8];

    //�𗋐j�p
    private bool trLightPlay = false;
    private turnBW trLightBW = turnBW.Not;
    private int trLightX = 0, trLightZ = 0;

    //���̌아�p
    private bool mgStormPlay = false;
    private turnBW mgStormBW = turnBW.Not;
    private bool mgStormMg = false;//�}�W�b�N�ɔ�����I�񂾂�

    private UiCon uiCon;

    // Start is called before the first frame update
    void Start()
    {
        uiCon = GetComponent<UiCon>();

        //�Ֆʃf�[�^����
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i == 3 && j == 4) || (i == 4 && j == 3))
                { //������
                    picecBoard[i, j] = turnBW.Black;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 0));

                }
                else if ((i == 3 && j == 3) || (i == 4 && j == 4))
                {//������
                    picecBoard[i, j] = turnBW.White;
                    pieceBox[i, j] = Instantiate(piece, new Vector3(j, 0.07f, -i), Quaternion.Euler(0, 0, 180));

                }
                else
                {                      //�����z�u�ȊO�͋�
                    picecBoard[i, j] = turnBW.Not;

                }

            }
        }

        //�g���b�v�f�[�^������
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
        //��ݒu
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 13.0f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                int z = (int)hit.transform.position.z * -1;
                int x = (int)hit.transform.position.x;

                if (skillOnTrap == true)
                { //�g���b�v�ݒu
                    PutSkillTrap(z, x, hit);
                }
                else if (skillOnMagic == true) {//�}�W�b�N�I��
                   PutSkillMagic(z,x);
                }
                else
                {//��ݒu
                    PutPiece(z, x, hit);
                }
            }
        }

        //�u����ꏊ���邩
        if (!canPut)
        {
            if (end)
            {//���݂������Ȃ��Ȃ�����I��
                GameEnd();
            }
            else
            {
                TurnChange();
                end = true;
            }
        }

    }

    //�g���b�v�ݒu
    private void PutSkillTrap(int z, int x, RaycastHit hit)
    {
        Debug.Log(playSkill);
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

    //�}�W�b�N�I��
    private void PutSkillMagic(int z, int x) { 
        if(playSkill == skill.MgRever) {
            SkillMagicReversal(x);
        }
        else if (playSkill == skill.MgStorm) {
            SkillMagicStormTr(z,x);
        }
    }

    //��ݒu
    private void PutPiece(int z, int x, RaycastHit hit)
    {

        if (canBoard[z, x] == true && skillOn == false)
        {
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
            TurnChange();//�^�[�����
        }
    }

    //�^�[�����
    private void TurnChange()
    {
        if (turn == turnBW.Black)//�����甒
        {
            turn = turnBW.White;
            notTurn = turnBW.Black;

            uiCon.TurnChangeUiW();
        }
        else if (turn == turnBW.White)//�����獕
        {
            turn = turnBW.Black;
            notTurn = turnBW.White;

            uiCon.TurnChangeUiB();
        }

        CountPiece();
        CanPut();
    }

    //��̐ݒu���\��
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

    //��̐�
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

    //���Ԃ錟��
    private void SearchTurnOver(int Z, int X, bool over)
    {
        int[] _X = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
        int[] _Z = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };//�΂̗�8����

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
                    if (over == true) OverPiece(overListX, overListZ);
                    isOver = true;

                }
                else
                {
                    break;
                }
            }
        }

    }

    //���Ԃ�
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

    //�Q�[���I��
    private void GameEnd()
    {
        Debug.Log("end");
        Application.Quit();
    }

    //�X�L������
    public void SkillPlay(string skillBW,int skillNumber)
    {
        skillOn = true;

        if (skillBW == "B"){
            playSkill = skillB[skillNumber-1];
        }
        else if (skillBW == "W"){
            playSkill = skillW[skillNumber-1];
        }

        switch (playSkill)
        {
            case skill.MgTenm:
                if (mgStormPlay&&mgStormBW == notTurn){
                    Debug.Log("�g���܂���");
                    mgStormPlay = false;
                    skillOn = false;
                }
                else if (SearchSkill(2)){//�p���Q�ȏ����Ă��Ȃ��Ɣ����o���Ȃ�
                    SkillMagicTenma();
                }
                else{
                    Debug.Log("�g���܂���");
                    skillOn = false;
                }
                break;
            case skill.MgStorm:
                mgStormBW = turn;
                uiCon.SelectUiT();
                break;
            case skill.MgRever:
                if (mgStormPlay && mgStormBW == notTurn)
                {
                    Debug.Log("�g���܂���");
                    mgStormPlay = false;
                    skillOn = false;
                }
                else if (SearchSkill(1)){ //�p���P�ȏ����Ă��Ȃ��Ɣ����o���Ȃ�
                    skillOnMagic = true;
                }
                else { 
                    Debug.Log("�g���܂���");
                    skillOn = false;
                }
                break;
            case skill.TrSetb:
            case skill.TrLand:
                skillOnTrap = true;
                break;
            case skill.TrLight:
                trLightPlay = true;
                trLightBW = turn;
                skillOnTrap = true;
                break;
            default:
                break;
        }
    }

    //�X�L��������������
    private bool SearchSkill(int corner) {
        for(int z = 0; z < 8; z += 7) {
            for (int x = 0; x < 8; x += 7)
            {
                if (picecBoard[z,x] == notTurn) { 
                    corner--;       
                }
            }
        }

        if(corner == 0) {
            return true;
        }
        else { 
            return false;
        }
    }

    //�g���b�v�̃X�L���֐��Ăяo��
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

    //�X�L���֐�������
    //�}�W�b�N�V���̃T�C�R��
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
            for (int c = 0; c < zList.Count; c++)
            {
                picecBoard[zList[c], xList[c]] = turn;
                pieceBox[zList[c], xList[c]].transform.Rotate(new Vector3(0, 0, 180));
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
                    pieceBox[raZ, raX].transform.Rotate(new Vector3(0, 0, 180));
                    count++;
                }
            }
        }

        skillOn = false;
    }

    //�}�W�b�N���̌아
    //�I����
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
    //���̌아�g���b�v�ɔ���
    private void SkillMagicStormTr(int z,int x){
        skillOnMagic = false;
        if (trapBoard[z,x] != skill.Not) { 
            trapBoard[z,x] = skill.Not;
            Destroy(trapBox[z, x]);
            skillOn = false;
        }
    }

    //�}�W�b�N�t�]�ւ̕z��
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
                }
            }
        }

        skillOn = false;
    }

    //�g���b�v��u�����ꏊ�ɋ��u�������̊֐�����
    //�g���b�v�Z�b�g�o�b�N
    private void SkillTrapSetback(int z,int x) {
        Destroy(pieceBox[z, x]);
        Destroy(trapBox[z, x]);
        trapBoard[z,x] = skill.Not;
        skillOn = false;

    }

    //�g���b�v�n��
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
        skillOn = false;

    }

    //�g���b�v�𗋐j
    private void SkillTrapLightningrod(int z, int x)
    {
        Debug.Log("ok");
        Destroy(pieceBox[z, x]);
        pieceBox[trLightZ, trLightX] = Instantiate(piece, new Vector3(trLightX, 0.07f, trLightZ * -1), Quaternion.Euler(0, 0, (int)turn));
        picecBoard[trLightZ, trLightX] = turn;
        SearchTurnOver(trLightZ, trLightX, true);
        Destroy(trapBox[z, x]);
        trapBoard[z, x] = skill.Not;
        trLightPlay = false;
        skillOn = false;

    }

}