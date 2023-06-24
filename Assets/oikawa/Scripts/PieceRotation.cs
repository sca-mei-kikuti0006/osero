using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PieceRotation : MonoBehaviour
{

    [SerializeField] private RotationDirection rotDir; //���������̔��ʗp
    private float x;
    private float y;
    [SerializeField] private float z = 100f;

    private float rotatedAngle; //�x���@
    private Vector3 startRotation;//�オ�肫�����Ƃ���ł̉�]�p�ۑ��p
    //private Coroutine currentCoroutine;//���ݎ��s���̃R���[�`��
    Queue<Coroutine> currentCoroutine = new Queue<Coroutine>(); //�L���[��錾

    [SerializeField] GameObject smoke;

    //��������
    private bool White;
    private bool Black;

    bool fall = false; //�ŏ��ɗ����Ȃ��悤false��

    Vector3 ini;
    Vector3 pos;

    //���C���R���g���[���[
    //[SerializeField] MainCon mc;

    //��]����
    public enum RotationDirection
    {
        Left,
        Right,
        Stop,
    }

    private void Start()
    {
        //�ŏ��͉�]���Ȃ�
        rotDir = RotationDirection.Stop;

        //transform�擾
        Transform initialTransform = this.gameObject.GetComponent<Transform>();

        //���W�擾
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
        //��]�����̌���
        if (Input.GetKeyDown(KeyCode.A) && White == true)
        {
            rotDir = RotationDirection.Left;
            currentCoroutine = StartCoroutine(Toss());//�g�X�J�n
            White = false;
            Black = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && Black == true)
        {
            rotDir = RotationDirection.Right;
            currentCoroutine = StartCoroutine(Toss());//�g�X�J�n
            White = true;
            Black = false;
        }
        */
    }

    public void StartToss(RotationDirection dir)
    {
        rotDir = dir;//��]����������
        Debug.Log("��]�X�^�[�g");
        //currentCoroutine = StartCoroutine(Toss());
        currentCoroutine.Enqueue(StartCoroutine(Toss()));
        currentCoroutine.Dequeue();
    }


    //�R�C���̃g�X
    private IEnumerator Toss()
    {
        //�����
        if (rotDir == RotationDirection.Left)
        {
            z *= 1.0f;
        }

        //�E���
        else if (rotDir == RotationDirection.Right)
        {
            z *= -1.0f;
        }

        //��]����
        Vector3 startPosition = transform.position;//�g�X�O�̃R�C�����W��ۑ�
        Vector3 nextPosition = startPosition;



        //��]

        while (true)
        {
            nextPosition.y += 0.01f;//���̈ړ��ꏊ��y���������₷
            transform.position = nextPosition;

            //�ړ��������K��l�𒴂������]
            if (transform.position.y - startPosition.y >= 1.0f)
            {
                while (true)
                {
                    transform.Rotate(new Vector3(x, y, z) * Time.deltaTime); //��]
                    rotatedAngle += Mathf.Abs(z) * Time.deltaTime; //���̒l��1��菬�����̂ł����œx���@�ɕϊ�

                    //rotateAngle��180.0f��葝�����甭��
                    if (rotatedAngle > 180.0f)
                    {
                        currentCoroutine.Enqueue(StartCoroutine(Fall()));//����
                        currentCoroutine.Dequeue();
                        //StopCoroutine(currentCoroutine);//�g�X�X�g�b�v
                    }

                    yield return null;
                }
            }

            yield return null;
        }
    }

    //������p�̃R���[�`���������iFall�Ƃ���j
    private IEnumerator Fall()
    {
        rotatedAngle = 0.0f; //�p�x��0�ɖ߂�
        rotDir = RotationDirection.Stop; //��~

        //��������
        Vector3 startPosition = transform.position;//�g�X�O�̃R�C�����W��ۑ�
        Vector3 nextPosition = startPosition;

        while (true)
        {
            nextPosition.y -= 0.03f;//y�����炵�Ȃ���ړ�
            transform.position = nextPosition;


            if (transform.position.y <= 0.07f)
            {
                transform.position = ini;
                GameObject PrefabSmoke = Instantiate(smoke, ini, Quaternion.identity); //smoke����
                Destroy(PrefabSmoke, 2.0f); //smoke�폜
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
