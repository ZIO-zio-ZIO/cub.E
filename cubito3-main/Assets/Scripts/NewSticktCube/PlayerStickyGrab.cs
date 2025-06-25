using UnityEngine;

public class PlayerStickyGrab : MonoBehaviour
{
    [SerializeField] private Transform handPosition; 
    private StickyCube stickyCubeHeld;

    void Update()
    {
        if (stickyCubeHeld == null)
        {
            TryGrabStickyCube();
        }
    }

    void TryGrabStickyCube()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (var col in colliders)
            {
                StickyCube sticky = col.GetComponent<StickyCube>();
                if (sticky != null && !sticky.IsHeld)
                {
                    GrabStickyCube(sticky);
                    break;
                }
            }
        }
    }

    void GrabStickyCube(StickyCube cube)
    {
        PlayerAudioManager.Instance?.PlayStickyGrabSound();

        stickyCubeHeld = cube;
        cube.AttachToHand(handPosition);
    }

    public void DropStickyCube()
    {
        if (stickyCubeHeld != null)
        {
            stickyCubeHeld.Detach();
            stickyCubeHeld = null;
        }
    }

    public bool IsHoldingStickyCube()
    {
        return stickyCubeHeld != null;
    }

    public bool IsHoldingThisCube(StickyCube cube)
    {
        return stickyCubeHeld == cube;
    }

    public StickyCube GetHeldCube()
    {
        return stickyCubeHeld;
    }

}

