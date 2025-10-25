using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance { get; private set; }

    [Header("Referencias principales")]
    [SerializeField] private ShipController controller;
    [SerializeField] private ShipView view;
    [SerializeField] private ShipWeapon weapon;
    [SerializeField] private ShipUI ui;

    public ShipController Controller => controller;
    public ShipView View => view;
    public ShipWeapon Weapon => weapon;
    public ShipUI UI => ui;
    public ShipModel Model { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Inicializar modelo una sola vez
        Model = new ShipModel();
        Model.Initialize();

        // Inicializar controlador
        if (controller != null)
            controller.Initialize(view, Model);

        // Inicializar arma
        if (weapon != null)
            weapon.Initialize(view, ui);

        // Inicializar UI y suscribirse al evento
        if (ui != null)
        {
            ui.InitHealth(Model.MaxHealth);
            ui.UpdateHealth(Model.CurrentHealth);
            Model.OnHealthChanged += ui.UpdateHealth;
        }
    }

}
