using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z.LayoutPanel;
// using Z.LayoutPanel;
namespace zUI

{
    //v02

    public static class UIBuilder
    {

        public static int topHeight = 16;
        public static int sliderHeight { get { return topHeight; } }
        public static int buttonHeight { get { return topHeight; } }
        public static int toggleDotHeight { get { return topHeight - 4; } }
        public static int toggleInnerDotHeight { get { return toggleDotHeight - 4; } }
        public static int handleWidth { get { return topHeight / 5; } }
        static int counter { get { return ++_counter; } }
        static int _counter;
        static int padSize = 1;
        static int spacing = 1;
        static int leftPad = 7;
        public static int fontSize = 10;
        public static int buttonFontSize { get { return fontSize; } }
        public static Color RandomBGColor()
        {
            Color c = new Color(Random.Range(0.05f, 0.3f), Random.Range(0.1f, 0.4f), Random.Range(0.1f, .4f));;
            c.a = Random.Range(0.1f, 0.2f);
            return c;
            //return (new Color(Random.value * .2f, Random.value * .3f, Random.value * .3f)).ShiftSat(0.3f);
        }
        public static LayoutElement AddLayoutElement(this RectTransform rectTransform)
        {
            return rectTransform.AddLayoutElement(defaultWidth, topHeight);

        }

        /// <summary>
        /// obviously temporary name
        /// </summary>
        public static LayoutElement AddLayoutElementFromExistingOrIgnoreIfExists(this RectTransform rectTransform)
        {

            if (rectTransform.GetComponent<LayoutElement>() != null) return rectTransform.GetComponent<LayoutElement>() ;
            return rectTransform.AddLayoutElement((int) rectTransform.rect.width, (int) rectTransform.rect.height);

        }
        public static LayoutElement AddLayoutElementFromExisting(this RectTransform rectTransform)
        {
            return rectTransform.AddLayoutElement((int) rectTransform.rect.width, (int) rectTransform.rect.height);

        }
        public static LayoutElement AddLayoutElement(this RectTransform rectTransform, int x, int y, float flexibleWidth = 0.1f, float flexibleHeight = -1)
        {
            var le = rectTransform.gameObject.AddOrGetComponent<LayoutElement>();
            if (x <= 0) x = defaultWidth;
            if (y <= 0) y = defaultHeight;
            le.preferredWidth = x;
            le.preferredHeight = y;
            le.flexibleWidth = flexibleWidth;
            if (flexibleHeight != -1);
            le.flexibleHeight = flexibleHeight;
            return le;
        }

        public static PanelInfo AddExpandable(this PanelInfo target, string label)
        {
            return target.content.AddExpandable(label);
        }

        public static PanelInfo AddExpandable(this RectTransform target, string label, bool padLeft = false)
        {

            var thisExp = target.AddPanel(label);
            PanelInfo info = new PanelInfo(thisExp);
            info.AddTop();
            // var le = info.top.rect.LayoutElement();

            var layout = thisExp.AddVerticalLayout();
            if (padLeft)
                layout.AddLeftPaddToLayout(leftPad);
            info.SetLabel(label);
            return info;
        }
        public static Color SquareColor(this Color src)
        {
            return new Color(src.r * src.r, src.g * src.g, src.b * src.b, src.a * src.a);
        }
        public static Color ButtonColor()
        {
            return DarkBGColor();
        }
        public static PanelInfo CreateScrollRectPanel(this RectTransform target, string label, bool addFrames = false)
        {

            PanelInfo info = PanelInfo.ConvertToPanel(target, addFrames);
            info.mainRect.AddLayoutElement(300, 300, 0.1f, 0.5f);
            info.AddRectForContent();
            info.ConvertContentForSCrollVEiw();
            info.content.AddVerticalLayout().AddLeftPaddToLayout(leftPad);
            info.SetLabel(label);
            return info;

        }
        public static float scrollbarWidth { get { return 3; } }
        public static int defaultWidth { get { return 100; } }
        public static int defaultHeight { get { return 500; } }
        public static Color DarkBGColor() { return Color.black * .4f; }
        public static RectTransform BuildMenuBar(RectTransform target)
        {
            var menu = (new GameObject("Menu " + counter, typeof(Image))).GetComponent<RectTransform>().SetParentAndFill(target);
            menu.SetAnchorTop(true, true);
            menu.SetColor(Color.black * .4f);
            menu.sizeDelta = new Vector2(-34, topHeight * 2);
            menu.anchoredPosition -= Vector2.up * 5;
            menu.SetAsLastSibling();
            return menu;
        }

