using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.VRehab.Scripts
{
    public class PanelManager : MonoBehaviour
    {
        private Animator _animator;
        private CanvasGroup _canvasGroup;
        private bool _isVisible = false;

        private const string InvisibleStateName = "Invisible";
        private const string HiddenStateName = "Hidden";

        public GameObject Panel;
        [SerializeField] private List<PanelManager> panelsToReset;

        private void Start()
        {
            _animator = Panel.GetComponent<Animator>();
            _canvasGroup = Panel.GetComponent<CanvasGroup>();


            // Initialize visibility based on `isVisible`
            if (_animator != null && _canvasGroup.alpha == 0)
            {
                SetPanelVisibility(_isVisible);
            }
        }

        private void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(InvisibleStateName) &&
                _animator.GetNextAnimatorStateInfo(0).IsName(HiddenStateName))
            {
                SetPanelVisibility(false);
            }
        }

        public void OpenPanel()
        {
            if (Panel == null || _animator == null || _isVisible) return;
            _isVisible = true;
            _animator.SetBool("IsVisible", _isVisible);
            SetPanelVisibility(_isVisible);
        }

        public void ClosePanel()
        {
            if (Panel == null || _animator == null || !_isVisible) return;
            _isVisible = false;
            _animator.SetBool("IsVisible", _isVisible);
        }

        private void SetPanelVisibility(bool visibility)
        {
            _canvasGroup.alpha = visibility ? 1 : 0;
            _canvasGroup.interactable = visibility;
            _canvasGroup.blocksRaycasts = visibility;
        }

        public void ResetPanels()
        {
            foreach (var panelManager in panelsToReset.Where(panelManager => panelManager != null))
            {
                panelManager._isVisible = false;
                panelManager._animator.SetBool("IsVisible", false);
                panelManager.SetPanelVisibility(false);
            }
        }
    }
}