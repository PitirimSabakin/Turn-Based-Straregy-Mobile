using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PressAndHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject panelDescription;

    Coroutine coroutine;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        coroutine = StartCoroutine(startHoldTimer());
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        StopCoroutine(coroutine);
        panelDescription.SetActive(false);
    }

    private IEnumerator startHoldTimer()
    {
        yield return new WaitForSeconds(0.5f);
        panelDescription.SetActive(true);

        for(int i = 0; i < Global.persons[0].arrSkill.Length; i++)
        {
            if(transform.name == $"Skill{i}" && Global.persons[i] != null)
            {
                SkillScript.Skill chooseSkill = Global.persons[0].arrSkill[i];
                FillPanel(chooseSkill);
                break;
            }
        }       
    }

    private void FillPanel(SkillScript.Skill chooseSkill)
    {
        panelDescription.transform.GetChild(0).GetComponent<Image>().sprite = chooseSkill.SpriteOfSpell;

        panelDescription.transform.GetChild(1)
            .GetComponent<TMP_Text>().text = chooseSkill.ToString();
    }
}
