using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    private ElementData data;

    [SerializeField] private GameObject selectedSprite;
    public bool isSelected=false;

    private void OnMouseDown()
    {
        //Debug.Log(data.row + " " + data.col);
        if (!isSelected)
        {
            isSelected = true;
            selectedSprite.gameObject.SetActive(true);
            MatchSystem.Instance.CheckMatch();
        }

    }
    public void ResetData()
    {
        isSelected=false;
        selectedSprite.gameObject.SetActive(false);
    }

    public void SetData(int row,int col)
    {
        if (row < 0 || col < 0) return;
        data = new ElementData(row,col);
    }
    public ElementData GetData()
    {
        return data;
    }
}

[System.Serializable]   
public class ElementData
{
    public int row;
    public int col;

    public ElementData(int row, int col)
    {
        this.row = row;
        this.col = col;
    }   
}

