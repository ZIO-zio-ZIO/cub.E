using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("Huecos necesarios")]
    [SerializeField] private List<MonoBehaviour> cubeHoles = new List<MonoBehaviour>();
    [SerializeField] private int requiredOccupiedCount = 1;

    [Header("Puerta")]
    [SerializeField] private Transform door;
    [SerializeField] private Vector3 openOffset;
    [SerializeField] private float openSpeed = 2f;

    [Header("Paneo")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private MovementPlayer playerMovementScript;
    [SerializeField] private MonoBehaviour cameraControlScript;
    [SerializeField] private float paneoDuration = 1.5f;
    [SerializeField] private float waitAtDoorTime = 1.5f;

    [Header("Paneo Ajustes")]
    [SerializeField] private float paneoDistance = 5f;
    [SerializeField] private float paneoHeight = 2f;
    [SerializeField, Range(0f, 360f)] private float paneoAngle = 0f;
    [SerializeField] private Vector3 lookAtOffset = Vector3.zero;

    [SerializeField] private List<Light> luces = new List<Light>();

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool opening = false;
    private bool hasPanned = false;

    void Start()
    {
        closedPos = door.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        if (!opening && CountOccupiedHoles() >= requiredOccupiedCount)
        {
            opening = true;
            if (!hasPanned)
            {
                StartCoroutine(LookAtDoorWhileOpening());
                hasPanned = true;
            }
        }

        if (opening)
        {
            door.position = Vector3.MoveTowards(door.position, openPos, openSpeed * Time.deltaTime);
            ApagarLuces();
        }
    }

    private int CountOccupiedHoles()
    {
        int count = 0;
        foreach (MonoBehaviour mb in cubeHoles)
        {
            if (mb == null) continue;
            var method = mb.GetType().GetMethod("IsSlotOccupied");
            if (method != null)
            {
                var result = method.Invoke(mb, null);
                if (result is bool && (bool)result)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void ApagarLuces()
    {
        foreach (var luz in luces)
        {
            if (luz != null)
            {
                luz.enabled = false;
            }
        }
    }

    private IEnumerator LookAtDoorWhileOpening()
    {
        if (playerMovementScript != null)
            playerMovementScript.SetMovementLocked(true);

        if (cameraControlScript != null)
            cameraControlScript.enabled = false;

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        float radians = paneoAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * paneoDistance;
        Vector3 targetPos = door.position + offset + Vector3.up * paneoHeight;
        Vector3 lookAtPoint = door.position + lookAtOffset;
        Quaternion targetRot = Quaternion.LookRotation(lookAtPoint - targetPos);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / paneoDuration;
            float easedT = Mathf.SmoothStep(0, 1, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, easedT);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, easedT);
            yield return null;
        }

        yield return new WaitForSeconds(waitAtDoorTime);

        while (door.position != openPos)
        {
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / paneoDuration;
            float easedT = Mathf.SmoothStep(0, 1, t);
            mainCamera.transform.position = Vector3.Lerp(targetPos, startPos, easedT);
            mainCamera.transform.rotation = Quaternion.Slerp(targetRot, startRot, easedT);
            yield return null;
        }

        if (cameraControlScript != null)
            cameraControlScript.enabled = true;

        if (playerMovementScript != null)
            playerMovementScript.SetMovementLocked(false);
    }

    private void OnDrawGizmos()
    {
        if (door == null) return;

        float radians = paneoAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * paneoDistance;
        Vector3 targetPos = door.position + offset + Vector3.up * paneoHeight;
        Vector3 lookAtPoint = door.position + lookAtOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(targetPos, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(targetPos, lookAtPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lookAtPoint, 0.3f);
    }
}

