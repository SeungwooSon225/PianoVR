using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SynchronizePiano))]
public class SynchronizePianoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        // MyComponent 스크립트의 인스턴스를 가져옮
        SynchronizePiano snchronizePiano = (SynchronizePiano)target;

        // 버튼을 인스펙터에 추가
        if (GUILayout.Button("Synchronize Start Point"))
        {
            // 버튼이 클릭되었을 때 MyFunction 함수 실행
            snchronizePiano.SynchronizeStartPoint();
        }

        // 버튼을 인스펙터에 추가
        if (GUILayout.Button("Synchronize Left Top Point"))
        {
            // 버튼이 클릭되었을 때 MyFunction 함수 실행
            snchronizePiano.SynchronizeLeftTopPoint();
        }
    }
}
