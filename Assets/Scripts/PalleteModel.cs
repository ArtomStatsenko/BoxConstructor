using UnityEngine;

[CreateAssetMenu(fileName = "PalleteModel", menuName = "Models/Pallete", order = 1)]
public sealed class PalleteModel : ScriptableObject
{
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _width;
    [SerializeField]
    private int _layerCount;

    public float Length => _length;
    public float Width => _width;
    public int LayerCount => _layerCount;
    public Vector2 Size => new Vector2(_length, _width);
}