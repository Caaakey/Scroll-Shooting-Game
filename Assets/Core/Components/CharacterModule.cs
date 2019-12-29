using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SSAction.Core.Characters
{
    using Interfaces;
    public class CharacterModule : MonoBehaviour, IAnimationStatus
    {
        [SerializeField] private Animator animator = null;

        private bool isLeft = false;
        private bool isReverse = false;

        public AnimStatus Status { get; set; } = AnimStatus.Idle;

        public void ChangeStatus(AnimStatus status, bool isSet)
        {
            switch (status)
            {
                case AnimStatus.Idle:
                    animator.SetBool(0, false);
                    break;
            }

            Status = status;
            animator.SetBool(0, isSet);
        }

        private void Awake()
        {

        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!isLeft)
                {
                    isReverse = true;
                    isLeft = true;
                }

                ChangeStatus(AnimStatus.Run, true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (isLeft)
                {
                    isReverse = true;
                    isLeft = false;
                }

                ChangeStatus(AnimStatus.Run, true);
            }
            else
            {
                ChangeStatus(AnimStatus.Idle, true);
            }
        }

        private void FixedUpdate()
        {
            switch (Status)
            {
                case AnimStatus.Run:
                    if (isReverse)
                    {
                        transform.localRotation = isLeft ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                        isReverse = false;
                    }

                    transform.Translate(1f * Time.fixedDeltaTime, 0, 0);
                    break;

                default: break;
            }
        }
    }

}
