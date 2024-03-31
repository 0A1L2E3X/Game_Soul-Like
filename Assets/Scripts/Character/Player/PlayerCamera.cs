using ALEX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("==== CAMERA OBJECT ====")]
    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    [Header("==== CAMERA SETTING ====")]
    [Space]
    private float cameraSmoothSpeed = 1;
    [SerializeField] float verticalRotateSpeed = 120;
    [SerializeField] float horizontalRotateSpeed = 120;
    [SerializeField] float minPivot = -30;
    [SerializeField] float maxPivot =  30;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("==== CAMERA VALUES ====")]
    [Space]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPos;
    [SerializeField] float horizontalLookAngle;
    [SerializeField] float verticalLookAngle;
    private float cameraPosZ;
    private float targetCameraPosZ;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        cameraPosZ = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        // 1.[FOLLOW PLAYER] 2.[ROTATE AOUND PLAYER] 3.[COLLSION WITH WALLS]

        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPos = Vector3.SmoothDamp(
            transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPos;
    }

    private void HandleRotations()
    {
        horizontalLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * horizontalRotateSpeed) * Time.deltaTime;
        verticalLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * verticalRotateSpeed) * Time.deltaTime;

        verticalLookAngle = Mathf.Clamp(verticalLookAngle, minPivot, maxPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        cameraRotation.y = horizontalLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = verticalLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraPosZ = cameraPosZ;
        RaycastHit hit;
        Vector3 dir = cameraObject.transform.position - cameraPivotTransform.position;
        dir.Normalize();

        if (Physics.SphereCast(
            cameraPivotTransform.position, 
            cameraCollisionRadius, dir, out hit, Mathf.Abs(targetCameraPosZ), collideWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraPosZ = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraPosZ) < cameraCollisionRadius)
        {
            targetCameraPosZ = -cameraCollisionRadius;
        }

        cameraObjectPos.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraPosZ, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPos;
    }
}