        public static Scrollbar BuildScrollBar(Transform target)
        {
            var scrollBarRect = (new GameObject("ScrollBar " + counter, typeof(Image))).GetComponent<RectTransform>();
            scrollBarRect.SetParentAndResetScale(target);
            scrollBarRect.SetColor(DarkBGColor());
            scrollBarRect.sizeDelta = new Vector2(scrollbarWidth, defaultHeight);
            //scrollRect.
            RectTransform slideArea = (new GameObject("SlideArea", typeof(RectTransform))).GetComponent<RectTransform>().SetParentAndFill(scrollBarRect);
            // slideArea.SetColor();
            RectTransform handle = (new GameObject("Handle", typeof(Image))).GetComponent<RectTransform>().SetParentAndFill(slideArea);
            handle.SetColor(GetHandleColorSemi());
            Scrollbar s = scrollBarRect.gameObject.AddComponent<Scrollbar>();
            s.direction = Scrollbar.Direction.BottomToTop;
            s.targetGraphic = handle.GetComponent<Image>();
            //s.fillRect = fill;
            s.handleRect = handle;
            s.value = 1;
            var le = s.gameObject.AddOrGetComponent<LayoutElement>();
            le.flexibleHeight = .1f;
            le.preferredWidth = scrollbarWidth;
            le.ignoreLayout = true;
            return s;
        }
        static Color GetHandleColorSemi()
        {
            Color c = GetHandleColor();
            c.a = 0.2f;
            return c;
        }
        static Color GetHandleColor()
        {
            return new Color(0.1f, 0.8f, 0.1f, .8f);
        }

        public static SliderInfo BuildSlider(Transform target, string name)
        {
            SliderInfo sliderInfo = new SliderInfo(target as RectTransform);
            var sliderRect = (new GameObject("Slider" + counter, typeof(RectTransform))).GetComponent<RectTransform>();
            sliderRect.SetParentAndResetScale(target);
            sliderRect.sizeDelta = new Vector2(defaultWidth, sliderHeight);
            RectTransform background = (new GameObject("Background", typeof(Image))).GetComponent<RectTransform>().SetParentAndFill(sliderRect);
            background.SetColor(Color.black * .4f);
            background.PadTop(3).PadBottom(3);
            sliderInfo.background = background;
            RectTransform fillArea = (new GameObject("Fill Area", typeof(RectTransform))).GetComponent<RectTransform>().SetParentAndFill(sliderRect);
            sliderInfo.fillArea = fillArea;

            RectTransform fill = (new GameObject("Fill", typeof(Image))).GetComponent<RectTransform>().SetParentAndFill(fillArea);
            sliderInfo.fill = fill.GetComponent<Image>();
            fill.SetColor(SquareColor(RandomBGColor()));
            fill.anchorMin = new Vector2(0, 0);
            fill.anchorMax = new Vector2(0, 1);
            fill.sizeDelta = new Vector2(0, 0);
            RectTransform slideArea = (new GameObject("HandleSlideArea", typeof(RectTransform))).GetComponent<RectTransform>().SetParentAndFill(sliderRect);
            sliderInfo.slideArea = slideArea;
            fillArea.PadTop(4).PadBottom(5).PadLeft(5).PadBottom(4);
            slideArea.PadLeft(5).PadRight(5);

            RectTransform handle = (new GameObject("Handle", typeof(Image))).GetComponent<RectTransform>(); //.SetParentAndFill(sliderRect);
            handle.SetParentAndResetScale(slideArea);
            handle.anchorMin = new Vector2(0, 0);
            handle.anchorMax = new Vector2(0, 1);
            handle.SetAnchorLeft(true);
            handle.sizeDelta = new Vector2(handleWidth, 0);
            handle.SetColor(GetHandleColor());
            sliderInfo.Handle = handle.GetComponent<Image>();

            //	background.SetColor(MoreUIExentsions.RandomBGColor());
            RectTransform textRect = (new GameObject("Label", typeof(Text))).GetComponent<RectTransform>().SetParentAndFill(sliderRect);
            Text text = textRect.GetComponent<Text>();
            sliderInfo.sliderLabel = text;
            SetDefaultFont(text);
            text.alignment = TextAnchor.MiddleLeft;
            text.fontSize = 10;
            text.SetText(name);

            textRect.PadLeft(6);
            textRect.PadRight(40);
            RectTransform textValueRect = (new GameObject("Label", typeof(Text))).GetComponent<RectTransform>().SetParentAndFill(sliderRect);
            textValueRect.SetAnchorRight(true, true);
            textValueRect.sizeDelta = new Vector2(50, 0);

            Text valueText = textValueRect.GetComponent<Text>();

            SetDefaultFont(valueText);
            valueText.alignment = TextAnchor.MiddleRight;
            valueText.fontSize = 10;
            textValueRect.PadRight(3);
            sliderInfo.sliderValueDisplay = textValueRect.gameObject.AddComponent<Z.SliderValueDisplay>();
            Slider s = sliderRect.gameObject.AddComponent<Slider>();
            sliderInfo.slider = s;
            s.targetGraphic = handle.GetComponent<Image>();
            s.fillRect = fill;
            s.handleRect = handle;
            var le = s.gameObject.AddOrGetComponent<LayoutElement>();
            le.flexibleWidth = .1f;
            le.preferredHeight = sliderHeight;
            le.preferredWidth = 200;
            return sliderInfo;
        }

