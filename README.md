# FDV-MovRect1

*Fundamentos del Desarrollo de Videojuegos'* assignment: linear motion exercises.

![A demonstration of the assignment's (mostly complete) Test Scene](Media/testscene.gif)

## Task 1
> Create a script that moves the object towards a fixed point marked as objective, using the `Translate()` method from the `Transform` class. The objective must be a public variable, so that we can manipulate it in the inspector and see different values' effects in the coordinates. Use `this.transform.Translate(goal)` at `Start()`, which only moves once. Experiment with the following options:
> - a. Add `this.transform.Translate(goal)` to `Update()`, then keep on multiplying `goal = goal * 0.5f` so that you'll keep on performing smaller jumps each time. What happens if you place `goal = goal * 0.5f` in `Start()`? What happens if you include `goal = goal * 2.0f` in `Update()`?
> - b. Set the objective's Y coordinate to 0.
> - c. Set the objective's Y coordinate to non-zero.
> - d. Modify the script so that the object flies off the ground like a plane.

This first task proposes the use of `transform.Translate()` to directly apply a translation transform to modify an object's position in the scene. Using this method only at the `Start()` method moves the object directly to the objective's position.

```c#
public class ScriptTask1 : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 0.5f;
    public Vector3 goalDir; // public to watch it in the editor

    void Start()
    {
        goalDir = goalObj.position - transform.position;
    }

    void Update()
    {
        goalDir = speed * goalDir;
        transform.Translate(goalDir);
    }
}
```

Moving the translation to `Update()` and constantly updating the `goalDir` vector right before the translation does, in fact, keep applying smaller and smaller translations, which gives a fast, non-constant (smooth) effect to the motion. Speed values within the range (0, 1) provide the expected results. Speed values over 1, like `2.0f`, will always make the object miss the objective. The code works fine for different Y values.

![A scene demonstrating the first task script's behaviour](Media/task1.gif)

## Task 2

> The objective is not an objective per se, but rather a **direction** in which we wish to move. A vector's relevant information is its direction. Normalized vectors preserve their direction but their scale does not affect the movement. A consistent movement must be achieved so that scale does not affect translation. At the same time, the character's route between frames must not have space-time aberrations. The relationship between speed, space and time must be taken into account. The time inbetween frames can be obtained with `Time.deltaTime`. In this exercise we pretend to give consistency to the character's movement:
> - a. Substitute the movement's direction for its normalized equivalent. This is achieved using `Vector3`'s `normalized` method: `this.transform.Translate(goal.normalized)`.
> - b. We can multiply the normalized vector by a speed value to determine how fast the character moves: 
> ```c#
> public float speed = 0.1f; 
> this.transform.Translate(goal.normalized * speed);
> ```
> - c. Even though these speeds might appear consistent, they are not. This is because they depend on the speed in which the `Update()` method is executed. The time between an object's two `Update()` calls is not always necessarily the same, so the speed can have inconsistencies. Even though we might not perceive it in low-complexity applications, we must use `this.transform.Translate(goal.normalized * speed * Time.deltaTime)` to smooth the movement, as `Time.deltaTime` is the time that has passed since last frame.

```c#
public class ScriptTask2 : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public Vector3 goalDir; // public to watch it in the editor

    void Update()
    {
        goalDir = goalObj.position - transform.position;
        transform.Translate(goalDir.normalized * speed * Time.deltaTime);
    }
}
```

For this second task, we want the speed to be constant. This is achieved by decoupling it from the distance to the objective (by normalizing the `goalDir` vector), and from the time between `Update()` calls (by multiplying it by `Time.deltaTime`). No matter how far away the two objects are, or how fast the computer running the game is, the movement speed will be constant and predictable.

![A scene demonstrating the second task script's behaviour](Media/task2.gif)

## Task 3

> Instead of using a direction as an objective, we'll now move towards a true goal `position`. We'll add it as a `public` field in the class to be able to configure it using the inspector. We'll also add a field to set the character's speed from the inspector. Even though we must move towards a point in space, the `Translate()` method must receive the movement's direction, which for any two points is obtained by subtracting the farther one from the closest one. Lastly, if a character is not facing the objective (it might even be behind it), the displacement will be smooth but its mesh's orientation will not be consistent. This is why we'll need to rotate it so that its local Z axis (*forward*) points towards the goal. `Transform`'s `LookAt()` method will help with this. In this case, therefore, to move towards a point in space at a set speed:
> - a. We make the objective a `public Transform goal` variable, and we add `public float speed = 1.0f`.
> - b. We rotate the character so that its movement is always forward by using `transform.LookAt(goal.position)` at `Start` so that it turns first and then it moves.
> - c. The direction in which we have to move is determined by the difference between the goal position and our position: `Vector3 direction = goal.position - this.transform.position;`.
> - d. If we execute it then, since the character's orientation is going to change, the `Translate()` call is not going to work properly, as the `direction` vector was calculated using the world-aligned reference system. By rotating the character we change that reference, and so we'll have to change the direction. The character's axes and the world's are not aligned, so the movement must be performed relative to the world's reference. `this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World)`.
> - e. Add `Debug.DrawRay(this.transform.position, direction, Color.red)` for debugging to ensure the direction is properly computed.

This third exercise opts to turn the object towards the goal before starting to move it. Since the object might be misaligned with the world reference system after rotating, the main difference when calling `transform.Translate()` is that the world refernce must be explicitly specified.

```c#
using UnityEngine;

public class ScriptTask3 : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir; // public to watch it in the editor

    void Start()
    {
        transform.LookAt(goalObj.position);
    }

    void Update()
    {
        goalDir = goalObj.position - transform.position;
        if (debugRay) {
            Debug.DrawRay(transform.position, goalDir, Color.red);
        }
        transform.Translate(goalDir.normalized * speed * Time.deltaTime, Space.World);
    }
}
```

Note that a `public debugRay` flag was added to the script to show a red line between the object and the goal to ensure the direction is computed properly.

![A scene demonstrating the third task script's behaviour](Media/task3.gif)

## Task 4

> Add a cube in the scene to serve as objective, and make it so that its movement is controlled with the keyboard's arrow keys. Place a character in the scene to follow the controlled cube.
> - Write a script that makes the character follow the cube continuously without physics simulation.
> - Add a `public` field that allows to set the movement speed via the object inspector.
> - Use the Space key to increment the movement speed during gameplay.

For this fourth task, two independent scripts had to be written. The first script corresponds to the objective:
```c#
using UnityEngine;

public class Script_Task4_ObjectiveObject : MonoBehaviour
{
    public float baseSpeed = 1.0f;
    public float boostSpeed = 3.0f;
    private Vector3 moveVector;

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
```

The script uses the Legacy Input System, since it is what the class slides include. As such, the project settings had to be changed from the newer default to this legacy option.

Each frame, the objective does the following:
- First, it checks whether keys assigned to the vertical and horizontal axes are pressed. This returns floating point values between 0 and 1.
- Then, it checks whether the Space key is pressed. I set two configurable speed values. The `baseSpeed` is for normal movement, and the `boostSpeed` is set when the Space key is detected.
- Finally, the translation applied to the objective uses all computed values. The `moveVector` is set to use the vertical axis value for the Z (forward) component, and the horizontal axis value for the X (sideways) component. Note: the `moveVector` *should* be normalized if we don't want the objective to move a bit faster when moving diagonally, but I prefer it the way it is.

```c#
using UnityEngine;

public class Script_Task4_MovingObject : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir;

    void Update()
    {
        transform.LookAt(goalObj.position);
        goalDir = goalObj.position - transform.position;
        if (debugRay) {
            Debug.DrawRay(transform.position, goalDir, Color.red);
        }
        transform.Translate(goalDir.normalized * speed * Time.deltaTime, Space.World);
    }
}
```

The character's script is mostly the same as the previous task, except we now have to add the `LookAt()` call each frame since the objective can now move.

![A scene demonstrating the fourth task script's behaviour](Media/task4.gif)

## Task 5

> As an alternative solution to the previous task, in this exercise we'll work on linear motion towards the objective by always making the character move in a forward straight line. To achieve this, the character must rotate towards the objective and then advance in the `forward` direction. In this case, the `Transform.Translate()` method has two ways to perform the translation. We'll solve it by rotating the character with `LookAt()` and then translating over its local `forward` axis, which matches the default value of `Translate`'s `relativeTo` parameter.
> - Write a script that rotates the character towards its objective in order to reach it by displacing it over its `forward` vector.

```c#
using UnityEngine;

public class Script_Task5_MovingObject : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir;

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
```

This script's only difference with the previous one is the use of the forward vector instead of the goal direction (it's still computed for the debug ray).

## Task 6

> When we execute the script, the character computes the objective's direction and moves towards it, but it can't stop moving, producing *jittering*. This is because we are still inside the loop, computing the direction and moving towards it. Most of the time we will not be able to make our character move to the objective's exact position, so it constantly oscillates around that position. In the last frame, the `speed * Time.deltaTime` step will be larger than the remaining distance, so the character overshoots a little. The next frame, the objective is behind: `LookAt` turns 180º and advances again, overshooting egain. It repeats every frame: the character jitters around the objective and turns constantly, which is very noticeable. To prevent this, we must add some kind of tolerance range computation. We'll include a global `public float accuracy = 0.01f`, and a condition `if (direction.magnitude > accuracy)`. Even with `accuracy`, the character can still jitter if the speed is too high.
> - a. Control the jittering using the magnitude of the direction.
> - b. Since the direction gives us the separation between objective and character, we can also control jittering using the distance between the two.
> - c. Use `MoveTowards()`, which never overshoots: if the step is greater than what's left, if leaves the object right at the objective.

The task proposes two alternatives to mitigate the jittering effect on the movement.

```c#
using UnityEngine;

public class Script_Task6_Magnitude : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir;
    public float accuracy = 0.01f;

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
```

The first method checks whether the distance to the goal is below some threshold we call `accuracy`. This method seems to eliminate most of the jitter, but I observed there's a chance it still appears on certain situations.

```c#
using UnityEngine;

public class Script_Task6_MoveTowards : MonoBehaviour
{
    public Transform goalObj;
    public float speed = 1.0f;
    public bool debugRay = true;
    public Vector3 goalDir;

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
```

The second method directly uses `MoveTowards`, which simply never overshoots and thus never produces jitter.

![A scene showing both methods to prevent jitter](Media/task6.gif)

On the left, the cube uses the magnitude script. On the right, the script uses the `MoveTowards` script.

## Task 7

> In this exercise we work on linear motion by making the character move in a straight line forward by introducing an improvement. The use of the `LookAt()` function makes the character turn instantly towards the objective, causing sudden changes. It is advised to perform a smooth transition over many frames. For this, instead of computing the necesary rotation angle, we make successive rotations where the angle in every frame is given by the intermediate values when interpolating between the original direction, and the final. We'll use the `Quaternion.Slerp()` method: `Quaternion.Slerp(Vector3 from, Vector3 to, float t)`. A quaternion is a mathematical instrument to ease the computation of rotations by avoiding the [Gimbal Lock](https://en.wikipedia.org/wiki/Gimbal_lock).

This task proposes the use of an interpolation function for rotations using `Quaternion.Slerp()` instead of the `LookAt()` function we used previously.

```c#
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
```

This script is based off the previous tasks' magnitude-accuracy variant. We replace the `LookAt()` method call with a snippet using `Quaternion.Slerp()` for the rotation interpolation. This time, the `accuracy` value needs to be a bit higher, because otherwise we may see the moving object circle around the objective instead of staying put.

I also added an extra `DebugRay` to show the moving object's `forward` direction as it tries to align itself with the goal's direction.

![A scene showing task 7's script behaviour](Media/task7.gif)

## Task 8

> In this section we'll work on a basic Waypoint system. Make a circuit in a scene with the collection of points that form it. Each point in the circuit has to be a 3D object with the "waypoint" tag assigned. Add a character object to reach the objectives. For this, a reference to each of the objectives must be taken, then apply displacements from an objective to the next by applying the previous work. The logic must include the management of getting the next objective.

This task is about writing a simple waypoint system. The scene is set up to include a few prisms with the `"Waypoint"` tag.

```c#
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
```

This script is simpler than it looks. It's based off of Task 7's. First, the class stores the list of waypoints an array of `GameObject`s. When `Start()` is called, the array is written by using `FindObjectsWithTag`, which will get all the waypoints we set in the editor with the `"Waypoint"` tag. An integer is used to mark current waypoint, and it is updated once we get close to it within a distance set by `accuracy`.

To make the system a bit more visual, I added materials for the currently selected waypoint, and the other inactive ones. The material handling logic is handled in `UpdateMaterial()`, and both materials are public so that we can set them in the editor.

![A scene showing the custom waypoint system in action](Media/task8.gif)

## Task 9

> In this section we'll work with the Unity Waypoints system. You must import the asset to the project in the `Utility/` folder. Configure the circuit, add the objective that must be followed by the character, then add the `WaypointProgressTracker` script to the character. Finally, add a script to the character that makes it follow the objective. The system will move the objective by keeping it away from the character following the points of the circuit. The character will try to follow the objective with our script, and so it will "force" the objective to go from point to point as it is followed.
> 1. **Create the circuit.** An empty object with the `WaypointCircuit` component.
> 2. **Create the waypoints as its children.** Empty objects inside the circuit placed wherever you want the character to go over. **The hierarchy's order is the circuit's order**. Match the heights so that the route does not go under or over the floor.
> 3. **Assign them.** In the circuit inspector, click *Assign using all child objects*. Every time you add, remove, or reorder a child, click it again.
> 4. **Choose a straight or curved path.** With *Smooth Route* active, the route is a curve that goes over all the points; when inactive, the path between points is straight. In the Scene view you'll see the route drawn in yellow (with Gizmos active). The route is **closed**: the last point loops back to the first.
> 5. **Create the auxiliary object.** An empty object called `Target`, **outside** the character (**not as a child**, or else it would move with it). To see it while you test it out yo can add a small sphere without a collider.
> 6. **Configure the tracker in the character**. Add `WaypointProgressTracker`, drag the circuit to the *Circuit* field, and `Target` to the *Target* field.
> 7. **Add the script that follows the target**, because the tracker only moves forward when the character does too.

The last task proposes using Unity's Waypoint System with our custom logic. I followed the instructions, and used Task 7's script for the moving object. The circuit is composed of 30 points distributed evenly alongside a circle of radius 8. The Waypoint System's scripts are stored in [Assets/Scripts/Utility](Assets/Scripts/Utility/).

![A scene showcasing the Waypoint System with the moving object](Media/task9.gif)