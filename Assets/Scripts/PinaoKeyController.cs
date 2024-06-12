using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 파이썬에서 받아온 피아노 입력 데이터를 통해 건반, 기쁨/슬픔 이모티콘을 조절하는 스크립트
public class PinaoKeyController : MonoBehaviour
{
    [SerializeField]
    private UDPReceiver _UDPReceiver;                      // 파이썬에서 데이터를 받아오는 인스턴스

    [SerializeField]
    private midiPiano.PianoManager midiPianoManager;       // 피아노 키를 관리하는 인스턴스

    // 기쁨과 슬픔 이모티콘 오브젝트
    [SerializeField]
    private GameObject _sad;
    [SerializeField]
    private GameObject _happy;


    // Update is called once per frame
    void Update()
    {
        if (_UDPReceiver.Data == null) return;

        // 파이썬에서 피아노 입력에 대한 데이터를 받아옴
        string[] data = _UDPReceiver.Data.Split(' ');

        switch (data[0])
        {
            // 건반을 눌렀다는 메세지면 해당 건반을 누르는 함수를 실행
            case "on":
                midiPianoManager.GetPianoKeyDown(int.Parse(data[1]));
                break;
            // 건반을 땠다는 메세지면 해당 건반을 때는 함수를 실행
            case "off":
                midiPianoManager.GetPianoKeyUp(int.Parse(data[1]));
                break;
            // 기쁨으로 감정을 전환했으면 기쁨 이모티콘을 활성화
            case "happy":
                if (!_happy.activeSelf)
                {
                    _happy.SetActive(true);
                    _sad.SetActive(false);
                }
                break;
            // 슬픔으로 감정을 전환했으면 슬픔 이모티콘을 활성화
            case "sad":
                if (!_sad.activeSelf)
                {
                    _happy.SetActive(false);
                    _sad.SetActive(true);
                }
                break;
        }
    }
}