        // public static PanelInfo BuildScrollRectAsContent(this PanelInfo target)
        // {
        //     var scrollViewRect = (new GameObject("ScrollView " + counter, typeof(ScrollRect))).GetComponent<RectTransform>();
        //     scrollViewRect.SetParentAndFill(target.mainRect);
        //     var scrollbar = BuildScrollBar(scrollViewRect);
        //     var scrollbarRect = scrollbar.GetComponent<RectTransform>();
        //     scrollbarRect.SetAnchorRight(true, true);
        //     scrollbarRect.sizeDelta = new Vector2(scrollbarWidth, 0);
        //     scrollViewRect.PadTop(topHeight);
        //     var viewportRect = (new GameObject("Viewport " + counter, typeof(Image), typeof(Mask))).GetComponent<RectTransform>().SetParentAndFill(scrollViewRect);
        //     viewportRect.GetComponent<Mask>().showMaskGraphic = false;
        //     target.content = (new GameObject("Content " + counter, typeof(Image))).GetComponent<RectTransform>().SetParentAndFill(viewportRect);
        //     // target.content.SetColor(Color.black * 0.2f);
        //     target.content.GetComponent<Image>().enabled = false;
        //     viewportRect.PadRight(scrollbarWidth + padSize); //.PadTop(topHeight)
        //     viewportRect.PadLeft(padSize); //.PadTop(topHeight)
        //     var scrollrect = scrollViewRect.GetComponent<ScrollRect>();
        //     scrollrect.verticalScrollbar = scrollbar;
        //     scrollrect.content = target.content;
        //     scrollrect.horizontal = false;
        //     //scrollViewRect.addl
        //     target.content.pivot = new Vector2(0, 1);
        //     var le = scrollViewRect.gameObject.AddOrGetComponent<LayoutElement>();
        //     le.flexibleHeight = 0.1f;
        //     le.flexibleWidth = 0.1f;
        //     le.preferredWidth = defaultWidth;
        //     le.preferredHeight = defaultHeight;
        //     return target;
        // }

        public static void AddRectForContent(this PanelInfo target)
        {
            target.content = target.mainRect.AddPanelWithSpace("Content");
        }
        public static void ConvertContentForSCrollVEiw(this PanelInfo target)
        {

            var scrollViewRect = target.content;

            //  (new GameObject("ScrollView " + counter, typeof(ScrollRect))).GetComponent<RectTransform>();

            //   scrollViewRect.SetParentAndFill(target.mainRect);
            var scrollbar = BuildScrollBar(scrollViewRect);
            var scrollbarRect = scrollbar.GetComponent<RectTransform>();
            scrollbarRect.SetAnchorRight(true, true);

            scrollbarRect.sizeDelta = new Vector2(scrollbarWidth, 0);
            scrollbarRect.anchoredPosition = Vector2.zero;

            scrollViewRect.PadTop(topHeight);
            var viewportRect = (new GameObject("Viewport " + counter, typeof(Image), typeof(Mask))).GetComponent<RectTransform>().SetParentAndFill(scrollViewRect);
            viewportRect.GetComponent<Mask>().showMaskGraphic = false;
            target.content = (new GameObject("Content " + counter, typeof(Image))).GetComponent<RectTransform>().SetParentAndFill(viewportRect);
            // target.content.SetColor(Color.black * 0.2f);
            target.content.GetComponent<Image>().enabled = false;
            viewportRect.PadRight(scrollbarWidth + padSize); //.PadTop(topHeight)
            viewportRect.PadLeft(padSize); //.PadTop(topHeight)

            var scrollrect = scrollViewRect.gameObject.AddComponent<ScrollRect>();
            scrollrect.verticalScrollbar = scrollbar;
            scrollrect.content = target.content;
            scrollrect.horizontal = false;
            //scrollViewRect.addl
            target.content.pivot = new Vector2(0, 1);
            var le = scrollViewRect.gameObject.AddOrGetComponent<LayoutElement>();
            le.flexibleHeight = 0.1f;
            le.flexibleWidth = 0.1f;
            le.preferredWidth = defaultWidth;
            le.preferredHeight = defaultHeight;
        }

