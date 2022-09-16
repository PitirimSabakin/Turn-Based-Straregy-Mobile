using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOfSpells : MonoBehaviour
{

    public Sprite sprite;
    public GameObject cellsParentSpells;

    public int x = 5;
    public int y = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    //Fill the panel with magick spells, which player know
    public void FillCellsWithSpells()
    {
        //Delete all spells on the panel
        for (int i = 0; i < cellsParentSpells.transform.childCount; i++)
        {
            for(int j = 0; j < cellsParentSpells.transform.GetChild(i).childCount; j++)
            {
                Destroy(cellsParentSpells.transform.GetChild(i).GetChild(j).gameObject);
            }
        }

        //Create object of Image 
        for (int i = 0; i < cellsParentSpells.transform.childCount; i++)
        {
            
            if (i == Global.persons[0].listMagickSpells.Count) break;
           
            GameObject obj = new GameObject();
            Image image = obj.AddComponent<Image>();
            image.sprite = Global.persons[0].listMagickSpells[i].SpriteOfSpell;
            obj.GetComponent<RectTransform>().SetParent(cellsParentSpells.transform.GetChild(i).transform);
            obj.GetComponent<RectTransform>().position = cellsParentSpells.transform.GetChild(i).transform.position;
            obj.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 1);
            obj.SetActive(true);
            obj.name = Global.persons[0].listMagickSpells[i].Name;
        }
    }

    //Spawn cells on the panel
    public void SpawnCells()
    {
        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                float xPosition = cellsParentSpells.transform.position.x + 45f * i;
                float yPosition = cellsParentSpells.transform.position.y - 45f * j;
                Vector3 position = Vector3.zero;
                position.x = xPosition;
                position.y = yPosition;
                GameObject obj = new GameObject();
                Image image = obj.AddComponent<Image>();
                image.sprite = sprite;
                obj.GetComponent<RectTransform>().SetParent(cellsParentSpells.transform);
                obj.GetComponent<RectTransform>().position = position;
                obj.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 1);
                obj.GetComponent<Image>().color = Color.gray;
                obj.SetActive(true);
                obj.name = $"{j} {i}";
            }
        }
    }

}
