#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
#endif

using UnityEngine;
[System.Serializable]

public struct DeathByLevel
{
    public int level;
    public int deathCount;
}
public class GameData
{
    public int level;
    public int deathCount = 0;

    public DeathByLevel[] deathByLevel;

}
