using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SynchronizePiano))]
public class SynchronizePianoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // �⺻ �ν����� �׸���
        DrawDefaultInspector();

        // MyComponent ��ũ��Ʈ�� �ν��Ͻ��� ������
        SynchronizePiano snchronizePiano = (SynchronizePiano)target;

        // ��ư�� �ν����Ϳ� �߰�
        if (GUILayout.Button("Synchronize Start Point"))
        {
            // ��ư�� Ŭ���Ǿ��� �� MyFunction �Լ� ����
            snchronizePiano.SynchronizeStartPoint();
        }

        // ��ư�� �ν����Ϳ� �߰�
        if (GUILayout.Button("Synchronize Left Top Point"))
        {
            // ��ư�� Ŭ���Ǿ��� �� MyFunction �Լ� ����
            snchronizePiano.SynchronizeLeftTopPoint();
        }
    }
}
