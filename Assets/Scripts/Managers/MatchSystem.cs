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

        // Se�ili ��eleri filtrele
        var selectedElements = elements.Where(e => e.isSelected).ToList();

        // Checklenmi� ��eleri tespit etmek i�in bir HashSet olu�tur
        HashSet<Element> visited = new HashSet<Element>();

        foreach (var element in selectedElements)
        {
            // Kontrol edildiyse ge�
            if (visited.Contains(element)) continue;

            // DFS ile ba�lant�l� ��eleri bul
            var connectedElements = new List<Element>();
            DFS(element, selectedElements, visited, connectedElements);

            // E�er en az 3 ��e ba�lant�l�ysa kontrol�
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
        // Skoru art�r
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
            // E�er kom�u ��e selectedElements i�inde yer al�yorsa ve hen�z checklenmemi�se, DFS fonksiyonu tekrar �a�r�l�r ve bu ��e �zerinde i�lem yap�l�r.
            if (selectedElements.Contains(neighbor) && !visited.Contains(neighbor))
            {
                DFS(neighbor, selectedElements, visited, connectedElements);
            }
        }
    }

    // T�m kom�ular� al (up, down, left, right)
    private List<Element> GetNeighbors(Element element)
    {
        var neighbors = new List<Element>();

        var data = element.GetData();
        int row = data.row;
        int col = data.col;

        // Ge�erli ��enin kom�ular�
        if (row > 0) neighbors.Add(GetElementAt(row - 1, col)); // Up
        if (row < gridSize - 1) neighbors.Add(GetElementAt(row + 1, col)); // Down
        if (col > 0) neighbors.Add(GetElementAt(row, col - 1)); // Left
        if (col < gridSize - 1) neighbors.Add(GetElementAt(row, col + 1)); // Right

        return neighbors.Where(e => e != null).ToList();
    }

    private Element GetElementAt(int row, int col)
    {
        //belirtilen sat�r ve s�tunda bulunan ��eyi d�nd�r
        return elements.FirstOrDefault(e => e.GetData().row == row && e.GetData().col == col);
    }
}
