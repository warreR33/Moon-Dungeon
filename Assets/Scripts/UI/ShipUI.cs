using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipUI : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private Image healthPrefab;
    [SerializeField] private Transform healthParent;
    private Image[] healthImages;

    [Header("Balas")]
    [SerializeField] private Image bulletPrefab;
    [SerializeField] private Transform bulletParent;
    private Image[] bulletImages;

    [SerializeField] private TMP_Text totalAmmoText;

    [Header("Recarga")]
    [SerializeField] private Image reloadBar;

    private int maxHealth;
    private int maxBullets;

    public void InitHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        healthImages = new Image[maxHealth];

        for (int i = 0; i < maxHealth; i++)
        {
            Image img = Instantiate(healthPrefab, healthParent);
            img.gameObject.SetActive(true);
            healthImages[i] = img;
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            healthImages[i].enabled = i < currentHealth;
        }
    }

    public void InitBullets(int maxBullets)
    {
        this.maxBullets = maxBullets;
        bulletImages = new Image[maxBullets];

        for (int i = 0; i < maxBullets; i++)
        {
            Image img = Instantiate(bulletPrefab, bulletParent);
            img.gameObject.SetActive(true);
            bulletImages[i] = img;
        }
    }

    public void UpdateBullets(int currentBullets, int totalBullets)
    {
        for (int i = 0; i < maxBullets; i++)
        {
            bulletImages[i].enabled = i < currentBullets;
        }

        if (totalAmmoText != null)
            totalAmmoText.text = totalBullets.ToString();
    }

    public void UpdateReloadBar(float progress)
    {
        if (reloadBar != null)
            reloadBar.fillAmount = progress;
    }
}
