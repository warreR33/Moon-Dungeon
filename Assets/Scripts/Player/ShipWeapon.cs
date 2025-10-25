using UnityEngine;
using System.Collections;

public class ShipWeapon : MonoBehaviour
{
    [Header("Arma equipada")]
    [SerializeField] private WeaponData equippedWeapon;

    [Header("Referencias")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private ShipView shipView;

    [SerializeField] private ShipUI ui;

    private int currentAmmo;
    private int totalAmmo;
    private float fireTimer;
    private bool isReloading;


    public void Initialize(ShipView view, ShipUI ui)
    {
        shipView = view;
        this.ui = ui;

        if (equippedWeapon != null)
            InitializeWeapon(equippedWeapon);

        if (ui != null)
        {
            ui.InitBullets(equippedWeapon.magazineSize);
            ui.UpdateBullets(currentAmmo, totalAmmo);
        }
    }

    private void Start()
    {
        if (equippedWeapon != null)
            InitializeWeapon(equippedWeapon);

        if (ui != null)
        {
            ui.InitBullets(equippedWeapon.magazineSize);
            ui.UpdateBullets(currentAmmo, totalAmmo);
        }
    }

    private void Update()
    {
        if (equippedWeapon == null) return;

        fireTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
            TryShoot();

        if (Input.GetMouseButton(0))
            TryShoot();
    }

    private void InitializeWeapon(WeaponData weapon)
    {
        equippedWeapon = weapon;
        currentAmmo = weapon.magazineSize;
        totalAmmo = weapon.totalAmmo;
        fireTimer = 0f;
        isReloading = false;
    }

    private void TryShoot()
    {
        if (isReloading || fireTimer > 0f) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        Shoot();
    }

    private void Shoot()
    {
        if (equippedWeapon.bulletPrefab == null || firePoint == null) return;
        string poolTag = equippedWeapon.bulletPrefab.name;

        GameObject bulletGO = ObjectPool.Instance.SpawnFromPool(poolTag, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
            bullet.SetDamage(equippedWeapon.damage);

        fireTimer = equippedWeapon.fireRate;
        currentAmmo--;

        ui?.UpdateBullets(currentAmmo, totalAmmo);
        shipView?.PlayShootAnimation();
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        float timer = 0f;

        while (timer < equippedWeapon.reloadTime)
        {
            timer += Time.deltaTime;
            ui?.UpdateReloadBar(timer / equippedWeapon.reloadTime);
            yield return null;
        }

        int ammoToLoad = Mathf.Min(equippedWeapon.magazineSize, totalAmmo);
        currentAmmo = ammoToLoad;
        totalAmmo -= ammoToLoad;

        isReloading = false;
        ui?.UpdateBullets(currentAmmo, totalAmmo);
        ui?.UpdateReloadBar(0f);
    }

    // 🔹 Permite cambiar de arma dinámicamente
    public void EquipWeapon(WeaponData newWeapon)
    {
        InitializeWeapon(newWeapon);
    }

    // 🔹 Información pública (útil para UI)
    public int CurrentAmmo => currentAmmo;
    public int TotalAmmo => totalAmmo;
    public WeaponData EquippedWeapon => equippedWeapon;
}
