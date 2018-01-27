using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFiltre : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField]
    public int _count;
    [SerializeField]
    public GameObject _filtrePrefab;
    [SerializeField]
    public Text _lblCount;

    public GameObject _blockGO;
    private Block _block; 
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        _lblCount.text = _count.ToString();
    }

    public void increment()
    {
        _count++;
        _lblCount.text = _count.ToString();
    }

    public void decrement()
    {
        _count--;
        _lblCount.text = _count.ToString();
    }

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        _blockGO = Instantiate(_filtrePrefab, transform.parent.parent.GetChild(0));
        _blockGO.transform.position = eventData.position;
        _block = _blockGO.GetComponent<Block>(); 
        startPosition = _blockGO.transform.position;
        startParent = _blockGO.transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        _blockGO.transform.position = eventData.position;
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        //_block = null;
        if (_count > 1)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        Vector2 blockPos = _blockGO.GetComponent<RectTransform>().anchoredPosition; 
        if (blockPos.x > 451f && blockPos.x < 2349f && blockPos.y > (-900f + 100f * _blockGO.GetComponent<Block>().size) && blockPos.y < 0f)
        {
            int cellX = (int) Mathf.Round(blockPos.x / 100f);
            int cellY = (int) Mathf.Round(blockPos.y / 100f);
            _block.colomn = cellX;
            _block.row = -cellY;
            _blockGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f * cellX, 100f * cellY);
            _block._button = gameObject; 
            decrement();
            _block.TableUpdate(); 
        } else
        {
            Destroy(_blockGO); 
        }
        /*if (_block.transform.parent == startParent)
        {
            _block.transform.position = startPosition;
        }*/
    }
    #endregion
}

