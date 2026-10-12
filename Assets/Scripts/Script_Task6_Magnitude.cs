using UnityEngine;

public class Script_Task6_Magnitude : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir; // public to watch it in the editor
    public float accuracy = 0.01f;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(goalObj.position);
        goalDir = goalObj.position - transform.position;
        if (debugRay) {
            Debug.DrawRay(transform.position, goalDir, Color.red);
        }
        if (goalDir.magnitude > accuracy) {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }
    }
}
