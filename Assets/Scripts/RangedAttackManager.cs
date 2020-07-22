using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackManager : MonoBehaviour
{

    public GameObject SpellPrefab;
    public Camera Camera;

    public RangedAttackSO FireBall;
    public RangedAttackSO FrostBolt;
    public RangedAttackSO PoisonBolt;
    public RangedAttackSO ShadowBolt;


    private Transform _transform;
    private Transform _playerTransform;
    private PlayerInput _playerInput;
    private float shoot = 0f;
    private float force = 20f;


    private void Awake() 
    {
        _transform = transform;
        
    }

    private void Start()
    {
        _playerTransform = _transform.GetComponentInParent<Transform>();
        _playerInput = _transform.GetComponentInParent<Player>().PlayerInput;
    }
    

    private void FixedUpdate()
    {
        shoot = _playerInput.Attack.Shoot.ReadValue<float>();
        if (shoot != 0f)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Gets mousePos in world coordinates, subtracts player position and normalises value
        // (this removes faster bullets if you click further away)
        Vector2 mousePosRaw = Camera.ScreenToWorldPoint(_playerInput.Mouse.Position.ReadValue<Vector2>());
        Vector3 mousePos = new Vector3(mousePosRaw.x, mousePosRaw.y, 0f);
        Vector3 direction = mousePos - _transform.position;
        direction.Normalize();

        // Calculates the shooting angle and creates a rotation from it
        float shootAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion rotation = Quaternion.Euler(0, 0, shootAngle - 35f);

        GameObject projectile = CreateByIndex(0);
        Rigidbody2D rb = projectile.transform.GetComponent<Rigidbody2D>();

        projectile.transform.SetParent(_playerTransform);
        projectile.transform.position = _playerTransform.position;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }


    public enum SpellID
    {
        FireBall,
        FrostBolt,
        PoisonBolt,
        ShadowBolt,
    };


    public GameObject CreateByIndex(int index)
    {
        GameObject newSpell = Instantiate(SpellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newSpell.GetComponent<RangedAttack>().RangedAttackType = FrostBolt;
        return newSpell;
    }

}