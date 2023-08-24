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
    private Vector3 reverseRot = new Vector3(0f, 180f, 0f);

    Queue<Coroutine> currentCoroutine = new Queue<Coroutine>(); //�L���[��錾

    [SerializeField] GameObject smoke; //�X���[�N�����p

    //�G�t�F�N�g�e�X�g�p
    [SerializeField] GameObject Thunder;
    [SerializeField] GameObject Bom;
    [SerializeField] GameObject Hit;

    //��������
    private bool White;
    private bool Black;

    bool fall = false; //�ŏ��ɗ����Ȃ��悤false��

    Vector3 ini;


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
    }

    public IEnumerator StartToss(RotationDirection dir)
    {
        rotDir = dir;//��]����������

        yield return StartCoroutine(Toss());
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
        startRotation = transform.rotation.eulerAngles;


        //��]
        while (true)
        {
            nextPosition.y += 0.03f;//���̈ړ��ꏊ��y���������₷
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
                        yield return StartCoroutine(Fall());
                        yield break;
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
                //GameObject PrefabSmoke = Instantiate(smoke, ini, Quaternion.Euler(90, 5, 0)); //smoke����
                //Destroy(PrefabSmoke, 2.0f); //smoke�폜

                //�G�t�F�N�g�e�X�g�p
                //GameObject PrefabThunder = Instantiate(Thunder, ini, Quaternion.Euler(0, 0, 0));
                //Destroy(PrefabThunder, 2.0f);

                //GameObject PrefabBom = Instantiate(Bom, ini, Quaternion.Euler(0, 0, 0));
                //Destroy(PrefabBom, 2.0f);

                GameObject PrefabHit = Instantiate(Hit, ini, Quaternion.Euler(0, 0, 0));
                Destroy(PrefabHit, 2.0f);

                yield break;
            }
            yield return null;
        }
    }
}
