using System.Collections.Generic;
using UnityEngine;

namespace Clovers.MinJamElectric
{
    public class SolderRenderer : MonoBehaviour
    {
        [SerializeField] private ElectricBoard _board;
        [SerializeField] private GameObject _solderPrefab;

        private List<List<GameObject>> _solderInstances = new();

        private void Awake()
        {
            _board.OnChange += Render;
        }

        private void Start()
        {
            CreateGrid();
            Render();
        }

        private void CreateGrid()
        {
            foreach (var instanceRow in _solderInstances)
            {
                foreach (var instance in instanceRow)
                {
                    Destroy(instance);
                }
            }
            _solderInstances.Clear();

            for (var i = 0; i < _board.HeightCells; i++)
            {
                _solderInstances.Add(new());
                for (var j = 0; j < _board.WidthCells; j++)
                {
                    var instance = Instantiate(_solderPrefab, _board.transform);
                    instance.transform.localPosition = new Vector3(ElectricBoard.GRID_SIZE * j, ElectricBoard.GRID_SIZE * i);
                    instance.SetActive(_board.Points[j][i] == 1);
                    _solderInstances[i].Add(instance);
                }
            }
        }

        private void Render()
        {
            for (var i = 0; i < _board.HeightCells; i++)
            {
                for (var j = 0; j < _board.WidthCells; j++)
                {
                    _solderInstances[i][j].SetActive(_board.Points[i][j] == 1);
                }
            }
        }
    }
}
