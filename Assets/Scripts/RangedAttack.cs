using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{

    public GameObject FireBallPrefab;
    public Camera Camera;

    private Transform _transform;
    private Transform _playerTransform;
    private Transform _firePoint;
    private PlayerInput _playerInput;
    private float shoot= 0f;
    private float force = 20f;

    private void Awake() 
    {
        _transform = transform;
        _playerTransform = _transform.GetComponentInParent<Transform>();
        
    }

    private void Start()
    {
        _firePoint = _playerTransform;
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

        float shootAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion rotation = Quaternion.Euler(0, 0, shootAngle - 35f);

        GameObject projectile = Instantiate(FireBallPrefab, _transform.position, rotation);

        Rigidbody2D rb = projectile.transform.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * force, ForceMode2D.Impulse);

    }
}
