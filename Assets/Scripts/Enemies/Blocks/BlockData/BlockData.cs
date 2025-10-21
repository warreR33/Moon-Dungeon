using UnityEngine;

[CreateAssetMenu(fileName = "New Block Data", menuName = "Game/Block Data")]
public class BlockData : ScriptableObject
{
    [Header("Información del Bloque")]
    public string blockName;
    public Sprite sprite;

    [Header("Estadísticas")]
    public int life = 3;
    public int reward = 10;

    [Header("Comportamiento")]
    public bool spawnEnemyOnDestroy = false;
}
