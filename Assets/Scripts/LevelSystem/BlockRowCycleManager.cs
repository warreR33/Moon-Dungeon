using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockRowCycleManager : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private BlockRowSpawner rowSpawner;

    [Header("Cycle Settings")]
    [SerializeField] private float rowStepY = 1.5f;       // Cuánto bajan las filas cada ciclo
    [SerializeField] private float moveSpeed = 3f;        // Velocidad de bajada
    [SerializeField] private float timeBetweenRows = 3f;  // Tiempo entre la aparición de cada fila

    private List<Transform> activeRows = new List<Transform>();

    private void Start()
    {
        if (rowSpawner == null)
        {
            Debug.LogError("No se asignó un RowSpawner al CycleManager.");
            return;
        }

        StartCoroutine(SpawnCycle());
    }

    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            GameObject newRow = new GameObject("BlockRow");
            newRow.transform.parent = transform;

            rowSpawner.SpawnRowInParent(newRow.transform);
            activeRows.Add(newRow.transform);

            yield return StartCoroutine(MoveRowsDown());
            yield return new WaitForSeconds(timeBetweenRows);
        }
    }

    private IEnumerator MoveRowsDown()
    {
        bool moving = true;

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> targetPositions = new List<Vector3>();

        foreach (Transform row in activeRows)
        {
            startPositions.Add(row.position);
            targetPositions.Add(row.position + Vector3.down * rowStepY);
        }

        float t = 0f;
        while (moving)
        {
            t += Time.deltaTime * moveSpeed;
            for (int i = 0; i < activeRows.Count; i++)
            {
                if (activeRows[i] != null)
                {
                    activeRows[i].position = Vector3.Lerp(startPositions[i], targetPositions[i], t);
                }
            }

            if (t >= 1f)
                moving = false;

            yield return null;
        }
    }
}
