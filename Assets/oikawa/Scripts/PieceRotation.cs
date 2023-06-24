using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PieceRotation : MonoBehaviour
{

    [SerializeField] private RotationDirection rotDir; //白か黒かの判別用
    private float x;
    private float y;
    [SerializeField] private float z = 100f;

    private float rotatedAngle; //度数法
    private Vector3 startRotation;//上がりきったところでの回転角保存用
    //private Coroutine currentCoroutine;//現在実行中のコルーチン
    Queue<Coroutine> currentCoroutine = new Queue<Coroutine>(); //キューを宣言

    [SerializeField] GameObject smoke;

    //黒か白か
    private bool White;
    private bool Black;

    bool fall = false; //最初に落ちないようfalseに

    Vector3 ini;
    Vector3 pos;

    //メインコントローラー
    //[SerializeField] MainCon mc;

    //回転方向
    public enum RotationDirection
    {
        Left,
        Right,
        Stop,
    }

    private void Start()
    {
        //最初は回転しない
        rotDir = RotationDirection.Stop;

        //transform取得
        Transform initialTransform = this.gameObject.GetComponent<Transform>();

        //座標取得
        ini = initialTransform.position;

        if (transform.rotation.z >= 0)
        {
            Debug.Log("White");
            White = true;
            Black = false;
        }
        else
        {
            Debug.Log("Black");
            White = false;
            Black = true;
        }

    }

    void Update()
    {
        /*
        //回転方向の決定
        if (Input.GetKeyDown(KeyCode.A) && White == true)
        {
            rotDir = RotationDirection.Left;
            currentCoroutine = StartCoroutine(Toss());//トス開始
            White = false;
            Black = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && Black == true)
        {
            rotDir = RotationDirection.Right;
            currentCoroutine = StartCoroutine(Toss());//トス開始
            White = true;
            Black = false;
        }
        */
    }

    public void StartToss(RotationDirection dir)
    {
        rotDir = dir;//回転方向を決定
        Debug.Log("回転スタート");
        //currentCoroutine = StartCoroutine(Toss());
        currentCoroutine.Enqueue(StartCoroutine(Toss()));
        currentCoroutine.Dequeue();
    }


    //コインのトス
    private IEnumerator Toss()
    {
        //左回り
        if (rotDir == RotationDirection.Left)
        {
            z *= 1.0f;
        }

        //右回り
        else if (rotDir == RotationDirection.Right)
        {
            z *= -1.0f;
        }

        //回転準備
        Vector3 startPosition = transform.position;//トス前のコイン座標を保存
        Vector3 nextPosition = startPosition;



        //回転

        while (true)
        {
            nextPosition.y += 0.01f;//次の移動場所はyを少し増やす
            transform.position = nextPosition;

            //移動距離が規定値を超えたら回転
            if (transform.position.y - startPosition.y >= 1.0f)
            {
                while (true)
                {
                    transform.Rotate(new Vector3(x, y, z) * Time.deltaTime); //回転
                    rotatedAngle += Mathf.Abs(z) * Time.deltaTime; //ｚの値が1より小さいのでここで度数法に変換

                    //rotateAngleが180.0fより増えたら発動
                    if (rotatedAngle > 180.0f)
                    {
                        currentCoroutine.Enqueue(StartCoroutine(Fall()));//落下
                        currentCoroutine.Dequeue();
                        //StopCoroutine(currentCoroutine);//トスストップ
                    }

                    yield return null;
                }
            }

            yield return null;
        }
    }

    //落ちる用のコルーチンを書く（Fallとする）
    private IEnumerator Fall()
    {
        rotatedAngle = 0.0f; //角度を0に戻す
        rotDir = RotationDirection.Stop; //停止

        //落下準備
        Vector3 startPosition = transform.position;//トス前のコイン座標を保存
        Vector3 nextPosition = startPosition;

        while (true)
        {
            nextPosition.y -= 0.03f;//yを減らしながら移動
            transform.position = nextPosition;


            if (transform.position.y <= 0.07f)
            {
                transform.position = ini;
                GameObject PrefabSmoke = Instantiate(smoke, ini, Quaternion.identity); //smoke生成
                Destroy(PrefabSmoke, 2.0f); //smoke削除
                break;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Board"))
        {
            fall = false;
            Quaternion q = new Quaternion();
            Vector3 angle;
            angle = startRotation;
            q.eulerAngles = angle;
            transform.rotation = q;
        }
    }
}
