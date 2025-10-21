using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class BlockSpawnChance
{
    public BlockData data;
    [Range(0f, 1f)] public float chance = 0.5f; // peso relativo
}


public class BlockRowSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Block blockPrefab;

    [Header("Bloques posibles (con probabilidades)")]
    [SerializeField] private List<BlockSpawnChance> blockChances;

    [Header("Espacios vacíos")]
    [Range(0f, 1f)] [SerializeField] private float emptySpaceChance = 0.2f;

    [Header("Posicionamiento")]
    [SerializeField] private float ySpawnOffset = 1f;
    [SerializeField] private float horizontalPadding = 0.1f;
    [SerializeField] private float leftMargin = 0.5f;
    [SerializeField] private float rightMargin = 0.5f;

    private float cameraWidth;
    private float cameraTopY;

    private void Start()
    {
        CalculateCameraBounds();
    }

    private void CalculateCameraBounds()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        cameraWidth = halfWidth * 2f;
        cameraTopY = cam.transform.position.y + halfHeight + ySpawnOffset;
    }

    public void SpawnRowInParent(Transform parent)
    {
        if (blockPrefab == null)
        {
            Debug.LogWarning("Spawner sin prefab asignado.");
            return;
        }

        float blockWidth = GetBlockWidth(blockPrefab);
        float availableWidth = cameraWidth - (leftMargin + rightMargin);
        int maxBlocks = Mathf.FloorToInt(availableWidth / (blockWidth + horizontalPadding));
        float startX = -cameraWidth / 2f + leftMargin + (blockWidth / 2f) + camOffsetX();

        for (int i = 0; i < maxBlocks; i++)
        {
            if (Random.value < emptySpaceChance)
                continue;
            BlockData chosenData = GetRandomBlockData();

            if (chosenData == null)
                continue;

            Vector3 spawnPos = new Vector3(startX + i * (blockWidth + horizontalPadding), cameraTopY, 0);
            Block newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, parent);
            newBlock.InitializeFromData(chosenData);
        }
    }

    private BlockData GetRandomBlockData()
    {
        if (blockChances == null || blockChances.Count == 0)
            return null;

        float totalChance = 0f;
        foreach (var b in blockChances)
            totalChance += b.chance;

        float rand = Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var b in blockChances)
        {
            cumulative += b.chance;
            if (rand <= cumulative)
                return b.data;
        }

        return blockChances[blockChances.Count - 1].data;
    }

    private float GetBlockWidth(Block prefab)
    {
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.x : 1f;
    }

    private float camOffsetX()
    {
        return Camera.main.transform.position.x;
    }
}
