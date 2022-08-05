using UnityEditor;

[CustomEditor(typeof(PingPongTransformRotation))]
public class PingPongTransformRotationEditor : PingPongValueEditor
{
    private PingPongTransformRotation _target;

    protected override void OnEnable()
    {
        base.OnEnable();

        _target = (PingPongTransformRotation)target;
    }

    public override void OnInspectorGUI()
    {
        OnInspectorGUI(_target);
    }
}