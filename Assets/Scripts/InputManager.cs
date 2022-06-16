using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private Transform gun;
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerHealth health;
    public PlayerInput.UIActions ui;
    public GunProjectile gunProjectile;
    
    
    void Awake()
    {
        gun = GameObject.Find("gun").transform;
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        ui = playerInput.UI;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        health = GetComponent<PlayerHealth>();
        gunProjectile = gun.GetComponent<GunProjectile>();
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Sprint.performed += ctx => motor.Sprint();
        ui.Damage.performed += ctx => health.TakeDamage(UnityEngine.Random.Range(5, 10));
        ui.Heal.performed += ctx => health.RestoreHealth(UnityEngine.Random.Range(5, 10));
        onFoot.Shoot.performed += ctx => gunProjectile.Shoot();
        onFoot.Reload.performed += ctx => gunProjectile.Reload();
    }


    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
        ui.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        ui.Disable();
    }
}
