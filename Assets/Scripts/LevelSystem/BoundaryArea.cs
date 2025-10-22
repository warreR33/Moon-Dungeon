using UnityEngine;

[System.Serializable]
public class BoundaryArea
{
    public Transform transform;
    public Vector2 size = new Vector2(1, 1);

    public bool IsInside(Vector2 point)
    {
        if (transform == null) return false;

        Vector2 pos = transform.position;
        Vector2 half = size * 0.5f;

        bool inside = point.x > pos.x - half.x && point.x < pos.x + half.x &&
                    point.y > pos.y - half.y && point.y < pos.y + half.y;

        return inside;

    }

    public Vector2 GetClosestPoint(Vector2 point)
    {
        Vector2 pos = transform.position;
        Vector2 half = size * 0.5f;

        // Si está dentro, lo empujamos hacia afuera
        if (IsInside(point))
        {
            float dxLeft = Mathf.Abs(point.x - (pos.x - half.x));
            float dxRight = Mathf.Abs(point.x - (pos.x + half.x));
            float dyDown = Mathf.Abs(point.y - (pos.y - half.y));
            float dyUp = Mathf.Abs(point.y - (pos.y + half.y));

            float minDist = Mathf.Min(dxLeft, dxRight, dyDown, dyUp);

            if (minDist == dxLeft) point.x = pos.x - half.x;
            else if (minDist == dxRight) point.x = pos.x + half.x;
            else if (minDist == dyDown) point.y = pos.y - half.y;
            else if (minDist == dyUp) point.y = pos.y + half.y;
        }

        return point;
    }

    public Vector2 GetCorrectedPosition(Vector2 point, ref Vector2 velocity)
    {
        Vector2 pos = transform.position;
        Vector2 half = size * 0.5f;

        if (IsInside(point))
        {
            float dxLeft = Mathf.Abs(point.x - (pos.x - half.x));
            float dxRight = Mathf.Abs(point.x - (pos.x + half.x));
            float dyDown = Mathf.Abs(point.y - (pos.y - half.y));
            float dyUp = Mathf.Abs(point.y - (pos.y + half.y));

            float minDist = Mathf.Min(dxLeft, dxRight, dyDown, dyUp);

            // Corrige posición y ajusta velocidad según el lado de impacto
            if (minDist == dxLeft)
            {
                point.x = pos.x - half.x;
                if (velocity.x > 0) velocity.x = 0; // Bloquear movimiento hacia el borde
            }
            else if (minDist == dxRight)
            {
                point.x = pos.x + half.x;
                if (velocity.x < 0) velocity.x = 0;
            }
            else if (minDist == dyDown)
            {
                point.y = pos.y - half.y;
                if (velocity.y > 0) velocity.y = 0;
            }
            else if (minDist == dyUp)
            {
                point.y = pos.y + half.y;
                if (velocity.y < 0) velocity.y = 0;
            }
        }

        return point;
    }



#if UNITY_EDITOR
    public void DrawGizmos(Color color)
    {
        if (transform == null) return;
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, size);
    }
#endif
}
