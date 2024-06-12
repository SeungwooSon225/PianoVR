using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 실제 피아노 위치와 VR 씬의 피아노 위치를 맞추기 위한 스크립트
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


    // 왼쪽 컨트롤러 위치로 VR 씬의 피아노 시작 지점을 옮기는 함수
    public void SynchronizeStartPoint()
    {
        startPoint.transform.position = lefttController.transform.position;
    }


    // 오른쪽 컨트롤러 방향으로 VR 씬의 피아노를 회전시키고 크기를 조절하는 함수
    public void SynchronizeLeftTopPoint()
    {
        float targetDistance = (startPoint.transform.position - lefttController.transform.position).magnitude;
        float currentDistance = (startPoint.transform.position - leftTopPoint.transform.position).magnitude;

        float scale = targetDistance / currentDistance;
        startPoint.transform.localScale *= scale;
        startPoint.transform.forward = startPoint.transform.position - lefttController.transform.position;
    }
}
