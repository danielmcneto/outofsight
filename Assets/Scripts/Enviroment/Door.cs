using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AnimationCurve openCurve;
    public float openDuration = 1f;
    public bool isOpen;

    private Quaternion closedRotation;
    private Coroutine doorCoroutine;

    private void Awake()
    {
        closedRotation = transform.rotation;
    }

    public void AttemptUnlock(Transform player)
    {
        // Do not start another opening animation if the door is already opening/open.
        if (isOpen)
            return;

        //direction from door to player
        Vector3 directionToPlayer =
            (player.position - transform.position).normalized;

        //figure out which side the player is on
        float dot = Vector3.Dot(transform.forward, directionToPlayer);

        //calculate target before the door can move
        Quaternion targetRotation = closedRotation * Quaternion.Euler(0f, dot > 0f ? 90f : -90f, 0f);

        // Cancel a closing animation if the player re-enters the trigger.
        StopDoorAnimation();
        isOpen = true;
        doorCoroutine = StartCoroutine(OpenDoor(targetRotation));
    }

    private IEnumerator OpenDoor(Quaternion targetRotation)
    {
        // Start from the current rotation so the door can open smoothly from any point.
        Quaternion startRotation = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;

            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                openCurve.Evaluate(Mathf.Clamp01(t)));

            yield return null;
        }

        // Ensure the final rotation is exact after the animation completes.
        transform.rotation = targetRotation;
        doorCoroutine = null;
    }

    public void Close()
    {
        // The door may still be opening, so cancel that animation first.
        if (!isOpen)
            return;

        StopDoorAnimation();
        isOpen = false;
        doorCoroutine = StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        // Start from the current rotation so leaving early reverses the door smoothly.
        Quaternion startRotation = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;

            transform.rotation = Quaternion.Slerp(
                startRotation,
                closedRotation,
                openCurve.Evaluate(Mathf.Clamp01(t)));

            yield return null;
        }

        transform.rotation = closedRotation;
        doorCoroutine = null;
    }

    private void StopDoorAnimation()
    {
        if (doorCoroutine == null)
            return;

        // StopCoroutine prevents opening and closing from fighting over the rotation.
        StopCoroutine(doorCoroutine);
        doorCoroutine = null;
    }
}