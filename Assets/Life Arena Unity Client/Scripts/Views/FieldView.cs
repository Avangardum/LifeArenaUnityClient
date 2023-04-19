using System;
using System.Collections.Generic;
using System.Linq;
using Avangardum.LifeArena.UnityClient.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Avangardum.LifeArena.UnityClient.Views
{
    public class FieldView : MonoBehaviour, IFieldView
    {
        private const float MaxZoom = 2f;
        private const float MinZoom = 0.1f;
        private const float ScrollZoomSensitivity = 0.1f;
        private const float MinZoomToShowBorder = 0.4f;
        private const float CellOffset = 50;
        
        [SerializeField] private GameObject _cellViewPrefab;
        [SerializeField] private Sprite _deadCellSprite;
        [SerializeField] private Sprite _livingCellSprite;
        
        private CellView[,] _cells;
        private Vector2? _lastMousePosition;
        
        public event EventHandler<IFieldView.CellClickedEventArgs> CellClicked;

        public bool[,] LivingCells
        {
            set
            {
                var shouldRecreateCells = _cells == null || _cells.GetLength(0) != value.GetLength(0) || 
                    _cells.GetLength(1) != value.GetLength(1);
                if (shouldRecreateCells)
                {
                    ClearCells();
                    CreateCells(value.GetLength(0), value.GetLength(1));
                }
                ColorCells(value);
            }
        }
        
        private bool IsMouseOverField
        {
            get
            {
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, raycastResults);
                var isMouseOverField = 
                    !raycastResults.Any() || raycastResults.First().gameObject.CompareTag(Tags.CellView);
                return isMouseOverField;
            }
        }

        public float Zoom
        {
            private get => transform.localScale.x;
            set
            {
                // Focus point is the position of the mouse in the coordinate system of the field if zoom is 1.
                // Focus point is zero if the mouse is over the center of the field, (fieldWidth / 2, fieldHeight / 2)
                // if the mouse is over the top right corner of the field, etc.
                // After zooming, the focus point should remain the same, so that the mouse is still over the same cell.
                var focusPoint = ( (Vector2)Input.mousePosition - (Vector2)transform.position ) / Zoom;
                
                var newZoom = Mathf.Clamp(value, MinZoom, MaxZoom);
                transform.localScale = new Vector3(newZoom, newZoom, 1f);
                
                // What position the mouse should have in the coordinate system of the field after zooming.
                var newRelativeMousePosition = focusPoint * newZoom;
                
                var newPosition = (Vector2)Input.mousePosition - newRelativeMousePosition;
                transform.position = newPosition;

                var isBorderVisible = newZoom >= MinZoomToShowBorder;
                foreach (var cell in _cells)
                {
                    cell.IsBorderVisible = isBorderVisible;
                }
            }
        }

        private void ClearCells()
        {
            if (_cells == null) return;
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
            _cells = null;
        }

        private void CreateCells(int width, int height)
        {
            _cells = new CellView[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var cell = Instantiate(_cellViewPrefab, transform).GetComponent<CellView>();
                    Assert.IsNotNull(cell);
                    cell.transform.localPosition = new Vector3(x, y) * CellOffset;
                    var pinnedX = x;
                    var pinnedY = y;
                    cell.Clicked += (_, _) => OnCellClicked(pinnedX, pinnedY);
                    _cells[x, y] = cell;
                }
            }
        }

        private void OnCellClicked(int x, int y)
        {
            Debug.Log($"Cell clicked: {x}, {y}");
            CellClicked?.Invoke(this, new IFieldView.CellClickedEventArgs(x, y));
        }

        private void ColorCells(bool[,] livingCells)
        {
            for (var x = 0; x < livingCells.GetLength(0); x++)
            {
                for (var y = 0; y < livingCells.GetLength(1); y++)
                {
                    _cells[x, y].IsAlive = livingCells[x, y];
                }
            }
        }

        private void Awake()
        {
            var livingCells = new bool[100, 100];
            livingCells[1, 1] = true;
            livingCells[2, 1] = true;
            livingCells[3, 1] = true;
            livingCells[3, 2] = true;
            livingCells[2, 3] = true;
            LivingCells = livingCells;
        }

        private void Update()
        {
            ProcessFieldMovement();
            ProcessFieldZoom();

            void ProcessFieldMovement()
            {
                if (Input.GetMouseButton(0) && IsMouseOverField && _lastMousePosition is { } lastMousePosition)
                {
                    var mousePositionDelta = (Vector2)Input.mousePosition - lastMousePosition;
                    transform.Translate(mousePositionDelta);
                }

                _lastMousePosition = Input.mousePosition;
            }
            
            void ProcessFieldZoom()
            {
                var zoomDelta = Input.mouseScrollDelta.y * ScrollZoomSensitivity;
                if (zoomDelta == 0) return;
                Zoom += zoomDelta;
            }
        }
    }
}