using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler
{
    Vector3 startPosition;
    Vector3 diffPosition;
    GameObject canvas_;
    GameObject panelGhost;
    public static float separator;

    Color color, newColor;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition - diffPosition;

        Image image = GetComponent<Image>();

        if(GetComponent<RectTransform>().localPosition.x<=separator)
        {
            image.color = newColor;
            GetComponent<UIDrop>().enabled=false;
        }
        
        else 
        {
            image.color = color;
            GetComponent<UIDrop>().enabled=true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(GetComponent<RectTransform>().localPosition.x <= separator)
            Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = transform.position;
        diffPosition = Input.mousePosition - startPosition;
        EventSystem.current.SetSelectedGameObject(gameObject);
        EventSystem.current.currentSelectedGameObject.transform.SetParent(canvas_.transform);
        EventSystem.current.currentSelectedGameObject.transform.SetAsFirstSibling();
       
       if(GetComponent<RectTransform>().localPosition.x < separator)
        {
            var i = GameObject.Instantiate(gameObject);
            i.transform.SetParent(TabSelect.current.transform);
            Vector2 scale = gameObject.transform.localScale;
            i.transform.localScale = scale;
            i.transform.position = startPosition;
        }

    }

    void Start()
    {
        canvas_ = GameObject.Find("Robot programming");
        panelGhost = GameObject.Find("Panel ghost");
        color = GetComponent<Image>().color;
        newColor = new Color(255, 0, 0, 1);
    }

}
