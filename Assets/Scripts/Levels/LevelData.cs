using UnityEngine;

[CreateAssetMenu(fileName = "LevelData_01", menuName = "BreakOutLevelData")]
public class LevelData : ScriptableObject
{
    [Header("Grid")]
    public int columbs = 12;
    public int rows = 6;
    public Vector2 _startPos = new Vector2(-6f, 3.5f);
    public Vector2 _spacing = new Vector2(1.1f, 0.55f);

    [Header("Pattern")]
    [Tooltip("0=Empty, 1=Normal, 2=Strong")]
    public int[] pattern;

    // [Header("Difficulty")]
    // [Tooltip("Top rows become stronger. Example: 3 means top row hp = 3, next hp = 2, etc.")]
    // public int maxRowHp = 3;

    [Header("Scoring")]
    public int basePointPerHp = 10;

    public int GetCell(int row, int col)
    {
        int index = row * columbs + col;
        if (index < 0 || index >= pattern.Length)
            return 0;

        return pattern[index];
    }

}
