using UnityEngine;

public class WinDetection : MonoBehaviour
{
    public enum PlayerSide
    {
        Left,
        Right
    }
    // field
    [SerializeField] private PlayerSide winningSide;
    [SerializeField] private MapProgress mapProgress;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the object carrying RopeMover can win a round.
        if (other.GetComponent<RopeMover>() == null)
        {
            return;
        }

        if (mapProgress == null)
        {
            Debug.LogError(
                "Assign MapManager to this win zone.",
                this
            );

            return;
        }

        if (winningSide == PlayerSide.Left)
        {
            mapProgress.LeftWinsRound();
        }
        else
        {
            mapProgress.RightWinsRound();
        }
    }
}