        public static RectOffset GetPaddingWithTop(int padSize, bool extraSpace)
        {
            RectOffset padding = GetPadding(padSize);
            if (extraSpace)
                padding.top += topHeight + spacing;

            return padding;
        }
        public static RectOffset GetPadding(int padSize)
        {
            RectOffset padding = new RectOffset();
            padding.top = padSize;
            padding.left = padSize; //+ leftPad;
            padding.bottom = padSize;
            padding.right = 0; //padSize;
            return padding;
        }
        //   public static RectOffset GetPadding(int padSize)
        // {
        //     RectOffset padding = new RectOffset();
        //     padding.top = padSize;
        //     padding.left = padSize ;
        //     padding.bottom = padSize;
        //     padding.right = 0;//padSize;
        //     return padding;
        // }
        public static VerticalLayoutGroup AddVerticalLayout(this Transform target, bool extraSpace = false)
        {
            var thisRect = target.GetComponent<RectTransform>();
            thisRect.pivot = new Vector2(0, 1);
            var layout = target.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            layout.padding = GetPaddingWithTop(padSize, extraSpace);
            layout.spacing = spacing;
            var sizefitter = target.gameObject.AddComponent<ContentSizeFitter>();
            sizefitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return layout;
        }
        public static void AddLeftPaddToLayout(this VerticalLayoutGroup group, int amount)
        {
            var currentpad = GetPadding(padSize); // group.padding;
            currentpad.left += amount;
            // Debug.Log("paddinglet "+currentpad.left);
            group.padding = currentpad;

        }

        public static HorizontalLayoutGroup AddHorizontalLayout(this RectTransform target)
        {
            var thisRect = target.GetComponent<RectTransform>();
            var layout = target.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            layout.padding = GetPadding(4);
            layout.spacing = 5;

            //  layout.padding = GetPaddingWithTop(padSize, extraSpace);
            //   layout.spacing = spacing;
            //  var sizefitter = target.gameObject.AddComponent<ContentSizeFitter>();
            //  sizefitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return layout;
        }
        // public static PanelInfo BuildPanelWithFrames(RectTransform target, string name)
        // {
        //     var info = new PanelInfo(target);
        //     info.content = UIBuilder.AddContentRectWithLayout(target, name); //.content;
        //     // info.frame = UIBuilder.AddFrame(info.mainRect) as RectTransform;
        //     info.AddFrame();
        //     info.AddTopToFrame(true);
        //     // info.top =  UIBuilder.AddTop(info.frame);
        //     target.SetLabel(name, true);
        //     info.top.rect.SetSiblingIndex(0);
        //     var fold = target.GetComponentInChildren<SimpleFoldController>();
        //     fold.foldRoot = target;
        //     fold.objectsToIgnore.Add(info.frame.gameObject);
        //     for (int i = 0; i < 5; i++)
        //         BuildButton(info.content);
        //     var le = info.mainRect.GetComponent<LayoutElement>();
        //     le.preferredHeight = (target as RectTransform).rect.height;
        //     return info;
        // }

        // public static PanelInfo BuildScrollPanelWithFrames(PanelInfo info, string name)
        // {
        //     //   PanelInfo info = new PanelInfo();

        //     info.mainRect = UIBuilder.AddContentRectWithLayout(info.mainRect, name); //.mainRect;
        //     info.AddFrame();
        //     // info.frame = UIBuilder.AddFrame(info.mainRect).GetComponent<RectTransform>();
        //     info.top = UIBuilder.AddTop(info.frame);
        //     info.SetLabel(name, true);
        //     info.top.rect.SetSiblingIndex(0);
        //     var fold = info.mainRect.GetComponentInChildren<SimpleFoldController>();
        //     fold.foldRoot = info.mainRect;
        //     fold.objectsToIgnore.Add(info.frame.gameObject);
        //     info.content = info.mainRect.AddPanel("ScrollView");
        //     info.ConvertContentForSCrollVEiw();
        //     // var le = info.mainRect.GetComponent<LayoutElement>();
        //     // le.preferredHeight = info.mainRect.rect.height;
        //     return info;
        // }

