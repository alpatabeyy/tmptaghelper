using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace TMPTagHelper.Scripts.Runtime
{
    public ref struct TextMeshProTagBuilder
    {
        private const char TAG_BEGIN = '<';
        private const char TAG_END = '>';
        private const char TAG_COMPLETED_SLASH = '/';
        private const char EQUALS = '=';
        
        private const string BOLD_TAG = "b";
        private const string ITALIC_TAG = "i";
        private const string STRIKETHROUGH_TAG = "s";
        private const string UNDERLINE_TAG = "u";
        private const string ALIGN_TAG = "align";
        private const string UPPERCASE = "uppercase";
        private const string LOWERCASE = "lowercase";
        private const string COLOR_TAG = "color";
        private const string LINK_TAG = "link";
        
        private readonly StringBuilder _stringBuilder;

        private string _color;
        
        private bool _isDisposed;

        public TextMeshProTagBuilder(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
            _isDisposed = false;
            _color = null;
        }
        
        public TextMeshProTagBuilder Align(AlignType alignType)
        {
            BuildTagWithValue(ALIGN_TAG, alignType.ToString().ToLowerInvariant());

            return this;
        }
        
        public TextMeshProTagBuilder ToBold()
        {
            BuildDefaultTag(BOLD_TAG);

            return this;
        }
        
        public TextMeshProTagBuilder ToItalic()
        {
            BuildDefaultTag(ITALIC_TAG);

            return this;
        }
        
        public TextMeshProTagBuilder ToStrikethrough()
        {
            BuildDefaultTag(STRIKETHROUGH_TAG);

            return this;
        }
        
        public TextMeshProTagBuilder ToUnderline()
        {
            BuildDefaultTag(UNDERLINE_TAG);

            return this;
        }
        
        public TextMeshProTagBuilder ToUpperCase()
        {
            BuildDefaultTag(UPPERCASE);

            return this;
        }
        
        public TextMeshProTagBuilder ToLowerCase()
        {
            BuildDefaultTag(LOWERCASE);

            return this;
        }
        
        public TextMeshProTagBuilder SetColor(string colorCode)
        {
            _color = colorCode;
            
            return this;
        }

        public TextMeshProTagBuilder SetColor(Color color)
        {
            SetColor(ColorUtility.ToHtmlStringRGBA(color));
            
            return this;
        }

        public TextMeshProTagBuilder SetLink(string id, TMP_Text textBox, Camera canvasCamera, Action callback)
        {
            BuildTagWithValue(LINK_TAG, id);

            if (!textBox.gameObject.TryGetComponent(out TextMeshProLinkTrigger textMeshProLinkTrigger))
            {
                textMeshProLinkTrigger = textBox.gameObject.AddComponent<TextMeshProLinkTrigger>();
            }
            
            textMeshProLinkTrigger.Initialize(textBox, canvasCamera);
            textMeshProLinkTrigger.AddId(id, callback);

            return this;
        }

        public TextMeshProTagBuilder SetLink(string id, TMP_Text textBox, Action callback)
        {
            SetLink(id, textBox, null, callback);

            return this;
        }

        private void BuildDefaultTag(string tag)
        {
            _stringBuilder
                .Insert(0, TAG_BEGIN)
                .Insert(1, tag)
                .Insert(1 + tag.Length, TAG_END)
                .Append(TAG_BEGIN)
                .Append(TAG_COMPLETED_SLASH)
                .Append(tag)
                .Append(TAG_END);
        }

        private void BuildTagWithValue(string tag, string value)
        {
            int tagLength = tag.Length;
            int valueLength = value.Length;
            _stringBuilder
                .Insert(0, TAG_BEGIN)
                .Insert(1, tag)
                .Insert(1 + tagLength, EQUALS)
                .Insert(2 + tagLength, value)
                .Insert(2 + valueLength + tagLength, TAG_END)
                .Append(TAG_BEGIN)
                .Append(TAG_COMPLETED_SLASH)
                .Append(tag)
                .Append(TAG_END);
        }

        public static implicit operator string(TextMeshProTagBuilder textMeshProTagBuilder)
        {
            return textMeshProTagBuilder.ToString();
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_color))
            {
                BuildTagWithValue(COLOR_TAG, $"#{_color}");
            }
            
            string stringValue = _stringBuilder.ToString();
            _stringBuilder.Clear();
            _isDisposed = true;
            return stringValue;
        }
    }
}