using UnityEditor;

[CustomEditor(typeof(PingPongSpriteRendererColor))]
public class PingPongSpriteRendererColorEditor : PingPongValueEditor
{
    private PingPongSpriteRendererColor _target;

    protected override void OnEnable()
    {
        base.OnEnable();

        _target = (PingPongSpriteRendererColor)target;
    }

    public override void OnInspectorGUI()
    {
        OnInspectorGUI(_target);
    }
}