using UnityEngine;

public sealed class BoxView : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Color _ghostColor;
    [SerializeField]
    private LayerMask _layer;

    public Renderer Renderer => _renderer;
    public Color GhostColor => _ghostColor;
    public LayerMask Layer => _layer;
    public bool IsCollised { get; private set; }
    public bool IsRotated { get; set; }
    public Vector3 Size { get; set; }


    private void OnTriggerStay(Collider other)
    {
        IsCollised = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsCollised = false;
    }
}
