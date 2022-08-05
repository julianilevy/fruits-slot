using UnityEditor;

[CustomEditor(typeof(PingPongTransformScale))]
public class PingPongTransformScaleEditor : PingPongValueEditor
{
    private PingPongTransformScale _target;

    protected override void OnEnable()
    {
        base.OnEnable();

        _target = (PingPongTransformScale)target;
    }

    public override void OnInspectorGUI()
    {
        OnInspectorGUI(_target);
    }
}