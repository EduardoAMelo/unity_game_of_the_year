using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 _velocity;

    private void LateUpdate()
    {
        if (!target) return;

        // Mantém Z fixo para evitar “pulos” por perspectiva
        Vector3 targetPos = target.position + offset;
        targetPos.z = offset.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref _velocity,
            smoothTime
        );
    }
}
