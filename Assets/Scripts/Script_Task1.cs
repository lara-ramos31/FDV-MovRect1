using UnityEngine;

public class ScriptTask1 : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 0.5f;
    public Vector3 goalDir; // public to watch it in the editor

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalDir = goalObj.position - transform.position;
        // goalDir = speed * goalDir;
    }

    // Update is called once per frame
    void Update()
    {
        goalDir = speed * goalDir;
        transform.Translate(goalDir);
    }
}
