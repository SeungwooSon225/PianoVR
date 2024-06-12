using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 파이썬에서 받아온 손 위치 추적 데이터를 사용하여 유니티 씬에서 손 모델을 움직이기 위한 스크립트
public class HandModelController : MonoBehaviour
{
    [SerializeField]
    private UDPReceiver _UDPReceiver;                     // 파이썬에서 데이터를 받아오는 인스턴스

    [SerializeField]
    private GameObject _startPoint;                     // 손 위치 추적에 사용되는 마커들 중 시작점 마커와 대응되는 오브젝트
    [SerializeField]
    private GameObject _endPoint;                       // 손 위치 추적에 사용되는 마커들 중 끝점 마커와 대응되는 오브젝트

    [SerializeField]
    private GameObject[] _pianoKeys;                    // 사용자가 누를 수 있는 10개의 피아노 건반에 해당되는 오브젝트

    [SerializeField]
    private GameObject _rightHand;                      // 오른손 모델
    [SerializeField]
    private GameObject _leftHand;                       // 왼손 모델

    [SerializeField]
    private GameObject[] _fingerLandmarks;              // 손가락 위치를 조절하기 위해 파이썬에서 받아온 손가락 랜드마크 위치에 맞춰 움직이는 오브젝트
    [SerializeField]
    private GameObject[] _handLandmarks;                // 손 위치/방향을 조절하기 위해 파이썬에서 받아온 손목/중지 시작점 랜드마크 위치에 맞춰 움직이는 오브젝트
    [SerializeField]
    private bool[] _isFingerPressed = new bool[10];     // 건반을 누르는 애니메이션을 위해 손가락이 건반을 눌렀는지 저장하는 변수

    // 피아노 음을 받아서 해당되는 오브젝트로 연결하기 위한 딕셔너리 (_pianoKeys 의 인덱스로 사용됨)
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

        // 파이썬에서 손을 추적한 데이터를 받아옴
        string[] data = _UDPReceiver.Data.Split(' ');

        int fingerLanmarkIndex = 0;
        int handLandmarkIndex = 0;

        for (int i = 0; i < data.Length / 2; i++)
        {
            // 각 좌표의 로컬 x, z 값을 계산
            float x = _endPoint.transform.localPosition.x * float.Parse(data[i * 2]);
            float z = _endPoint.transform.localPosition.z * float.Parse(data[i * 2 + 1]);

            // 손가락 좌표인 경우
            if (i != 0 && i != 3 && i != 7 && i != 10)
            {
                // 손가락 랜드마크를 해당 좌표로 이동
                // 각 손가락이 해당되는 랜드마크를 목표로 IK가 적용되어 손가락이 랜드마크 위치를 따라 이동
                _fingerLandmarks[fingerLanmarkIndex].transform.localPosition = new Vector3(x, 0f, z);

                // 만약 해당 손가락이 건반을 누른 상태라면 랜드마크를 아래로 이동 (IK를 활용한 건반을 누르는 애니메이션을 위함)
                if (_isFingerPressed[fingerLanmarkIndex])
                {
                    _fingerLandmarks[fingerLanmarkIndex].transform.localPosition += new Vector3(0f, -1.5f, 0f);
                }

                fingerLanmarkIndex++;
            }
            // 손목, 중지 시작점 좌표인 경유
            else
            {
                // 손목, 중지 시작점 랜드마크를 해당 좌표로 이동
                _handLandmarks[handLandmarkIndex].transform.localPosition = new Vector3(x, 0f, z);
                handLandmarkIndex++;
            }
        }

        // 손 모델을 손목 좌표로 이동하고 중지 방향으로 바라 보게 만들어서 손의 방향을 맞춰줌
        _rightHand.transform.position = _handLandmarks[0].transform.position;
        _rightHand.transform.localPosition = new Vector3(_rightHand.transform.localPosition.x, 0.06f, _rightHand.transform.localPosition.z);
        _rightHand.transform.forward = _handLandmarks[1].transform.position - _handLandmarks[0].transform.position;

        _leftHand.transform.position = _handLandmarks[2].transform.position;
        _leftHand.transform.localPosition = new Vector3(_leftHand.transform.localPosition.x, 0.06f, _leftHand.transform.localPosition.z);
        _leftHand.transform.forward = _handLandmarks[3].transform.position - _handLandmarks[2].transform.position;
    }


    // 피아노 건반을 입력 받아 해당 건반위에 손가락이 있다면 해당 손가락이 건반을 누른 상태라고 표시해주는 함수 
    // 건반을 눌렀다는 입력이 들어오면 실행되어서 손가락 랜드마크가 아래로 내려가서 손가락이 건반을 누를 수 있게 해줌
    public void SetFingerPressed(int key)
    {
        // 건반의 시작/끝 좌표를 받아 옴
        int mappedKey = _keyMapper[key];
        float startX = _pianoKeys[mappedKey].transform.localPosition.x;
        float endX = _pianoKeys[mappedKey + 1].transform.localPosition.x;

        // 손가락을 하나씩 검사하면서 해당 손가락이 건반의 시작/끝 좌표 사이에 위치하는지 검사
        for (int i = 0; i < _fingerLandmarks.Length; i++)
        {
            float fingerX = _fingerLandmarks[i].transform.localPosition.x;

            // 손가락이 건반의 시작/끝 좌표 사이에 위치한다면 그 손가락이 건반을 눌렀다고 표시
            if (fingerX < startX && fingerX > endX) _isFingerPressed[i] = true;
        }
    }



    // 피아노 건반을 입력 받아 해당 건반위에 손가락이 있다면 해당 손가락이 건반에서 때진 상태라고 표시해주는 함수 
    // 건반을 눌렀다는 입력이 들어오면 실행되어서 손가락 랜드마크가 위로 올라가서 건반을 눌렀던 손가락이 다시 원래대로 돌아올 수 있게 해줌
    public void SetFingerUnPressed(int key)
    {
        // 건반의 시작/끝 좌표를 받아 옴
        int mappedKey = _keyMapper[key];
        float startX = _pianoKeys[mappedKey].transform.localPosition.x;
        float endX = _pianoKeys[mappedKey + 1].transform.localPosition.x;

        // 손가락을 하나씩 검사하면서 해당 손가락이 건반의 시작/끝 좌표 사이에 위치하는지 검사
        for (int i = 0; i < _fingerLandmarks.Length; i++)
        {
            float fingerX = _fingerLandmarks[i].transform.localPosition.x;

            // 손가락이 건반의 시작/끝 좌표 사이에 위치한다면 그 손가락이 건반을 누르지 않았다고 표시
            if (fingerX < startX && fingerX > endX) _isFingerPressed[i] = false;
        }
    }
}
