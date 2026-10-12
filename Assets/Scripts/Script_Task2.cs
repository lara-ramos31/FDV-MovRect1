using UnityEngine;

public class ScriptTask2 : MonoBehaviour
{
    public Transform goalObj;
    public float speed;
    public Vector3 goalDir; // public to watch it in the editor

    // Update is called once per frame
    void Update()
    {
        goalDir = goalObj.position - transform.position;
        transform.Translate(goalDir.normalized * speed * Time.deltaTime);
    }
}
