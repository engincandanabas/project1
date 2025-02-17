using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI matchCountText;
    [SerializeField] private TMP_InputField gridSizeInputField;
    [SerializeField] private Button reloadButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        reloadButton.onClick.AddListener(() => ReloadGridSize());
    }

    public void UpdateMatchCount(int count)
    {
        matchCountText.text ="Match Count "+count.ToString();
    }

    private void ReloadGridSize()
    {
        if(gridSizeInputField!=null && gridSizeInputField.text != null)
        {
            GridManager.Instance.ReloadGridSize(int.Parse(gridSizeInputField.text));
        }
    }


}
