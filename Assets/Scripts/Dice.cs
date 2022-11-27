using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dice : MonoBehaviour
{
    public int id = 0;
    public float spinForce = 100f;
    public UnityEvent<int, int> OnDice;

    Rigidbody body;
    Vector3 startPos;
   

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        startPos = transform.localPosition;
    }
    private void Update()
    {
    }

    [ContextMenu("Reset")]
    public void ResetDice()
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.useGravity = false;
        body.isKinematic = true;

        //transform.eulerAngles = Vector3.zero;
        transform.localPosition = startPos;
    }
    [ContextMenu("Spin dice")]
    public void Spin()
    {
        StartCoroutine(UpateDice());
    }

    IEnumerator UpateDice()
    {

        ResetDice();
        body.maxAngularVelocity = spinForce * 2f;

        body.isKinematic = false;

        Vector3 spin = new Vector3(Random.value, Random.value, Random.value);
        body.AddTorque(spin * spinForce * (1 + Random.value), ForceMode.Impulse);
        float delay = 1f + Random.value;
        while (delay > 0f)
        {
            yield return null;
            delay -= Time.deltaTime;
        }

        delay = 3;

        body.useGravity = true;

        while (!body.isKinematic)
        {
            yield return new WaitForFixedUpdate();
            delay -= Time.fixedDeltaTime;
            if (delay < 0)
            {
                Spin();
                break;
            }
            if (body.velocity.magnitude < 0.03f && body.angularVelocity.magnitude < 0.3f)
            {
                body.isKinematic = true;
                body.useGravity = false;

                Vector3Int norm = Vector3Int.zero;
                norm.x = Mathf.RoundToInt(transform.localEulerAngles.x / 90f);
                norm.z = Mathf.RoundToInt(transform.localEulerAngles.z / 90f);

                norm.x = norm.x == 4 ? 0 : norm.x * 90;
                norm.z = norm.z == 4 ? 0 : norm.z * 90;

                transform.localEulerAngles = new Vector3(norm.x, transform.localEulerAngles.y, norm.z);

                OnDice.Invoke(id, Number(norm));
                break;
            }
 
        }

    }

    void StartDice()
    {
        body.useGravity = true;
        body.isKinematic = false;
    }

    int Number(Vector3Int value)
    {
        return value.x switch
        {
            0 => value.z switch
            {
                90 => 6,
                180 => 5,
                270 => 1,
                _ => 2,
            },
            90 => 3,
            270 => 4,
            _ => 0,
        };
    }
    
}
