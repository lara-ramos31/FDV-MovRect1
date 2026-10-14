using UnityEngine;

public class Script_Task7 : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public float accuracy = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir; // public to watch it in the editor

    // Update is called once per frame
    void Update()
    {
        goalDir = goalObj.position - transform.position;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(goalDir, transform.up);
        float speedScale = speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(from, to, speedScale);
        if (debugRay) {
            Debug.DrawRay(transform.position, goalDir, Color.red);
            Debug.DrawRay(transform.position, transform.forward, Color.green);
        }
        if (goalDir.magnitude > accuracy) {
            transform.Translate(speedScale * Vector3.forward);
        }
    }
}
