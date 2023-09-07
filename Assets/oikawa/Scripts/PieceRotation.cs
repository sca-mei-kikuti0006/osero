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
    private float totalRotatedAngle = 0.0f; //今までの回転量の合計
    private Vector3 startRotation;//上がりきったところでの回転角保存用
    private Vector3 reverseRot = new Vector3(0f, 180f, 0f);
    Vector3 ini;

    Queue<Coroutine> currentCoroutine = new Queue<Coroutine>(); //キューを宣言

    [SerializeField] GameObject smoke; //スモーク生成用


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
    }


    void Update()
    {
    }


    public IEnumerator StartToss(RotationDirection dir)
    {
        rotDir = dir;//回転方向を決定

        yield return StartCoroutine(Toss());
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
        startRotation = transform.rotation.eulerAngles;

        //回転
        while (true)
        {
            nextPosition.y += 0.08f;//次の移動場所はyを少し増やす
            transform.position = nextPosition;

            //移動距離が規定値を超えたら回転
            if (transform.position.y - startPosition.y >= 1.0f)
            {
                while (true)
                {
                    transform.Rotate(new Vector3(x, y, z) * 3 * Time.deltaTime); //回転
                    rotatedAngle += Mathf.Abs(z) * 3 * Time.deltaTime; //ｚの値が1より小さいのでここで度数法に変換

                    //rotateAngleが180.0fより増えたら発動
                    if (rotatedAngle > 180.0f)
                    {
                        float dif = rotatedAngle - 180.0f;//回転しすぎた分
                        rotatedAngle = 0.0f; //rotatedAngleを0に直す
                        transform.Rotate(new Vector3(x, y, -dif));//回転しすぎて分だけ回転を逆方向に戻す

                        /*
                        totalRotatedAngle += 180.0f;
                        
                        var currentRotation = transform.rotation;//現在のちょっとずれてしまっているrotation
                        Debug.Log("cur" + currentRotation.eulerAngles);
                        var threshold = 2.0f;
                        var targetRotation = transform.rotation;
                        //180度近辺なら
                        if (180.0f - threshold < currentRotation.eulerAngles.z && currentRotation.eulerAngles.z < 180.0f + threshold)
                        {
                            targetRotation.eulerAngles = new Vector3(0,0,180.0f);
                            Debug.Log("180度近辺です");
                        }
                        else
                        {
                            targetRotation.eulerAngles = new Vector3(0,0,0.0f);
                            Debug.Log("0度近辺です");
                        }
                        var step = 0.01f;
                        int howMany = 0;
                        while (true)
                        {
                            ++howMany;

                            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
                            if(Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z) < 0.1f) break;
                        }
                        Debug.Log(howMany);
                        */

                        yield return StartCoroutine(Fall());
                        yield break;
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
        rotDir = RotationDirection.Stop; //停止

        //落下準備
        Vector3 startPosition = transform.position;//トス前のコイン座標を保存
        Vector3 nextPosition = startPosition;

        while (true)
        {
            nextPosition.y -= 0.08f;//yを減らしながら移動
            transform.position = nextPosition;

            if (transform.position.y <= 0.07f)
            {
                transform.position = ini;
                GameObject PrefabSmoke = Instantiate(smoke, ini, Quaternion.Euler(90, 5, 0)); //smoke生成
                Destroy(PrefabSmoke, 2.0f); //smoke削除              
                yield break;
            }
            yield return null;
        }
    }
}
