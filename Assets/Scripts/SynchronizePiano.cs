using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� �ǾƳ� ��ġ�� VR ���� �ǾƳ� ��ġ�� ���߱� ���� ��ũ��Ʈ
public class SynchronizePiano : MonoBehaviour
{
    [SerializeField]
    private GameObject rightController;
    [SerializeField]
    private GameObject lefttController;
    [SerializeField]
    private GameObject startPoint;
    [SerializeField]
    private GameObject leftTopPoint;


    // ���� ��Ʈ�ѷ� ��ġ�� VR ���� �ǾƳ� ���� ������ �ű�� �Լ�
    public void SynchronizeStartPoint()
    {
        startPoint.transform.position = lefttController.transform.position;
    }


    // ������ ��Ʈ�ѷ� �������� VR ���� �ǾƳ븦 ȸ����Ű�� ũ�⸦ �����ϴ� �Լ�
    public void SynchronizeLeftTopPoint()
    {
        float targetDistance = (startPoint.transform.position - lefttController.transform.position).magnitude;
        float currentDistance = (startPoint.transform.position - leftTopPoint.transform.position).magnitude;

        float scale = targetDistance / currentDistance;
        startPoint.transform.localScale *= scale;
        startPoint.transform.forward = startPoint.transform.position - lefttController.transform.position;
    }
}