        // public static PanelInfo BuildScrollRectAsContentAndADdLayout(PanelInfo target, bool extraSpace = false)
        // {
        //     BuildScrollRectAsContent(target);
        //     target.content.AddVerticalLayout(extraSpace);
        //     return target;
        // }
        // public static PanelInfo BuildScrollRectWithConentSizeFitter(PanelInfo target, bool extraSpace = false)
        // {
        //  target = BuildScrollRect(target);
        //     target.content.AddVerticalLayout(extraSpace);
        //     return target;
        // }
        public static PanelInfo BuildBasicPanelTopOnly(RectTransform target, string name)
        {
            if (target == null)
            {
                Debug.Log("no conetn");
            }
            //target.addre
            PanelInfo info = new PanelInfo(target);
            var panel = AddContentRectWithLayout(target, name);
            info.top = panel.AddTop();
            info.SetLabel(name);
            return info;
        }

        public static PanelInfo BuildHierarchyPanel(RectTransform target, string name, bool addToggle = true)
        {
            if (target == null)
            {
                Debug.Log("no conetn");
            }
            var rect = AddContentRectWithLayout(target, name);
            PanelInfo info = new PanelInfo(rect);
            info.content.GetComponentInChildren<VerticalLayoutGroup>().AddLeftPaddToLayout(leftPad);
            info.top = AddTop(info.mainRect, true);
            if (addToggle) info.AddToggle();
            // toggle.GetComponent<RectTransform>().localPosition = new Vector3(toggleDotHeight * 1.5f, 0, 0);
            return info;
        }
        public static Toggle AddToggle(this PanelInfo info)
        {
            if (info.top == null)
            {
                Debug.Log("no top");
                return null;;
            }
            info.top.toggle = BuildToggleDot(info.top.rect);
            var toggleRect = info.top.toggle.GetComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0, 0.5f);
            toggleRect.anchorMax = new Vector2(0, 0.5f);
            toggleRect.sizeDelta = new Vector2(toggleDotHeight, toggleDotHeight);
            toggleRect.anchoredPosition = new Vector2(toggleDotHeight + 2, 0);
            info.top.label.GetComponent<RectTransform>().PadLeft(toggleDotHeight * 1.5f);
            return info.top.toggle;
        }
        // public static PanelInfo AddContentRectWithLayout(this PanelInfo target, string name, bool extraSpace = true)
        // {
        //     return target.mainRect.AddContentRectWithLayout(name, extraSpace);
        // }
        public static RectTransform AddPanel(this RectTransform target, string name)
        {
            var panel = (new GameObject(name + counter, typeof(Image))).GetComponent<RectTransform>();

            // panel.SetColor(RandomBGColor() * .4f);
            panel.GetComponent<Image>().enabled = false;
            panel.SetParentAndFill(target);
            panel.pivot = new Vector2(0, 1);
            //panel.sizeDelta = new Vector2(defaultWidth, defaultHeight);
            //panel.sizeDelta = new Vector2(defaultWidth, defaultHeight);
            // var le = panel.gameObject.AddOrGetComponent<LayoutElement>();
            // le.preferredWidth = defaultHeight;
            // le.flexibleWidth = 0.1f;
            // le.preferredHeight = target.rect.height;
            //  panel.offsetMax=Vector2.zero;
            //  panel.offsetMin=Vector2.zero;
            //	le.preferredHeight = defaultSize;
            //   info.content = panel;
            //  panel.AddVerticalLayout(extraSpace);
            return panel;
        }

        public static RectTransform AddPanelWithSpace(this RectTransform target, string name)
        {
            var rect = target.AddPanel(name);
            rect.PadTop(topHeight);
            return rect;

        }

        // public static RectTransform AddContentAndLE(this RectTransform target, string name, bool extraSpace = true)
        // {

        //     //  info.mainRect = panel;
        //    var newpanel= target.AddPanel(name);
        //     //    var info = target.AddPanel(name, extraSpace);
        //     var le = newpanel.AddOrGetComponent<LayoutElement>();
        //     le.preferredWidth = defaultHeight;
        //     le.flexibleWidth = 0.1f;
        //     //  panel.offsetMax=Vector2.zero;
        //     //  panel.offsetMin=Vector2.zero;
        //     //	le.preferredHeight = defaultSize;
        //     // info.content = panel;
        //    // target.AddVerticalLayout(extraSpace);
        //     return newpanel;
        // }

