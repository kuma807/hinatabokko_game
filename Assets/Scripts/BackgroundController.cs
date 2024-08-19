using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject stage1Prefab;
    [SerializeField] private GameObject stage2Prefab;
    [SerializeField] private GameObject stage3Prefab;

    private int _currentStage;

    private GameObject backgroundPrefab;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    public float backgroundWidth;
    public float backgroundHeight;

    void Start()
    {
        this._currentStage = 2;
        ChangeStageBackGround(this._currentStage);
    }

    public void ChangeStageBackGround(int stage)
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        switch (stage)
        {
            case 1:
                backgroundPrefab = stage1Prefab;
                Debug.Log("IMa Stage1");
                break;
            case 2:
                backgroundPrefab = stage2Prefab;
                Debug.Log("IMa Stage2");
                break;
            case 3:
                backgroundPrefab = stage3Prefab;
                Debug.Log("IMa Stage3");
                break;
            default:
                Debug.LogWarning("Invalid stage selected. Defaulting to stage 1.");
                backgroundPrefab = stage1Prefab;
                break;
        }
        Instantiate(backgroundPrefab, transform.position, Quaternion.identity, transform);
    }

    void Update()
    {
    }
}
