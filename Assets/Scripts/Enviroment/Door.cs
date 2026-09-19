using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AnimationCurve openCurve;
    public float openDuration = 1f;
    public bool isOpen;

    private Quaternion closedRotation;

    private void Awake()
    {
        closedRotation = transform.rotation;
    }

    public void AttemptUnlock(Transform player)
    {
        if (isOpen)
            return;

        //direction from door to player
        Vector3 directionToPlayer =
            (player.position - transform.position).normalized;

        //figure out which side the player is on
        float dot = Vector3.Dot(transform.forward, directionToPlayer);

        //calculate target before the door can move
        Quaternion targetRotation = closedRotation * Quaternion.Euler(0f, dot > 0f ? 90f : -90f, 0f);

        StartCoroutine(OpenDoor(targetRotation));
    }

    private IEnumerator OpenDoor(Quaternion targetRotation)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;

            transform.rotation = Quaternion.Slerp(closedRotation, targetRotation, openCurve.Evaluate(t));

            yield return null;
        }

        //ensure targetrotation is met
        transform.rotation = targetRotation;

        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen)
            return;

        StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        Quaternion startRotation = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;

            transform.rotation = Quaternion.Slerp(startRotation, closedRotation, openCurve.Evaluate(t));

            yield return null;
        }

        transform.rotation = closedRotation;
        isOpen = false;
    }
}