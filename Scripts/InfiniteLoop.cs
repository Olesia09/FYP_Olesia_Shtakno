using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.VRehab.Scripts
{
    public class InfiniteLoop : MonoBehaviour
    {
        public ScrollRect ScrollRect;
        public RectTransform ViewPortTransform;
        public RectTransform ContentPanelTransform;
        public HorizontalLayoutGroup HLG;

        public RectTransform[] ItemList;


        private Vector2 _oldVelocity;
        private bool _isUpdated;
        // Start is called before the first frame update
        void Start()
        {
            _isUpdated = false;
            _oldVelocity = Vector2.zero;

            var itemsToAdd = Mathf.CeilToInt(ViewPortTransform.rect.width / ItemList[0].rect.width + HLG.spacing);

            for (var i = 0; i < itemsToAdd; i++)
            {
                var rt = Instantiate(ItemList[i % ItemList.Length], ContentPanelTransform);
                rt.SetAsLastSibling();
            }

            for (var i = 0; i < itemsToAdd; i++)
            {
                var num = ItemList.Length -i -1;

                while (num < 0)
                {
                    num += ItemList.Length;
                }

                var rt = Instantiate(ItemList[num], ContentPanelTransform);
                rt.SetAsFirstSibling();
            }
            
            ContentPanelTransform.localPosition = new Vector3(0 - (ItemList[0].rect.width + HLG.spacing) * itemsToAdd,
                ContentPanelTransform.localPosition.y,
                    ContentPanelTransform.localPosition.z);
        }

        void Update()
        {
            if (_isUpdated)
            {
                _isUpdated = false;
                ScrollRect.velocity = _oldVelocity;
            }

            if (ContentPanelTransform.localPosition.x > 0)
            {
                Canvas.ForceUpdateCanvases();
                _oldVelocity = ScrollRect.velocity;
                ContentPanelTransform.localPosition -= new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
                _isUpdated = true;
            }

            if (ContentPanelTransform.localPosition.x < 0 - (ItemList.Length * (ItemList[0].rect.width + HLG.spacing)))
            {
                Canvas.ForceUpdateCanvases();
                _oldVelocity = ScrollRect.velocity;
                ContentPanelTransform.localPosition += new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
                _isUpdated = true;
            }
        }

    }
}
