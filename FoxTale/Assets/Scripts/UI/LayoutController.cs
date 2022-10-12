using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dialogueTextPrefab;

    private List<TextMeshProUGUI> items = new List<TextMeshProUGUI>();


    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            items.Add(Instantiate(dialogueTextPrefab, gameObject.transform));
        }
    }

    public TextMeshProUGUI AddListItem()
    {
        TextMeshProUGUI newItem = Instantiate(dialogueTextPrefab, transform).GetComponent<TextMeshProUGUI>();
        items.Add(newItem);

        return newItem;
    }

    public TextMeshProUGUI GetItem(int index)
    {
        if (index >= items.Count)
            return null;

        return items[index];
    }

    public TextMeshProUGUI GetFirstItem()
    {
        if (items.Count == 0)
            AddListItem();

        return items[0];
    }

    public TextMeshProUGUI GetLastItem()
    {
        if (items.Count == 0)
            AddListItem();

        return items[items.Count - 1];
    }

    public void ClearItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].text = "";
        }
    }
}
