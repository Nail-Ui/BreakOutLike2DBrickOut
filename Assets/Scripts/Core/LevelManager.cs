using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData[] levels;
    public LevelData Current => (levels != null && levels.Length > 0) ? levels[_levelIndex] : null;
    private int _levelIndex;

    public void ResetToFirstLevel() => _levelIndex = 0;

    public bool TryGoNextLevel()
    {
        if(levels == null || levels.Length == 0) return false;

        _levelIndex++;
        if(_levelIndex >= levels.Length)
        {
            _levelIndex = levels.Length -1;
            return false; // bitti
        }
        return true;
    }
    public int GetLevelNumber() => _levelIndex +1;
}
