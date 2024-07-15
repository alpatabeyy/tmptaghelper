using TMPro;
using TMPTagHelper.Scripts.Runtime;
using UnityEngine;

namespace TMPTagHelper.Demo.Scripts
{
    public class TMPTagHelperDemoText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tmpText;

        private void Start()
        {
            _tmpText.text = "This text " 
                            + "bolded".CreateTagBuilder().ToBold()
                            + " with TMP Tag Helper. "
                            + "Click here".CreateTagBuilder().ToItalic().ToUnderline()
                                .SetLink("2", _tmpText, OnLinkClicked).SetColor(Color.blue).ToUpperCase() 
                            + " to get more information";
        }

        private void OnLinkClicked()
        {
            Application.OpenURL("https://u3d.as/3ke6");
        }
    }
}