        public static RectTransform AddContentRectWithLayout(this RectTransform target, string name)
        {

            PanelInfo info = new PanelInfo(target);

            info.content = AddPanel(target, name);
            //  info.mainRect = panel;
            //   info.content = info.mainRect.AddPanel(name, extraSpace);
            //    var info = target.AddPanel(name, extraSpace);
            // var le = info.content.AddOrGetComponent<LayoutElement>();
            // le.preferredWidth = defaultHeight;
            //  le.flexibleWidth = 0.1f;
            //  panel.offsetMax=Vector2.zero;
            //  panel.offsetMin=Vector2.zero;
            //	le.preferredHeight = defaultSize;
            // info.content = panel;
            info.content.AddVerticalLayout(true);
            return info.content;
        }
        // public static RectTransform AddFrame(this PanelInfo target)
        // {
        //     target.frame = target.mainRect.AddFrame();
        //     return target.frame;
        // }
        public static RectTransform AddFrame(this PanelInfo targetPanel)
        {
            var target = targetPanel.mainRect;
            if (GameObject.FindObjectOfType(typeof(LayoutBorderControl)) == null)
            {
                var borderControl = new GameObject("borderContorl", typeof(LayoutBorderControl)).GetComponent<LayoutBorderControl>();
            }
            var frame = (new GameObject("Frame " + counter, typeof(RectTransform))).GetComponent<RectTransform>().SetParentAndFill(target);
            var le = frame.gameObject.AddOrGetComponent<LayoutElement>();
            le.ignoreLayout = true;
            var targetLe = targetPanel.mainRect.GetComponent<LayoutElement>();
            for (int i = 0; i < 4; i++)
            {
                var thisframe = (new GameObject("Frame " + counter, typeof(RectTransform), typeof(Image))).GetComponent<RectTransform>().SetParentAndResetScale(frame);
                var dragger = thisframe.gameObject.AddOrGetComponent<LayoutBorderDragger>();
                dragger.side = (Z.LayoutPanel.Side) i;
                dragger.OnValidate();
                //if (dragger.side.isHorizontal()) 
                dragger.elementToResize = targetLe; // target.GetComponentInParent<LayoutElement>();
            }
            frame.SetAsFirstSibling();
            targetPanel.frame = frame;
            return frame;
        }
        public static void SetDefaultFont(Text text)
        {
            if (UIDefaultFont.defaultFont != null) text.font = UIDefaultFont.defaultFont;
            if (UIDefaultFont.defaultFontSize != 0) text.fontSize = UIDefaultFont.defaultFontSize;
        }
        public static Text AddFillingText(this RectTransform rect)
        {
            var text = rect.AddFillingChild().gameObject.AddComponent<Text>();
            SetDefaultFont(text);
            text.color = Color.white;
            return text;
        }
        public static InputField BuildInputField(RectTransform target, string label = "InputField", string placeholder = "Input")
        {
            var subrect = target.AddFillingChild();

            subrect.PadLeft(30);
            var textlab = subrect.AddFillingText();
            var textrect = textlab.GetComponent<RectTransform>();
            //textrect.anchorMin = new Vector2(0, 0);
            //            textrect.anchorMax = new Vector2(1, 1);
            textrect.PadLeft(4);
            //textrect.sizeDelta
            textlab.text = label;
            textlab.fontSize -= 2;
            textlab.alignment = TextAnchor.MiddleLeft;
            var inputRect = (new GameObject("InputField " + counter, typeof(Image), typeof(InputField))).GetComponent<RectTransform>();
            // inputRect.sizeDelta = new Vector2(defaultWidth, buttonHeight * 1.5f);
            // inputRect.SetColor(Color.white);
            inputRect.SetColor(Color.black + Color.white * .1f);

            inputRect.SetParentAndFill(subrect);
            inputRect.pivot = new Vector2(1, 0.5f);
            // inputRect.anchoredPosition= 

            inputRect.PadTop(2).PadBottom(2).PadLeft(50);;

            // inputRect.GetComponent<Image>().color = ButtonColor();
            var textRectplaceholder = (new GameObject("Text " + counter, typeof(Text))).GetComponent<RectTransform>();
            textRectplaceholder.SetParentAndFill(inputRect);
            Text textplaceholder = textRectplaceholder.GetComponent<Text>();
            textplaceholder.text = placeholder;
            textplaceholder.fontSize = buttonFontSize;
            SetDefaultFont(textplaceholder);
            textplaceholder.alignment = TextAnchor.MiddleCenter;
            var textRect = (new GameObject("Text " + counter, typeof(Text))).GetComponent<RectTransform>();
            textRect.SetParentAndFill(inputRect);
            var input = inputRect.GetComponent<InputField>();
            input.text = label;
            Text text = textRect.GetComponent<Text>();
            text.text = label;
            text.color = Color.white;
            text.fontSize = buttonFontSize;
            SetDefaultFont(text);
            input.placeholder = textplaceholder;
            textplaceholder.text = "Placeholder";
            textplaceholder.color = Color.gray * .5f;
            text.supportRichText = false;
            textplaceholder.supportRichText = false;
            textplaceholder.fontSize -= 2;

            input.textComponent = text;
            text.alignment = TextAnchor.MiddleCenter;
            subrect.AddLayoutElement(defaultWidth, (int) (1.2f * buttonHeight));
            // var le = subrect.gameObject.AddOrGetComponent<LayoutElement>();
            // le.preferredHeight = buttonHeight;
            // le.preferredWidth = defaultWidth;
            // le.flexibleWidth = .01f;
            return input;
        }
        public static RectTransform AddFillingChild(this RectTransform target)
        {
            var nrewrect = (new GameObject(target.name + counter, typeof(RectTransform))).GetComponent<RectTransform>().SetParentAndFill(target);
            return nrewrect;
        }
        public static Button BuildButton(Transform target, string label = "Button")
        {
            var buttonRect = (new GameObject("Button " + counter, typeof(Image), typeof(Button))).GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(defaultWidth, buttonHeight);
            buttonRect.SetParentAndResetScale(target);
            buttonRect.GetComponent<Image>().color = ButtonColor();
            var textRect = (new GameObject("Text " + counter, typeof(Text))).GetComponent<RectTransform>();
            textRect.SetParentAndFill(buttonRect);
            Text text = textRect.GetComponent<Text>();
            text.text = label;

            text.fontSize = buttonFontSize;
            SetDefaultFont(text);
            text.alignment = TextAnchor.MiddleCenter;
            //   buttonRect.addlay

            buttonRect.AddLayoutElement();
            // var le = buttonRect.gameObject.AddOrGetComponent<LayoutElement>();
            //             le.preferredHeight = buttonHeight;
            //             le.preferredWidth = defaultWidth;
            //             le.flexibleWidth = .01f;
            return buttonRect.GetComponent<Button>();
        }

