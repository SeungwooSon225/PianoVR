using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���̽㿡�� �޾ƿ� �� ��ġ ���� �����͸� ����Ͽ� ����Ƽ ������ �� ���� �����̱� ���� ��ũ��Ʈ
public class HandModelController : MonoBehaviour
{
    [SerializeField]
    private UDPReceiver _UDPReceiver;                     // ���̽㿡�� �����͸� �޾ƿ��� �ν��Ͻ�

    [SerializeField]
    private GameObject _startPoint;                     // �� ��ġ ������ ���Ǵ� ��Ŀ�� �� ������ ��Ŀ�� �����Ǵ� ������Ʈ
    [SerializeField]
    private GameObject _endPoint;                       // �� ��ġ ������ ���Ǵ� ��Ŀ�� �� ���� ��Ŀ�� �����Ǵ� ������Ʈ

    [SerializeField]
    private GameObject[] _pianoKeys;                    // ����ڰ� ���� �� �ִ� 10���� �ǾƳ� �ǹݿ� �ش�Ǵ� ������Ʈ

    [SerializeField]
    private GameObject _rightHand;                      // ������ ��
    [SerializeField]
    private GameObject _leftHand;                       // �޼� ��

    [SerializeField]
    private GameObject[] _fingerLandmarks;              // �հ��� ��ġ�� �����ϱ� ���� ���̽㿡�� �޾ƿ� �հ��� ���帶ũ ��ġ�� ���� �����̴� ������Ʈ
    [SerializeField]
    private GameObject[] _handLandmarks;                // �� ��ġ/������ �����ϱ� ���� ���̽㿡�� �޾ƿ� �ո�/���� ������ ���帶ũ ��ġ�� ���� �����̴� ������Ʈ
    [SerializeField]
    private bool[] _isFingerPressed = new bool[10];     // �ǹ��� ������ �ִϸ��̼��� ���� �հ����� �ǹ��� �������� �����ϴ� ����

    // �ǾƳ� ���� �޾Ƽ� �ش�Ǵ� ������Ʈ�� �����ϱ� ���� ��ųʸ� (_pianoKeys �� �ε����� ����)
    private Dictionary<int, int> _keyMapper = new Dictionary<int, int>() {
        { 48, 0 },
        { 50, 1 },
        { 52, 2 },
        { 53, 3 },
        { 55, 4 },
        { 57, 5 },
        { 59, 6 },
        { 60, 7 },
        { 62, 8 },
        { 64, 9 },
        { 65, 10 },
        { 67, 11 },
        { 69, 12 },
        { 71, 13 },
        { 72, 14 },
    };


    // Update is called once per frame
    void Update()
    {
        if (_UDPReceiver.Data == null) return;

        // ���̽㿡�� ���� ������ �����͸� �޾ƿ�
        string[] data = _UDPReceiver.Data.Split(' ');

        int fingerLanmarkIndex = 0;
        int handLandmarkIndex = 0;

        for (int i = 0; i < data.Length / 2; i++)
        {
            // �� ��ǥ�� ���� x, z ���� ���
            float x = _endPoint.transform.localPosition.x * float.Parse(data[i * 2]);
            float z = _endPoint.transform.localPosition.z * float.Parse(data[i * 2 + 1]);

            // �հ��� ��ǥ�� ���
            if (i != 0 && i != 3 && i != 7 && i != 10)
            {
                // �հ��� ���帶ũ�� �ش� ��ǥ�� �̵�
                // �� �հ����� �ش�Ǵ� ���帶ũ�� ��ǥ�� IK�� ����Ǿ� �հ����� ���帶ũ ��ġ�� ���� �̵�
                _fingerLandmarks[fingerLanmarkIndex].transform.localPosition = new Vector3(x, 0f, z);

                // ���� �ش� �հ����� �ǹ��� ���� ���¶�� ���帶ũ�� �Ʒ��� �̵� (IK�� Ȱ���� �ǹ��� ������ �ִϸ��̼��� ����)
                if (_isFingerPressed[fingerLanmarkIndex])
                {
                    _fingerLandmarks[fingerLanmarkIndex].transform.localPosition += new Vector3(0f, -1.5f, 0f);
                }

                fingerLanmarkIndex++;
            }
            // �ո�, ���� ������ ��ǥ�� ����
            else
            {
                // �ո�, ���� ������ ���帶ũ�� �ش� ��ǥ�� �̵�
                _handLandmarks[handLandmarkIndex].transform.localPosition = new Vector3(x, 0f, z);
                handLandmarkIndex++;
            }
        }

        // �� ���� �ո� ��ǥ�� �̵��ϰ� ���� �������� �ٶ� ���� ���� ���� ������ ������
        _rightHand.transform.position = _handLandmarks[0].transform.position;
        _rightHand.transform.localPosition = new Vector3(_rightHand.transform.localPosition.x, 0.06f, _rightHand.transform.localPosition.z);
        _rightHand.transform.forward = _handLandmarks[1].transform.position - _handLandmarks[0].transform.position;

        _leftHand.transform.position = _handLandmarks[2].transform.position;
        _leftHand.transform.localPosition = new Vector3(_leftHand.transform.localPosition.x, 0.06f, _leftHand.transform.localPosition.z);
        _leftHand.transform.forward = _handLandmarks[3].transform.position - _handLandmarks[2].transform.position;
    }


