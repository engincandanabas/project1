using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private GameObject framePrefab;
    [SerializeField] private SpriteRenderer referenceImage;
    [SerializeField] private int gridSize = 4;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InstantiateGridFrames();
    }

    void InstantiateGridFrames()
    {
        if (referenceImage == null || framePrefab == null || gridSize <= 0 || gridSize <= 0)
        {
            Debug.LogError("Missing reference image, frame prefab, or invalid grid values!");
            return;
        }

        float totalWidth = referenceImage.bounds.size.x;
        float totalHeight = referenceImage.bounds.size.y;

        float frameWidth = totalWidth / gridSize;
        float frameHeight = totalHeight / gridSize;

        Vector3 startPosition = new Vector3(referenceImage.bounds.min.x, referenceImage.bounds.max.y, 0);

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject frame = Instantiate(framePrefab, transform);

                float posX = startPosition.x + frameWidth * (col + 0.5f);
                float posY = startPosition.y - frameHeight * (row + 0.5f);

                frame.transform.position = new Vector3(posX, posY, 0);

                frame.transform.localScale = new Vector3(
                    frameWidth / framePrefab.GetComponent<SpriteRenderer>().bounds.size.x,
                    frameHeight / framePrefab.GetComponent<SpriteRenderer>().bounds.size.y,
                    1);
            }
        }
    }

    public void ReloadGridSize(int gridSize)
    {

    }
}
