using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof (TextMeshProUGUI))]
public class LinkOpener : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick (PointerEventData eventData)
    {
        TextMeshProUGUI pTextMeshPro = GetComponent<TextMeshProUGUI> ();
        int linkIndex =
            TMP_TextUtilities.FindIntersectingLink (pTextMeshPro , eventData.position ,
                null); // If you are not in a Canvas using Screen Overlay, put your camera instead of null
        if ( linkIndex != -1 )
        {
            // was a link clicked?
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo [linkIndex];
            Application.OpenURL (linkInfo.GetLinkID ());
        }
    }
}