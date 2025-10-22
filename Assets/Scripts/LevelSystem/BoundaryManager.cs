using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    public static BoundaryManager Instance;

    [SerializeField] private List<BoundaryArea> boundaries = new List<BoundaryArea>();
    public List<BoundaryArea> Boundaries => boundaries;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsInsideBoundary(Vector2 position)
    {
        foreach (var b in boundaries)
        {
            if (b.IsInside(position))
                return true;
        }
        return false;
    }

    public Vector2 GetCorrectedPosition(Vector2 position, ref Vector2 velocity)
    {
        foreach (var b in boundaries)
        {
            if (b.IsInside(position))
            {
                return b.GetCorrectedPosition(position, ref velocity);
            }
        }
        return position;
    }

    public BoundaryArea GetCollidingBoundary(Vector2 position)
    {
        foreach (var b in boundaries)
        {
            if (b.IsInside(position))
                return b;
        }
        return null;
    }

    public BoundaryArea GetBoundaryAtPosition(Vector2 position)
    {
        foreach (var b in boundaries)
        {
            if (b.IsInside(position))
                return b;
        }
        return null;
    }




#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (boundaries == null) return;
        foreach (var b in boundaries)
            b.DrawGizmos(Color.red);
    }
#endif
}
