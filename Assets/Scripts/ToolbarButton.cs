using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarButton : MonoBehaviour, IPointerClickHandler
{
    private const int CREATE_OBJECT=1, EDIT_OBJECT=2;
    public int type;
    public GameObject objectToCreate;
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (type)
        {
            case CREATE_OBJECT:
                {
                    Instantiate(objectToCreate);
                    break;
                }
            case EDIT_OBJECT:
                {
                    StartCoroutine(waitForClick());
                    break;
                }
        }
    }

    IEnumerator waitForClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform.gameObject.tag != "Building/Base")
                    {
                        ObjectCreation oc = hit.transform.gameObject.AddComponent(typeof(ObjectCreation)) as ObjectCreation;
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }
}
