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
    private float totalRotatedAngle = 0.0f; //���܂ł̉�]�ʂ̍��v
    private Vector3 startRotation;//�オ�肫�����Ƃ���ł̉�]�p�ۑ��p
    private Vector3 reverseRot = new Vector3(0f, 180f, 0f);
    Vector3 ini;

    Queue<Coroutine> currentCoroutine = new Queue<Coroutine>(); //�L���[��錾

    [SerializeField] GameObject smoke; //�X���[�N�����p


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
            nextPosition.y += 0.08f;//���̈ړ��ꏊ��y���������₷
            transform.position = nextPosition;

            //�ړ��������K��l�𒴂������]
            if (transform.position.y - startPosition.y >= 1.0f)
            {
                while (true)
                {
                    transform.Rotate(new Vector3(x, y, z) * 3 * Time.deltaTime); //��]
                    rotatedAngle += Mathf.Abs(z) * 3 * Time.deltaTime; //���̒l��1��菬�����̂ł����œx���@�ɕϊ�

                    //rotateAngle��180.0f��葝�����甭��
                    if (rotatedAngle > 180.0f)
                    {
                        float dif = rotatedAngle - 180.0f;//��]����������
                        rotatedAngle = 0.0f; //rotatedAngle��0�ɒ���
                        transform.Rotate(new Vector3(x, y, -dif));//��]�������ĕ�������]���t�����ɖ߂�

                        /*
                        totalRotatedAngle += 180.0f;
                        
                        var currentRotation = transform.rotation;//���݂̂�����Ƃ���Ă��܂��Ă���rotation
                        Debug.Log("cur" + currentRotation.eulerAngles);
                        var threshold = 2.0f;
                        var targetRotation = transform.rotation;
                        //180�x�ߕӂȂ�
                        if (180.0f - threshold < currentRotation.eulerAngles.z && currentRotation.eulerAngles.z < 180.0f + threshold)
                        {
                            targetRotation.eulerAngles = new Vector3(0,0,180.0f);
                            Debug.Log("180�x�ߕӂł�");
                        }
                        else
                        {
                            targetRotation.eulerAngles = new Vector3(0,0,0.0f);
                            Debug.Log("0�x�ߕӂł�");
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


    //������p�̃R���[�`���������iFall�Ƃ���j
    private IEnumerator Fall()
    {
        rotDir = RotationDirection.Stop; //��~

        //��������
        Vector3 startPosition = transform.position;//�g�X�O�̃R�C�����W��ۑ�
        Vector3 nextPosition = startPosition;

        while (true)
        {
            nextPosition.y -= 0.08f;//y�����炵�Ȃ���ړ�
            transform.position = nextPosition;

            if (transform.position.y <= 0.07f)
            {
                transform.position = ini;
                GameObject PrefabSmoke = Instantiate(smoke, ini, Quaternion.Euler(90, 5, 0)); //smoke����
                Destroy(PrefabSmoke, 2.0f); //smoke�폜              
                yield break;
            }
            yield return null;
        }
    }
}
