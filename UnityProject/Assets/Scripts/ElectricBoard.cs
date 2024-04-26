using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace Clovers.MinJamElectric
{
    public class ElectricBoard : MonoBehaviour
    {
        public const int GRID_SIZE = 16;

        [SerializeField] private RectTransform _uiElement;

        public IReadOnlyList<IReadOnlyList<int>> Points => _points;
        public int WidthCells => (int)_uiElement.sizeDelta.x / GRID_SIZE;
        public int HeightCells => (int)_uiElement.sizeDelta.y / GRID_SIZE;

        private readonly List<List<int>> _points = new();

        public Action OnChange;

        public bool IsCellWithinBounds(Vector2Int cell)
        {
            return cell.x >= 0 && cell.y >= 0 && cell.x < WidthCells && cell.y < HeightCells;
        }

        public bool RaycastCell(Vector3 screenPosition, out Vector2Int cell)
        {
            Vector2 localPosition;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiElement, screenPosition, Camera.main, out localPosition))
            {
                cell = default;
                return false;
            }
            cell = new Vector2Int(Mathf.FloorToInt(localPosition.x / GRID_SIZE), Mathf.FloorToInt(localPosition.y / GRID_SIZE));

            return IsCellWithinBounds(cell);
        }

        private void Awake()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _points.Clear();

            for (var i = 0; i < HeightCells; i++)
            {
                _points.Add(new());
                for (var j = 0; j < WidthCells; j++)
                {
                    _points[i].Add(0);
                }
            }
        }

        public bool Set(Vector2Int cell, int value)
        {
            if(!IsCellWithinBounds(cell)) return false;

            var isDirty = _points[cell.y][cell.x] != value;
            _points[cell.y][cell.x] = value;

            if(!isDirty) return false;

            OnChange?.Invoke();
            return true;
        }

        private void PrintDebugInfo()
        {
            var str = "";
            for (var i = 0; i < WidthCells; i++)
            {
                for (var j = 0; j < HeightCells; j++)
                {
                    str += _points[i][j];
                }
                str += '\n';
            }
            Debug.Log(str);
        }
    }
}
