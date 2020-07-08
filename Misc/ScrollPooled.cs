using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//v..2 tiny fix

public class ScrollPooled : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
#pragma warning disable 414

	// ScrollRect scrollRect { get { if (_scrollRect == null) _scrollRect = GetComponent<ScrollRect>(); return _scrollRect; } }
	// ScrollRect _scrollRect;
	public Scrollbar scrollBar;
	RectTransform rectTransform { get { if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }
	RectTransform _rectTransform;

	// float currentHeight { get { return rectTransform.rect.height; s} }
	[SerializeField]
	public RectTransform content;
	public float _lastViewportHeight; // { get { return } }

	public System.Action<int, GameObject> OnFill = delegate {};
	Dictionary<ListItem, int> itemIndexDict = new Dictionary<ListItem, int>();
	Dictionary<ListItem, int> itemSiblingIndexDict = new Dictionary<ListItem, int>();
	public int viewStartIndex = 0;
	public int viewEndIndex = 0;
	public int maxItems;
	public float lastScrollAmt;
	public int topSourceIndex = 0;
	public int bottomSourceIndex = 0;
	bool isRunning;
	[Header("Options")]
	public float veloDamping = 1.1f;
	float minVelocity = 3;
	private Dictionary<int, float> heights = new Dictionary<int, float>();

	private Dictionary<int, float> positions = new Dictionary<int, float>();

	public float totalHeight;
	public RectTransform topmostobject;
	public RectTransform bottomobject;
	Vector2 dragBeginPos;
	float velocity;
	public int additionalItemCount = 20;
	[Header("in pixels")]
	public float swapMargin = 50;

	bool isHovered;
	float scrollSpeed = 10;
	[Header("Geometry")]
	public int spacing = 1;
	[Header("Horixontal when sizing itemss")]
	public Vector2Int margins;

	public ListItem itemTemplate;
	// public Transform content;
	protected List<ListItem> items;
	int GetHeight(RectTransform rect)
	{
		var le = rect.GetComponent<LayoutElement>();
		if (le != null && le.preferredHeight > 0) return (int) le.preferredHeight;
		return (int) rect.rect.height;
	}
	void SetOffsets(RectTransform thisRect)
	{
		thisRect.offsetMin = new Vector2(margins.x, thisRect.offsetMin.y);
		thisRect.offsetMax = new Vector2(-margins.y, thisRect.offsetMax.y);
		thisRect.anchorMin = new Vector2(0, 1);
		thisRect.anchorMax = new Vector2(1, 1);
		thisRect.sizeDelta = new Vector2(margins.x + margins.y, GetHeight(thisRect));
	}
	protected void OnValidate()
	{
		if (zBench.PrefabModeIsActive(gameObject)) return;
		if (scrollBar == null) scrollBar = GetComponentInChildren<Scrollbar>();
		if (content != null)
			if (liveUpdateOnAll)
			{
				for (int i = 0; i < content.transform.childCount; i++)
				{
					var thisRect = content.transform.GetChild(i).GetComponent<RectTransform>();
					if (thisRect != null)
						SetOffsets(thisRect);
				}
			}

	}
	void Awake()
	{
		var vert = content.GetComponent<VerticalLayoutGroup>();
		if (vert != null)
		{
			Debug.Log("Destoring " + name, gameObject);
			Destroy(vert);
		}
	}
	void Start()
	{
		OnValidate();

		if (scrollBar == null) scrollBar = GetComponentInChildren<Scrollbar>();
		if (scrollBar == null) scrollBar = transform.parent.GetComponentInChildren<Scrollbar>();
		if (scrollBar != null) scrollBar.onValueChanged.AddListener(OnScrollbar);
		if (itemTemplate != null)
		{
			itemTemplate.transform.localScale = Vector3.one;
			if (itemTemplate.GetComponentInParent<ScrollPooled>() != this)
			{
				itemTemplate = Instantiate(itemTemplate, content);
			}

			SetOffsets(itemTemplate.rectTransform);

		}
	}
	ItemInterface itemInterface;
	protected virtual ListItem CreateItem()
	{
		ListItem item = null;

		if (items == null) items = new List<ListItem>();
		try
		{
			item = Instantiate(itemTemplate, content);
			items.Add(item);
			item.gameObject.SetActive(true);
		}
		catch (System.Exception e)
		{
			Debug.Log("Creating of prefab failed on " + name + " " + e.Message, gameObject);
		}
		return item;
	}
	protected void SetListSize(int size)
	{
		if (itemTemplate == null) return;
		itemTemplate.gameObject.SetActive(false);
		if (items == null) items = new List<ListItem>();
		while (items.Count > size)
		{
			var thisItem = items[0];
			items.Remove(thisItem);
			Destroy(thisItem.gameObject);
		}
		while (items.Count < size)
			CreateItem();
	}
	public void InitList(ItemInterface itemInterface, int maxItems)
	{
		this.itemInterface = itemInterface;
		this.maxItems = maxItems;
		heights = new Dictionary<int, float>();
		positions = new Dictionary<int, float>();
		int createItems = 0;

		totalHeight = spacing;
		float templateheight = itemTemplate.rectTransform.rect.height;
		_lastViewportHeight = rectTransform.rect.height;
		for (int i = 0; i < maxItems; i++)
		{
			float thisHeight = itemInterface.GetHeight(i);
			if (thisHeight == 0) thisHeight = templateheight;
			totalHeight += thisHeight + spacing;
			heights.Add(i, thisHeight);
			positions.Add(i, -totalHeight);
			if (createItems == 0 && (totalHeight > _lastViewportHeight))
				createItems = i;
		}
		content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
		if (scrollBar != null) scrollBar.gameObject.SetActive(createItems != 0);
		if (createItems == 0) createItems = maxItems;
		createItems += additionalItemCount;
		if (createItems > maxItems) createItems = maxItems;
		viewEndIndex = createItems;
		SetListSize(createItems);
		topSourceIndex = 0;
		bottomSourceIndex = createItems - 1;
		itemIndexDict = new Dictionary<ListItem, int>();
		itemSiblingIndexDict = new Dictionary<ListItem, int>();
		viewEndIndex = createItems;
		for (int i = 0; i < items.Count; i++)
		{
			items[i].rectTransform.pivot = new Vector2(0, 0);
			itemIndexDict.Add(items[i], i);
			itemSiblingIndexDict.Add(items[i], i);
		}
		topmostobject = items[0].rectTransform;
		bottomobject = items[items.Count - 1].rectTransform;
		UpdateFill();
		UpdateScrollBar();
		Invoke("UpdateScrollBar", 1);
	}
	public void InitList(ItemInterface itemInterface)
	{

		InitList(itemInterface, itemInterface.GetCount());

	}
	// public void InitList(int maxItems, System.Func<int, float> heightFunct = null)
	// {
	// 	//OnFill = fillAction;
	// 	heights = new Dictionary<int, float>();
	// 	positions = new Dictionary<int, float>();
	// 	int createItems = 0;
	// 	this.maxItems = maxItems;
	// 	totalHeight = spacing;
	// 	float templateheight = itemTemplate.rectTransform.rect.height;
	// 	for (int i = 0; i < maxItems; i++)
	// 	{
	// 		float thisHeight = heightFunct == null?templateheight : heightFunct(i);
	// 		totalHeight += thisHeight + spacing;
	// 		heights.Add(i, thisHeight);
	// 		positions.Add(i, -totalHeight);
	// 		if (createItems == 0 && (totalHeight > viewportHeight))
	// 			createItems = i;
	// 	}
	// 	content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
	// 	if (scrollBar != null) scrollBar.gameObject.SetActive(createItems != 0);
	// 	if (createItems == 0) createItems = maxItems;
	// 	createItems += additionalItemCount;
	// 	if (createItems > maxItems) createItems = maxItems;
	// 	viewEndIndex = createItems;
	// 	SetListSize(createItems);
	// 	topSourceIndex = 0;
	// 	bottomSourceIndex = createItems - 1;
	// 	itemIndexDict = new Dictionary<ListItem, int>();
	// 	itemSiblingIndexDict = new Dictionary<ListItem, int>();
	// 	viewEndIndex = createItems;
	// 	for (int i = 0; i < items.Count; i++)
	// 	{
	// 		items[i].rectTransform.pivot = new Vector2(0, 0);
	// 		itemIndexDict.Add(items[i], i);
	// 		itemSiblingIndexDict.Add(items[i], i);
	// 	}
	// 	topmostobject = items[0].rectTransform;
	// 	bottomobject = items[items.Count - 1].rectTransform;
	// 	UpdateFill();
	// 	UpdateScrollBar();
	// }
	bool checkPending;
	public void OnDrag(PointerEventData e)
	{
		ScrollAmount(e.delta.y);

	}
	void UpdateItem(ListItem item, int i)
	{
		int targetIndex;
		if (itemIndexDict.TryGetValue(item, out targetIndex))
		{
			items[i].rectTransform.pivot = new Vector2(0, 0);
			items[i].rectTransform.anchoredPosition = new Vector2(0, positions[targetIndex]);
			try
			{
				OnFill(targetIndex, items[i].gameObject);
			}
			catch (System.Exception e)
			{
				Debug.Log(" Tracing Exception on " + name + " " + e.Message);
			}
		}
	}

	[ExposeMethodInEditor]
	public void UpdateFill()
	{
		if (items != null)
			for (int i = 0; i < items.Count; i++)
				UpdateItem(items[i], i);
	}
	public bool TopToBottomCondition()
	{
		if (viewEndIndex == maxItems)
			return false;
		return (content.anchoredPosition.y + topmostobject.anchoredPosition.y) > swapMargin;
	}
	public bool BottomToTopCondition()
	{
		if (viewStartIndex == 0)
			return false;
		var limit = content.anchoredPosition.y + _lastViewportHeight;
		var actual = -bottomobject.anchoredPosition.y;
		return actual - limit > swapMargin;
	}

	int antiFreeze = 20;
	void CheckTopToBottomSwaps()
	{
		int counter = 0;
		if (TopToBottomCondition())
			while (TopToBottomCondition())
			{
				if (counter > antiFreeze)
				{
					Debug.Log(" counter Exceeded");
					return;
				}

				ShiftTopToBottom();
				counter++;
			}

	}
	void CheckBottomToTopSwaps()
	{
		int counter = 0;
		if (BottomToTopCondition())
			while (BottomToTopCondition() && counter < antiFreeze)
			{
				if (counter > antiFreeze)
				{
					Debug.Log(" counter Exceeded");
					return;
				}
				ShiftBottomToTop();
				counter++;
			}
	}
	void RepositionItems()
	{
		// Debug.Log("swappnig");
		if (lastScrollAmt > 0)
			CheckTopToBottomSwaps();
		else
			CheckBottomToTopSwaps();
		checkPending = false;
		lastScrollAmt = 0;
	}

	[ExposeMethodInEditor]
	void ShiftTopToBottom()
	{
		var movedItem = items[topSourceIndex];
		int lastIndex = topSourceIndex;
		if (!itemIndexDict.ContainsKey(movedItem))
		{
			Debug.Log("invalid index " + movedItem.name);
		}
		int currentIndex;
		if (itemIndexDict.TryGetValue(movedItem, out currentIndex))
		{
			currentIndex += items.Count;
			if (currentIndex < maxItems)
			{
				bottomSourceIndex = topSourceIndex;
				topSourceIndex++;
				if (topSourceIndex >= items.Count) topSourceIndex = 0;
				topmostobject = items[topSourceIndex].rectTransform;
				bottomobject = items[bottomSourceIndex].rectTransform;
				itemIndexDict[movedItem] = currentIndex;
				viewEndIndex++;
				viewStartIndex++;
				//			OnFill(currentIndex, movedItem.gameObject);
				UpdateItem(movedItem, lastIndex);
			}
		}
	}
	void OnTransformChanged()
	{
		Debug.Log("changed");
		//	_lastViewportHeight = rectTransform.rect.height;
	}

	[ExposeMethodInEditor]
	void ShiftBottomToTop()
	{
		if (viewStartIndex == 0) return;
		int lastIndex = bottomSourceIndex;
		var movedItem = items[bottomSourceIndex];
		bottomSourceIndex--;
		if (bottomSourceIndex < 0) bottomSourceIndex = items.Count - 1;
		int currentIndex = itemIndexDict[movedItem] - items.Count;
		topmostobject = items[topSourceIndex].rectTransform;
		bottomobject = items[bottomSourceIndex].rectTransform;
		if (currentIndex >= 0)
		{

			topSourceIndex--;
			if (topSourceIndex < 0) topSourceIndex = items.Count - 1;
			itemIndexDict[movedItem] = currentIndex;
			OnFill(currentIndex, movedItem.gameObject);
			viewEndIndex--;
			viewStartIndex--;
			UpdateItem(movedItem, lastIndex);
		}
	}
	Queue<float> positionQueue = new Queue<float>();
	Queue<float> times = new Queue<float>();
	int sampleCount = 3; // three frames
	bool ignoreScrollbar;
	public bool liveUpdateOnAll;
	void Reset()
	{
		var sr = GetComponentInChildren<ScrollRect>();
		if (sr != null)
		{
			content = sr.content;
#if UNITY_EDITOR
			UnityEditor.Undo.DestroyObjectImmediate(sr);
#endif
		}
		itemTemplate = GetComponentInChildren<ListItem>();

	}
	void OnScrollbar(float f)
	{
		if (ignoreScrollbar) return;
		contentVerticalPosition = (1 - f) * totalHeight;
		ScrollAmount(0);
	}
	void UpdateScrollBar()
	{
		if (scrollBar != null)
		{
			ignoreScrollbar = true;
			scrollBar.size = _lastViewportHeight / totalHeight;
			scrollBar.value = 1 - (content.anchoredPosition.y) / (totalHeight - _lastViewportHeight);
			ignoreScrollbar = false;
		}
		//RepositionItems();
	}
	public float contentVerticalPosition
	{
		get { return content.anchoredPosition.y; }
		set
		{
			var currentpos = content.anchoredPosition;
			currentpos.y = value;
			if (currentpos.y < 0) currentpos.y = 0;
			float max = totalHeight - rectTransform.rect.height;
			if (currentpos.y > max) currentpos.y = max;
			content.anchoredPosition = currentpos;
		}
	}
	float scrollAcumulator;
	void ApplyScroll()
	{
		Vector2 currentpos = content.anchoredPosition;
		if (totalHeight < _lastViewportHeight)
		{
			currentpos.y = 0;
			content.anchoredPosition = currentpos;
			return;
		}
		// currentpos.y = currentpos.y + amt;
		contentVerticalPosition += scrollAcumulator;
		lastScrollAmt = scrollAcumulator;
		scrollDirty = true;
		scrollAcumulator = 0;
		RepositionItems();
	}
	void ScrollAmount(float amt)
	{
		//if ()
		scrollAcumulator += amt;
	}
	bool scrollDirty;
	bool isDragging;

	public void OnBeginDrag(PointerEventData e)
	{
		dragBeginPos = content.anchoredPosition;
		isRunning = false;
		isDragging = true;
		velocity = 0;
		positionQueue.Clear();
		times.Clear();
	}
	public void OnEndDrag(PointerEventData e)
	{
		isDragging = false;
	}
	public bool UpdateScrollBar1;
	public bool UpdateScrollBar2;

	public bool RepositionItems1;
	public bool RepositionItems2;
	void Update()
	{

		//}}
		RepositionItems();
		if (isHovered && Input.GetMouseButtonDown(0)) velocity = 0;
		if (isDragging)
		{
			float currentPos = content.anchoredPosition.y;
			positionQueue.Enqueue(currentPos);
			float thisTime = Time.time;
			times.Enqueue(thisTime);
			if (positionQueue.Count > sampleCount)
			{
				float oldpos = positionQueue.Dequeue();
				float oldTime = times.Dequeue();
				float delta = currentPos - oldpos;
				float timedelta = thisTime - oldTime;
				if (timedelta == 0)
				{
					Debug.Log("note");
					return;
				}
				velocity = delta / timedelta;
			}
		}
		else
		if (velocity > minVelocity || velocity < -minVelocity)
		{
			ScrollAmount(velocity * Time.deltaTime);
			velocity = velocity * (1 - Time.deltaTime * veloDamping);
		}
		if (isHovered)
		{
			var scroll = Input.mouseScrollDelta.y;
			if (scroll != 0)
			{
				ScrollAmount(scroll * scrollSpeed);
				velocity = scroll * scrollSpeed * 10;
			}
		}
		// if (scrollDirty)
		// {
		// 	scrollDirty = false;

		// }
		// if (RepositionItems2)
		// 	RepositionItems();
		// if (UpdateScrollBar2)
		// 	UpdateScrollBar();
		if (scrollAcumulator != 0)
		{
			UpdateScrollBar();
			ApplyScroll();
			//	if (UpdateScrollBar1)

			//	if (RepositionItems1)
			//		RepositionItems();
		};
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isHovered = true;

	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isHovered = false;

	}
#pragma warning restore 414
}