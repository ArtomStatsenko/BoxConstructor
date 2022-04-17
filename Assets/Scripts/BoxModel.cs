using UnityEngine;

[CreateAssetMenu(fileName = "BoxModel" , menuName = "Models/Box", order = 1)]
public sealed class BoxModel : ScriptableObject
{
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _height; 
    [SerializeField]
    private float _width;

    public float Length => _length;
    public float Height => _height;
    public float Width => _width;
    public Vector3 Size => new Vector3(_length, _height, _width);
}