    // �ǾƳ� �ǹ��� �Է� �޾� �ش� �ǹ����� �հ����� �ִٸ� �ش� �հ����� �ǹ��� ���� ���¶�� ǥ�����ִ� �Լ� 
    // �ǹ��� �����ٴ� �Է��� ������ ����Ǿ �հ��� ���帶ũ�� �Ʒ��� �������� �հ����� �ǹ��� ���� �� �ְ� ����
    public void SetFingerPressed(int key)
    {
        // �ǹ��� ����/�� ��ǥ�� �޾� ��
        int mappedKey = _keyMapper[key];
        float startX = _pianoKeys[mappedKey].transform.localPosition.x;
        float endX = _pianoKeys[mappedKey + 1].transform.localPosition.x;

        // �հ����� �ϳ��� �˻��ϸ鼭 �ش� �հ����� �ǹ��� ����/�� ��ǥ ���̿� ��ġ�ϴ��� �˻�
        for (int i = 0; i < _fingerLandmarks.Length; i++)
        {
            float fingerX = _fingerLandmarks[i].transform.localPosition.x;

            // �հ����� �ǹ��� ����/�� ��ǥ ���̿� ��ġ�Ѵٸ� �� �հ����� �ǹ��� �����ٰ� ǥ��
            if (fingerX < startX && fingerX > endX) _isFingerPressed[i] = true;
        }
    }



    // �ǾƳ� �ǹ��� �Է� �޾� �ش� �ǹ����� �հ����� �ִٸ� �ش� �հ����� �ǹݿ��� ���� ���¶�� ǥ�����ִ� �Լ� 
    // �ǹ��� �����ٴ� �Է��� ������ ����Ǿ �հ��� ���帶ũ�� ���� �ö󰡼� �ǹ��� ������ �հ����� �ٽ� ������� ���ƿ� �� �ְ� ����
    public void SetFingerUnPressed(int key)
    {
        // �ǹ��� ����/�� ��ǥ�� �޾� ��
        int mappedKey = _keyMapper[key];
        float startX = _pianoKeys[mappedKey].transform.localPosition.x;
        float endX = _pianoKeys[mappedKey + 1].transform.localPosition.x;

        // �հ����� �ϳ��� �˻��ϸ鼭 �ش� �հ����� �ǹ��� ����/�� ��ǥ ���̿� ��ġ�ϴ��� �˻�
        for (int i = 0; i < _fingerLandmarks.Length; i++)
        {
            float fingerX = _fingerLandmarks[i].transform.localPosition.x;

            // �հ����� �ǹ��� ����/�� ��ǥ ���̿� ��ġ�Ѵٸ� �� �հ����� �ǹ��� ������ �ʾҴٰ� ǥ��
            if (fingerX < startX && fingerX > endX) _isFingerPressed[i] = false;
        }
    }
}
