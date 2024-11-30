using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.VRehab.Hands.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class AnimateHandOnInput : MonoBehaviour
    {
        public InputActionProperty triggerAnimationAction;
        public InputActionProperty gripAnimationAction;

        private Animator handAnimator;
        private float gripValue;
        private float triggerValue;

        private void Start()
        {
            handAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            AnimateGrip();
            AnimateTrigger();
        }

        private void AnimateGrip()
        {
            gripValue = gripAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);
        }

        private void AnimateTrigger()
        {
            triggerValue = triggerAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", triggerValue);
        }
    }
}
