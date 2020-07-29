using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed;

    // TESTING
    [SerializeField] private Player _player;

    private Transform _transform;

    private Vector2 _dest;
    private bool _destArrived = false;

    private void Awake() {
        _transform = transform;
    }

    private void Update()
    {
        Move();
        // print(_dest);
    }

    private void Move() {

        Vector2 direction = Vector2.zero;
        float timeDelta = Time.deltaTime;

        if (_destArrived | _dest == null | direction == Vector2.zero) 
        { 
            SelectNewDestination(); 
        }

        _destArrived = false;

        direction = _rb.position - _dest;
        direction.Normalize();

        _rb.position += new Vector2(-direction.x * _moveSpeed * timeDelta, -direction.y * _moveSpeed * timeDelta);
    }

    private void SelectNewDestination()
    {
        print("new dest");
        _dest = new Vector2((int) Random.Range(-5, 5), (int) Random.Range(-5, 5));
        // _dest = _player.GetComponent<Rigidbody2D>().position;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        _destArrived = true;
    }

}
