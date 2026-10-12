using UnityEngine;

public class ScriptTask3 : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir; // public to watch it in the editor

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.LookAt(goalObj.position);
    }

    // Update is called once per frame
    void Update()
    {
        goalDir = goalObj.position - transform.position;
        if (debugRay) {
            Debug.DrawRay(transform.position, goalDir, Color.red);
        }
        transform.Translate(goalDir.normalized * speed * Time.deltaTime, Space.World);
    }
}
