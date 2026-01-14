using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Управление оружием игрока
/// </summary>
public class PlayerWeaponController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject[] objWeapons;

    // TODO:
    // Инвентарь -> взять оружее -> 
    // создать выбранные оружия (Instantiate) -> 
    // далее примерно также 

    private StarterAssets_InputSystem _inputs; // или через фасад StarterAssetsInputs 
    private int _selectedIndexWeapon = 0; // или ссылку на выбранное оружее 
    private Weapon[] _weapons;

    private void OnEnable()
    {
        _inputs = new();
        _inputs.Enable();

        _inputs.Shooting.Fire.performed += OnFire;
        _inputs.Shooting.SwitchWeapon.performed += OnSwitchWeapon;
    }

    private void Start()
    {
        InitWeapons();
        ActivSelectedWeapon();
    }

    private void InitWeapons()
    {
        _weapons = new Weapon[objWeapons.Length];

        for (int i = 0; i < objWeapons.Length; i++)
        {
            _weapons[i] = objWeapons[i].GetComponent<Weapon>();
        }
    }

    private void ActivSelectedWeapon()
    {
        for (int i = 0; i < objWeapons.Length; i++)
        {
            objWeapons[i].SetActive(false);
        }

        objWeapons[_selectedIndexWeapon].SetActive(true);
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        _weapons[_selectedIndexWeapon].TryShoot();
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        _selectedIndexWeapon = _selectedIndexWeapon < _weapons.Length - 1 ?
            _selectedIndexWeapon + 1 : 0;

        ActivSelectedWeapon();
    }
}
