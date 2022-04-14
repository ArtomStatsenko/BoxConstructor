using UnityEngine;

[CreateAssetMenu(fileName = "BoxModel" , menuName = "Models/Box", order = 1)]
public sealed class BoxModel : ScriptableObject
{
    [SerializeField]
    private float _width;
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _height;

    public float Width => _width;
    public float Length => _length;
    public float Height => _height;
}