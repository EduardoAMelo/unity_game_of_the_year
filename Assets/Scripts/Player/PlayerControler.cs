using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerControler : MonoBehaviour
{   
    private Rigidbody2D _rigidbody;
    private Animator anim;
    private float _speed = 3f;
    private bool facingRight = false;
    private bool isBack = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleAnimations();
        HandleFlip();
    }

    private void HandleAnimations()
    {
        bool isMoving;
        isMoving = _rigidbody.linearVelocity.x != 0 || _rigidbody.linearVelocity.y != 0 ;
        if (_rigidbody.linearVelocity.y != 0)
        {
            isBack = _rigidbody.linearVelocity.y > 0;
        }

        if (_rigidbody.linearVelocity.x > 0 && _rigidbody.linearVelocity.y <= 0)
            isBack = false;

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isBack", isBack);


    }

    private void HandleFlip()
    {
        if (_rigidbody.linearVelocity.x > 0 && facingRight == false)
            Flip();
        else if (_rigidbody.linearVelocity.x < 0 && facingRight == true)
            Flip();
        

    }       

    private void Flip()
    {
        transform.Rotate(0,180,0);
        facingRight = !facingRight;
    }

    private void OnMove(InputValue value)
    {
        _rigidbody.linearVelocity = value.Get<Vector2>() * _speed;
    }
}