        public static Toggle BuildToggleDot(RectTransform target)
        {
            var toggledot = (new GameObject("Toggle " + counter, typeof(Image), typeof(Toggle))).GetComponent<RectTransform>();
            toggledot.sizeDelta = new Vector2(toggleDotHeight, toggleDotHeight);
            toggledot.SetParentAndResetScale(target);
            toggledot.GetComponent<Image>().color = ButtonColor();
            var checkmark = (new GameObject("Checkmark " + counter, typeof(Image))).GetComponent<RectTransform>();
            checkmark.SetParentAndFill(toggledot);
            var checkImage = checkmark.GetComponent<Image>();
            checkmark.Pad(2);
            checkImage.color = GetHandleColor();
            var toggle = toggledot.GetComponent<Toggle>();;
            toggle.onValueChanged.AddListener((x) => checkImage.enabled = x);
            checkImage.enabled = false;
            var le = toggledot.gameObject.AddOrGetComponent<LayoutElement>();
            le.preferredHeight = toggleDotHeight;
            le.preferredWidth = toggleDotHeight;
            return toggle;
        }
        public static Toggle BuildToggle(RectTransform target, string label = "Button")
        {
            var subrect = target.AddFillingChild();

            subrect.PadLeft(30);
            var textlab = subrect.AddFillingText();
            var textrect = textlab.GetComponent<RectTransform>();
            //textrect.anchorMin = new Vector2(0, 0);
            //            textrect.anchorMax = new Vector2(1, 1);
            textrect.PadLeft(4);
            //textrect.sizeDelta
            textlab.text = label;
            textlab.fontSize -= 2;
            textlab.alignment = TextAnchor.MiddleLeft;

            var buttonRect = (new GameObject("Toggle " + counter, typeof(Image), typeof(Toggle))).GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(toggleDotHeight, toggleDotHeight);
            buttonRect.SetParentAndResetScale(subrect);
            buttonRect.GetComponent<Image>().color = ButtonColor();
            buttonRect.anchorMin = new Vector2(1, 0);
            buttonRect.anchorMin = new Vector2(1, 1);
            buttonRect.pivot = new Vector2(1, 0.5f);
            buttonRect.anchoredPosition = new Vector2(-5, 0);
            buttonRect.sizeDelta = new Vector2(toggleDotHeight, toggleDotHeight);
            //  var textRect = (new GameObject("Text " + counter, typeof(Text))).GetComponent<RectTransform>();
            var checkmark = (new GameObject("Checkmark " + counter, typeof(Image))).GetComponent<RectTransform>();
            checkmark.SetParentAndFill(buttonRect);
            var checkImage = checkmark.GetComponent<Image>();
            checkmark.Pad(2);
            checkImage.color = GetHandleColor();
            var toggle = buttonRect.GetComponent<Toggle>();;
            toggle.onValueChanged.AddListener((x) => checkImage.enabled = x);
            checkImage.enabled = false;
            subrect.AddLayoutElement(defaultWidth, toggleDotHeight + 4);
            var le = subrect.gameObject.AddOrGetComponent<LayoutElement>();

            return toggle;
        }
        public static RectTransform BuildEmptyRect(Transform target)
        {
            var buttonRect = (new GameObject("Button " + counter, typeof(Image))).GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(defaultWidth, buttonHeight);
            buttonRect.SetParentAndResetScale(target);
            buttonRect.GetComponent<Image>().enabled = false;
            var le = buttonRect.gameObject.AddOrGetComponent<LayoutElement>();

            le.preferredHeight = buttonHeight;
            le.preferredWidth = defaultWidth;
            le.flexibleWidth = .01f;
            return buttonRect;
        }
        public static TopInfo AddTopToFrame(this PanelInfo src, bool button = false)
        {
            return src.frame.AddTop(button);
        }
        public static TopInfo AddTopToMain(this PanelInfo src, bool button = false)
        {
            src.top = src.mainRect.AddTop(button);
            return src.top;
        }
        public static TopInfo AddTop(this PanelInfo src, bool button = false)
        {
            if (src.frame != null && src.frame.rect != null)
            {
                src.top = src.AddTopToFrame();
            }
            else
            {
                src.top = src.AddTopToMain();

            }
            return src.top;
        }

