using UnityEngine;

[CreateAssetMenu(fileName = "NewCursorData", menuName = "Game/Cursor Data")]
public class CursorData : ScriptableObject
{
    public Sprite cursorImage;
    public Vector2 hotspot;
    public Color cursorColor = Color.white;
    public float rotationSpeed;
}
