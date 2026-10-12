using UnityEngine;
using UnityEngine.Animations;

public class Script_Task4_ObjectiveObject : MonoBehaviour
{
    public float baseSpeed = 1.0f;
    public float boostSpeed = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 moveVector;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float forward = Input.GetAxis("Vertical");
        float sideways = Input.GetAxis("Horizontal");
        float currentSpeed = Input.GetKey(KeyCode.Space) ? boostSpeed : baseSpeed;
        moveVector.Set(sideways, 0, forward);
        transform.Translate(Time.deltaTime * currentSpeed * moveVector);
    }
}
