using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerPhysics : MonoBehaviour
{
    Rigidbody rb;
    static float globalGravity = -9.81f;

    [Header("Cooldown Time")]
    [SerializeField]
    float timeForAction;
    float _timeForAction;
    bool cooldownAction;

    [Header("Force Values")]
    [SerializeField]
    float jumpForce;
    float gravityScale;
    [SerializeField]
    float fallMultiplier;
    float lowJumpMultiplier;
    [SerializeField]
    float dashForce;

    bool inGround;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Start()
    {
        
        SetValuesBase();
    }

    void Update()
    {
        //tests only
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.Z))
            Dash();
    }

    void FixedUpdate()
    {
        Cooldown();
        BetterJumpControl();

        Debug.Log(timeForAction);
    }

    void SetValuesBase()
    {
        rb.freezeRotation = true;
        dashForce = dashForce == 0 ? 5 : dashForce;
        
        cooldownAction = false;
        timeForAction = timeForAction == 0 ? 5 : timeForAction;
        _timeForAction = timeForAction;

        jumpForce = jumpForce == 0 ? 10 : jumpForce;
        inGround = true;

        fallMultiplier = fallMultiplier == 0 ? 3.5f : fallMultiplier;
        lowJumpMultiplier = 2f;
    }

    public void Jump()
    {
        if(inGround)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void Dash()
    {
        if (!cooldownAction)
        {
            cooldownAction = true;
            rb.AddForce(transform.forward * dashForce * 100, ForceMode.Force);
        }
    }

    void Cooldown()
    {
        if(cooldownAction)
        {
            if (timeForAction > 0)
            {
                timeForAction -= Time.fixedDeltaTime;
            }
            else
            {
                cooldownAction = false;
                timeForAction = _timeForAction;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        inGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        inGround = false;
    }

    void BetterJumpControl()
    {
        if (rb.velocity.y < 0)
        {
            gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0)
        {
            gravityScale = lowJumpMultiplier;
        }
        else
            gravityScale = 1;

        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
