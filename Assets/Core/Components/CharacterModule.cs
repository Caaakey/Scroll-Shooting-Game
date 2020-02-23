using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SSAction.Core.Characters
{
    public class CharacterModule : MonoBehaviour
    {
        [Flags]
        public enum StatusFlag
        {
            isLeft      = 1,
            isReverse   = 2,
            isMove      = 4,
            isJump      = 8
        }

        private const string RUN = "Run";
        private const string JUMP = "Jump";
        private const string JUMP_DOWN = "JumpDown";

        [SerializeField] private Animator animator = null;
        [SerializeField] private Rigidbody2D rootRigid = null;
        [SerializeField] private Transform bottomPosition = null;
        [SerializeField] private CircleCollider2D bottomCollider = null;

        public float jumpPower = 4.5f;
        public float moveSpeed = 1f;
        [NonSerialized] public StatusFlag status = 0;

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!BitUtility.IsSet(status, StatusFlag.isLeft))
                {
                    BitUtility.Set(ref status, StatusFlag.isLeft);
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }

                if (!BitUtility.IsSet(status, StatusFlag.isJump))
                    animator.SetBool(RUN, true);

                BitUtility.Set(ref status, StatusFlag.isMove);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (BitUtility.IsSet(status, StatusFlag.isLeft))
                {
                    BitUtility.UnSet(ref status, StatusFlag.isLeft);
                    transform.localRotation = Quaternion.identity;
                }

                if (!BitUtility.IsSet(status, StatusFlag.isJump))
                    animator.SetBool(RUN, true);

                BitUtility.Set(ref status, StatusFlag.isMove);
            }
            else
            {
                animator.SetBool(RUN, false);
                BitUtility.UnSet(ref status, StatusFlag.isMove);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (BitUtility.IsSet(status, StatusFlag.isJump)) return;

                animator.SetBool(RUN, false);
                animator.SetBool(JUMP, true);
                transform.Translate(new Vector3(0, -bottomPosition.localPosition.y * .5f, 0));

                rootRigid.velocity = Vector2.zero;
                rootRigid.AddForce(new Vector2(0, jumpPower));

                BitUtility.Set(ref status, StatusFlag.isJump);
                bottomCollider.enabled = true;
            }

            if (BitUtility.IsSet(status, StatusFlag.isMove) &&
                    rootRigid.velocity.y < -Mathf.Epsilon)
            {
                if (!animator.GetBool(JUMP))
                {
                    animator.SetBool(RUN, false);
                    animator.SetBool(JUMP, true);
                    animator.SetTrigger(JUMP_DOWN);

                    BitUtility.Set(ref status, StatusFlag.isJump);
                    bottomCollider.enabled = true;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer != 9) return;
            if (rootRigid.velocity.y <= 0)
            {
                animator.SetBool(JUMP, false);
                BitUtility.UnSet(ref status, StatusFlag.isJump);
            }

            if (rootRigid.velocity.y == 0)
                bottomCollider.enabled = false;
        }

        private void FixedUpdate()
        {
            if (BitUtility.IsSet(status, StatusFlag.isMove))
            {
                transform.Translate(moveSpeed * Time.fixedDeltaTime, 0, 0);
            }
        }

    }

}
