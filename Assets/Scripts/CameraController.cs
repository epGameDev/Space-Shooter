using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    public void ShakeCamera()
    {
        _anim.SetTrigger("PlayerHit");
    }
}