using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private GameObject framePrefab;
    [SerializeField] private SpriteRenderer referenceImage;
    [SerializeField] private int gridSize = 4;

    public List<Element> elements = new List<Element>();

    public List<Element> Elements { get { return elements; } }
    public int GridSize { get { return gridSize; } }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InstantiateGridFrames();
    }

    private void InstantiateGridFrames()
    {
        if (referenceImage == null || framePrefab == null || gridSize <= 0 || gridSize <= 1)
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
                GameObject frame = Instantiate(framePrefab, referenceImage.transform);

                float posX = startPosition.x + frameWidth * (col + 0.5f);
                float posY = startPosition.y - frameHeight * (row + 0.5f);

                frame.transform.position = new Vector3(posX, posY, 0);

                frame.transform.localScale = new Vector3(
                    (frameWidth / framePrefab.GetComponent<SpriteRenderer>().bounds.size.x) / referenceImage.transform.localScale.x,
                    (frameHeight / framePrefab.GetComponent<SpriteRenderer>().bounds.size.y) / referenceImage.transform.localScale.y,
                    1);

                var element_sc = frame.GetComponent<Element>();
                element_sc.SetData(row, col);
                elements.Add(element_sc);
            }
        }
    }

    public void ReloadGridSize(int _gridSize)
    {
        if (_gridSize <= 1) return;

        DestroyAllElements();
        gridSize = _gridSize;
        InstantiateGridFrames();
    }

    private void DestroyAllElements()
    {
        for (int i = 0; i < referenceImage.transform.childCount; i++)
        {
            Destroy(referenceImage.transform.GetChild(i).gameObject);
        }
        elements.Clear();
    }

    
}
