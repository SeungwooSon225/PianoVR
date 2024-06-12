using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���̽㿡�� �޾ƿ� �ǾƳ� �Է� �����͸� ���� �ǹ�, ���/���� �̸�Ƽ���� �����ϴ� ��ũ��Ʈ
public class PinaoKeyController : MonoBehaviour
{
    [SerializeField]
    private UDPReceiver _UDPReceiver;                      // ���̽㿡�� �����͸� �޾ƿ��� �ν��Ͻ�

    [SerializeField]
    private midiPiano.PianoManager midiPianoManager;       // �ǾƳ� Ű�� �����ϴ� �ν��Ͻ�

    // ��ݰ� ���� �̸�Ƽ�� ������Ʈ
    [SerializeField]
    private GameObject _sad;
    [SerializeField]
    private GameObject _happy;


    // Update is called once per frame
    void Update()
    {
        if (_UDPReceiver.Data == null) return;

        // ���̽㿡�� �ǾƳ� �Է¿� ���� �����͸� �޾ƿ�
        string[] data = _UDPReceiver.Data.Split(' ');

        switch (data[0])
        {
            // �ǹ��� �����ٴ� �޼����� �ش� �ǹ��� ������ �Լ��� ����
            case "on":
                midiPianoManager.GetPianoKeyDown(int.Parse(data[1]));
                break;
            // �ǹ��� ���ٴ� �޼����� �ش� �ǹ��� ���� �Լ��� ����
            case "off":
                midiPianoManager.GetPianoKeyUp(int.Parse(data[1]));
                break;
            // ������� ������ ��ȯ������ ��� �̸�Ƽ���� Ȱ��ȭ
            case "happy":
                if (!_happy.activeSelf)
                {
                    _happy.SetActive(true);
                    _sad.SetActive(false);
                }
                break;
            // �������� ������ ��ȯ������ ���� �̸�Ƽ���� Ȱ��ȭ
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
