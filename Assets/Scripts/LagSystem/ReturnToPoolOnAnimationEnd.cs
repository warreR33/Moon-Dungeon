using UnityEngine;

public class ReturnToPoolOnAnimationEnd : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
