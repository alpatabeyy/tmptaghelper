using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPTagHelper.Scripts.Runtime
{
    public class TextMeshProLinkTrigger : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _textBox;
        
        private Camera _canvasCamera;

        private readonly Dictionary<string, Action> _idToCallbackMap = new Dictionary<string, Action>();

        public void Initialize(TMP_Text textBox, Camera canvasCamera)
        {
            _textBox = textBox;
            _canvasCamera = canvasCamera;
        }

        public void AddId(string id, Action callback)
        {
            _idToCallbackMap.TryAdd(id, callback);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);

            int linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_textBox, mousePosition, _canvasCamera);
            if (linkTaggedText != -1)
            {
                TMP_LinkInfo linkInfo = _textBox.textInfo.linkInfo[linkTaggedText];

                if (_idToCallbackMap.TryGetValue(linkInfo.GetLinkID(), out Action callback))
                {
                    callback?.Invoke();
                }
            }
        }
    }
}