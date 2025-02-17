using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class MatchSystem : MonoBehaviour
{
    public static MatchSystem Instance { get; private set; }    


    private List<Element> elements;
    private int gridSize;

    [Range(0.01f, 1f)]
    public float clearDelay = 0.3f;


    private void Awake()
    {
        Instance = this;
    }

    public void CheckMatch()
    {
        elements = GridManager.Instance.Elements;
        gridSize=GridManager.Instance.GridSize;

        // Seçili öðeleri filtrele
        var selectedElements = elements.Where(e => e.isSelected).ToList();

        // Checklenmiþ öðeleri tespit etmek için bir HashSet oluþtur
        HashSet<Element> visited = new HashSet<Element>();

        foreach (var element in selectedElements)
        {
            // Kontrol edildiyse geç
            if (visited.Contains(element)) continue;

            // DFS ile baðlantýlý öðeleri bul
            var connectedElements = new List<Element>();
            DFS(element, selectedElements, visited, connectedElements);

            // Eðer en az 3 öðe baðlantýlýysa kontrolü
            if (connectedElements.Count >= 3)
            {
                StartCoroutine(ClearMatches(connectedElements));
            }
        }
    }
    IEnumerator ClearMatches(List<Element> elements)
    {
        yield return new WaitForSeconds(clearDelay);
        foreach (var connectedElement in elements)
        {
            connectedElement.ResetData();
        }
        // Skoru artýr
        GameManager.Instance.Score++;

    }
    private void DFS(Element current, List<Element> selectedElements, HashSet<Element> visited, List<Element> connectedElements)
    {
        if (visited.Contains(current)) return;

        visited.Add(current);
        connectedElements.Add(current);

        var neighbors = GetNeighbors(current);

        foreach (var neighbor in neighbors)
        {
            // Eðer komþu öðe selectedElements içinde yer alýyorsa ve henüz checklenmemiþse, DFS fonksiyonu tekrar çaðrýlýr ve bu öðe üzerinde iþlem yapýlýr.
            if (selectedElements.Contains(neighbor) && !visited.Contains(neighbor))
            {
                DFS(neighbor, selectedElements, visited, connectedElements);
            }
        }
    }

    // Tüm komþularý al (up, down, left, right)
    private List<Element> GetNeighbors(Element element)
    {
        var neighbors = new List<Element>();

        var data = element.GetData();
        int row = data.row;
        int col = data.col;

        // Geçerli öðenin komþularý
        if (row > 0) neighbors.Add(GetElementAt(row - 1, col)); // Up
        if (row < gridSize - 1) neighbors.Add(GetElementAt(row + 1, col)); // Down
        if (col > 0) neighbors.Add(GetElementAt(row, col - 1)); // Left
        if (col < gridSize - 1) neighbors.Add(GetElementAt(row, col + 1)); // Right

        return neighbors.Where(e => e != null).ToList();
    }

    private Element GetElementAt(int row, int col)
    {
        //belirtilen satýr ve sütunda bulunan öðeyi döndür
        return elements.FirstOrDefault(e => e.GetData().row == row && e.GetData().col == col);
    }
}
