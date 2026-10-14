using UnityEngine;

public class Script_Task8 : MonoBehaviour
{
    public float speed = 1.0f;
    public float accuracy = 0.5f;
    public bool debugRay = true;
    public Vector3 goalDir; // public to watch it in the editor

    public GameObject[] goalObjs = null;
    private int goalIdx = 0;

    public Material selectedMaterial;
    public Material inactiveMaterial;

    void Start()
    {
        goalObjs = GameObject.FindGameObjectsWithTag("Waypoint");
        if (goalObjs == null || goalObjs.Length == 0) {
            Debug.LogError("Error[Task8]: No waypoints found!");
            return;
        }
        UpdateMaterial(goalObjs[goalIdx], selectedMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        if (goalObjs == null || goalObjs.Length == 0) { return; }
        GameObject goalObj = goalObjs[goalIdx];
        goalDir = goalObj.transform.position - transform.position;
        if (goalDir.magnitude < accuracy) { // move to the next one
            UpdateMaterial(goalObj, inactiveMaterial);
            goalIdx = (goalIdx + 1) % goalObjs.Length;
            goalObj = goalObjs[goalIdx];
            goalDir = goalObj.transform.position - transform.position;
            UpdateMaterial(goalObj, selectedMaterial);
        }
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(goalDir, transform.up);
        float speedScale = speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(from, to, speedScale);
        if (debugRay) {
            Debug.DrawRay(transform.position, goalDir, Color.red);
            Debug.DrawRay(transform.position, transform.forward, Color.green);
        }
        transform.Translate(speedScale * Vector3.forward);
    }

    void UpdateMaterial(GameObject obj, Material newMat)
    {
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer == null) { return; }
        meshRenderer.material = newMat;
    }
}
