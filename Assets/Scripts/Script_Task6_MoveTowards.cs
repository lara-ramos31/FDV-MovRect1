using Unity.VisualScripting;
using UnityEngine;

public class Script_Task6_MoveTowards : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir; // public to watch it in the editor

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(goalObj.position);
        if (debugRay) {
            goalDir = goalObj.position - transform.position;
            Debug.DrawRay(transform.position, goalDir, Color.red);
        }
        transform.position = Vector3.MoveTowards(transform.position, goalObj.position, speed * Time.deltaTime);
    }
}
