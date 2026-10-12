using UnityEngine;

public class Script_Task5_MovingObject : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir; // public to watch it in the editor

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(goalObj.position);
        goalDir = goalObj.position - transform.position;
        if (debugRay) {
            Debug.DrawRay(transform.position, goalDir, Color.red);
        }
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}
