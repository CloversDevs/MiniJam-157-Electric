using UnityEngine;

namespace Clovers.MinJamElectric
{
    public class ToolBelt : MonoBehaviour
    {
        public int CurrentTool = 1;

        [SerializeField] private ElectricBoard _board;
        private void Update()
        {
            if (!Input.GetMouseButton(0)) return;
            if (!_board.RaycastCell(Input.mousePosition, out var locationInt)) return;
            var didApplyTool = _board.Set(locationInt, CurrentTool);

            if (!didApplyTool) return;
            Debug.Log($"Painted {locationInt}: {CurrentTool}");
        }
    }
}
