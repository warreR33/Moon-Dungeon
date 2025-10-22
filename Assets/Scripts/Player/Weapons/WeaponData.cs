using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info General")]
    public string weaponName;

    [Header("Stats")]
    public float damage = 10f;
    public int magazineSize = 10;
    public int totalAmmo = 50;
    public float fireRate = 0.2f;
    public float reloadTime = 1.5f;

    [Header("Bullet Prefab")]
    public GameObject bulletPrefab;
}
