using UnityEngine;

public class ShipView : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator shipAnimator;
    [SerializeField] private Animator enginesAnimator;

    public void UpdateThrustAnimation(bool isThrusting)
    {
        if (enginesAnimator != null)
            enginesAnimator.SetBool("IsThrusting", isThrusting);
    }

    public void UpdateTiltAnimation(Vector2 input, Transform shipTransform)
    {
        if (shipAnimator == null) return;

        Vector2 localInput = shipTransform.InverseTransformDirection(input);
        float tilt = Mathf.Clamp(localInput.x, -1f, 1f);
        shipAnimator.SetFloat("Tilt", tilt);
    }

    public void PlayDamageFlash()
    {
        if (shipAnimator != null)
            shipAnimator.SetTrigger("Damage");
    }

    public void PlayShootAnimation()
    {
        if (shipAnimator != null)
        {
            //shipAnimator.SetTrigger("Shoot");
        }
            
    }
}
