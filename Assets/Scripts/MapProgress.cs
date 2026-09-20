using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MapProgress : MonoBehaviour
{
    //fields
    [SerializeField] private int borderPosition = 5;
    [SerializeField] private Image mapImage;
    [SerializeField] private TMP_Text borderLabel;
    [SerializeField] private Sprite[] mapSprites = new Sprite[11];
    [SerializeField] private GameObject mapOverlay;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private RopeMover ropeMover;

    [SerializeField, Min(0.1f)]
    private float mapDisplaySeconds = 3f;
    private Vector3 ropeStartPosition;
    private bool roundInProgress = false;
    private bool setupComplete = false;
    private Coroutine nextRoundRoutine;

    private void Start()
    {
        // Do not allow movement until setup is checked.
        if (ropeMover != null)
        {
            ropeMover.enabled = false;
        }

        if (mapOverlay == null ||
            restartButton == null ||
            ropeMover == null ||
            mapImage == null ||
            borderLabel == null)
        {
            Debug.LogError(
                "Assign all scene references on MapManager.",
                this
            );

            return;
        }

        if (mapSprites == null || mapSprites.Length != 11)
        {
            Debug.LogError(
                "Map Sprites must contain exactly 11 slots.",
                this
            );

            return;
        }

        ropeStartPosition = ropeMover.transform.position;
        setupComplete = true;

        RestartMatch();
    }

    public void LeftWinsRound()
    {
        ChangeBorder(1);
    }

    public void RightWinsRound()
    {
        ChangeBorder(-1);
    }

    private void ChangeBorder(int movement)
    {
        // Ignore extra results during the map screen or after victory.
        if (!roundInProgress)
        {
            return;
        }

        // Close the round before doing anything else.
        roundInProgress = false;
        ropeMover.enabled = false;

        borderPosition = Mathf.Clamp(
            borderPosition + movement,
            0,
            10
        );

        UpdateMapDisplay();

        string winner = movement > 0
            ? "Left player"
            : "Right player";

        bool matchOver =
            borderPosition == 0 || borderPosition == 10;

        string result = matchOver
            ? winner + " conquered the map!"
            : winner + " wins this round!";

        borderLabel.text = result + "\n" + borderLabel.text;

        Debug.Log(
            result + " Border position: " + borderPosition
        );

        restartButton.SetActive(matchOver);
        mapOverlay.SetActive(true);

        if (!matchOver)
        {
            nextRoundRoutine =
                StartCoroutine(ShowMapThenNextRound());
        }
    }
    private void UpdateMapDisplay()
    {
        // The scene objects must be connected in the Inspector.
        if (mapImage == null || borderLabel == null)
        {
            Debug.LogError("Assign Map Image and Border Label on MapManager.");
            return;
        }

        // Check the array before accessing one of its slots.
        if (mapSprites == null || mapSprites.Length != 11 ||
            borderPosition < 0 || borderPosition > 10)
        {
            Debug.LogError("Use 11 map slots and a border position from 0 to 10.");
            return;
        }

        Sprite selectedMap = mapSprites[borderPosition];

        mapImage.sprite = selectedMap;
        mapImage.color = Color.white;
        mapImage.preserveAspect = true;

        borderLabel.text = "Border position: " + borderPosition + " / 10";

        if (selectedMap == null)
        {
            borderLabel.text += "\n Placeholder: map image not assigned";
        }
    }
    private IEnumerator ShowMapThenNextRound()
    {
        yield return new WaitForSecondsRealtime(
            mapDisplaySeconds
        );

        nextRoundRoutine = null;

        BeginNextRound();
    }

    private void BeginNextRound()
    {
        ropeMover.transform.position = ropeStartPosition;

        restartButton.SetActive(false);
        mapOverlay.SetActive(false);

        roundInProgress = true;
        ropeMover.enabled = true;
    }

    public void RestartMatch()
    {
        if (!setupComplete)
        {
            return;
        }

        roundInProgress = false;
        ropeMover.enabled = false;

        // Prevent an old transition from starting a round later.
        if (nextRoundRoutine != null)
        {
            StopCoroutine(nextRoundRoutine);
            nextRoundRoutine = null;
        }

        borderPosition = 5;

        UpdateMapDisplay();
        BeginNextRound();
    }
}