        public static TopInfo AddTop(this Transform target, bool button = false)
        {
            RectTransform top = (new GameObject("Top " + counter, typeof(Image))).GetComponent<RectTransform>();
            TopInfo info = new TopInfo(target);

            var topimage = top.GetComponent<Image>();
            info.image = topimage;
            info.image.SetColor(RandomBGColor());

            var thisRect = top.GetComponent<RectTransform>();
            info.rect = thisRect;
            thisRect.SetParentAndResetScale(target);
            thisRect.SetAnchorTop(true, true);
            thisRect.localScale = Vector3.one;
            thisRect.offsetMin = new Vector2(0, -topHeight);
            thisRect.offsetMax = new Vector2(0, 0);
            GameObject toplabel = new GameObject("Label " + counter, typeof(Text));
            toplabel.transform.SetParent(thisRect);
            var textlanbel = toplabel.GetComponent<Text>();
            textlanbel.text = "Label";
            info.label = textlanbel;
            SetDefaultFont(textlanbel);
            textlanbel.alignment = TextAnchor.MiddleLeft;
            var labelrect = toplabel.GetComponent<RectTransform>();
            labelrect.localScale = Vector3.one;
            labelrect.anchorMin = Vector2.zero;
            labelrect.anchorMax = Vector2.one;
            labelrect.offsetMin = new Vector2(5, 0);
            labelrect.offsetMax = new Vector2(-30, 0);

            GameObject foldButton = new GameObject("Foldbutton" + counter, typeof(Text), typeof(Button));
            var foldrect = foldButton.GetComponent<RectTransform>();

            foldrect.SetParent(thisRect);
            var foldtext = foldButton.GetComponent<Text>();
            SetDefaultFont(foldtext);
            info.foldLabel = foldtext;
            info.foldButton = foldButton.GetComponent<Button>();
            foldtext.text = "<";
            foldtext.alignment = TextAnchor.MiddleCenter;
            foldrect.localScale = Vector3.one;
            foldrect.anchorMin = new Vector2(1, 0);
            foldrect.anchorMax = new Vector2(1, 1);
            foldrect.offsetMin = new Vector2(-22, -2);
            foldrect.offsetMax = new Vector2(2, 2);

            var le = top.gameObject.AddOrGetComponent<LayoutElement>();
            le.flexibleWidth = 0.1f;
            le.preferredHeight = topHeight;
            le.preferredWidth = 150;
            var foldcontrol = top.gameObject.AddComponent<SimpleFoldController>();
            foldcontrol.foldButton = foldButton.GetComponent<Button>();
            foldcontrol._foldLabelText = foldtext;
            if (button)
            {
                info.button = le.gameObject.AddComponent<Button>();
            }
            top.SetAsFirstSibling();
            return info;
        }

    }

}