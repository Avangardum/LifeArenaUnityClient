using System;
using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;

namespace Avangardum.LifeArena.UnityClient.Views
{
    public class FieldView : MonoBehaviour, IFieldView
    {
        private const float MaxZoom = 2f;
        private const float MinZoom = 0.1f;
        private const float MinZoomToShowBorder = 0.4f;
        private const float CellSize = 50;

        // When above 1, zoom will change faster when close to the max value.
        private const float SmoothZoomFactor = 2f;
        
        [SerializeField] private GameObject _cellViewPrefab;
        
        private CellView[,] _cells;
        
        public event EventHandler<CellClickedEventArgs> CellClicked;
        public event EventHandler<ZoomChangedEventArgs> ZoomChanged;

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
                    ResetPositionAndZoom();
                }
                ColorCells(value);
            }
        }

        private float Zoom
        {
            get => transform.localScale.x;
            set
            {
                if (value == Zoom) return;
                
                // Focus point is the position of the mouse(in case of scrolling) / center of the screen (in case of
                // using zoom slider) in the coordinate system of the field if zoom is 1.
                // After zooming, the focus point should remain the same, so that the mouse is still over the same cell.
                // Focus is the above mentioned object (mouse / center of the screen)
                var focusPosition = ZoomFocusPointMode switch
                {
                    ZoomFocusPointMode.Mouse => (Vector2)Input.mousePosition,
                    ZoomFocusPointMode.ScreenCenter => new Vector2(Screen.width / 2f, Screen.height / 2f),
                    _ => throw new ArgumentOutOfRangeException()
                };
                var focusPoint = ( focusPosition - (Vector2)transform.position ) / Zoom;
                
                var newZoom = Mathf.Clamp(value, MinZoom, MaxZoom);
                transform.localScale = new Vector3(newZoom, newZoom, 1f);
                
                // What position the focus should have in the coordinate system of the field after zooming.
                var newRelativeFocusPosition = focusPoint * newZoom;
                
                var newPosition = focusPosition - newRelativeFocusPosition;
                transform.position = newPosition;

                var isBorderVisible = newZoom >= MinZoomToShowBorder;
                foreach (var cell in _cells)
                {
                    cell.IsBorderVisible = isBorderVisible;
                }
                
                ZoomChanged?.Invoke(this, new ZoomChangedEventArgs(Zoom, ZoomPercentage));
            }
        }

        public float ZoomPercentage
        {
            get => ZoomToZoomPercentage(Zoom);
            set
            {
                var clampedValue = Mathf.Clamp(value, 0, 1);
                if (clampedValue == ZoomPercentage) return;
                Zoom = ZoomPercentageToZoom(clampedValue);
            }
        }

        public ZoomFocusPointMode ZoomFocusPointMode { private get; set; }

        public void Move(Vector2 movement)
        {
            transform.Translate(movement, Space.World);
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
                    cell.transform.localPosition = new Vector3(x, y) * CellSize;
                    var pinnedX = x;
                    var pinnedY = y;
                    cell.Clicked += (_, _) => OnCellClicked(pinnedX, pinnedY);
                    _cells[x, y] = cell;
                }
            }
        }

        private void OnCellClicked(int x, int y)
        {
            CellClicked?.Invoke(this, new CellClickedEventArgs(x, y));
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
        
        private void ResetPositionAndZoom()
        {
            Zoom = MinZoom;
            
            // Middle cell should be in the middle of the screen.
            // Zero cell position is equal to field position
            var middleCellPosition = new Vector2(Screen.width, Screen.height) / 2;
            var middleCellIndex = new Vector2Int(_cells.GetLength(0) / 2, _cells.GetLength(1) / 2);
            var zeroCellPosition = middleCellPosition - (Vector2)middleCellIndex * (CellSize * Zoom);
            transform.position = zeroCellPosition;
        }

        private float ZoomPercentageToZoom(float zoomPercentage)
        {
            Assert.IsTrue(zoomPercentage is >= 0 and <= 1);
            return MinZoom + Mathf.Pow(zoomPercentage, SmoothZoomFactor) * (MaxZoom - MinZoom);
        }
        
        private float ZoomToZoomPercentage(float zoom)
        {
            Assert.IsTrue(zoom is >= MinZoom and <= MaxZoom);
            return Mathf.Pow((zoom - MinZoom) / (MaxZoom - MinZoom), 1 / SmoothZoomFactor);
        }
    }
}