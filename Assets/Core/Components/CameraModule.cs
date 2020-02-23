using System;
using System.Collections;
using UnityEngine;

namespace SSAction.Core.Characters
{
    public class CameraModule : MonoBehaviour
    {
        private const float FixedHeight = -0.678f;

        public Camera mainCamera = null;
        public CharacterModule character = null;
        public float speed = .1f;
        public float distance = 1f;

        private float lerpTime = .8f;
        private IEnumerator iUpdatePosition = null;

        private void Awake()
        {
            iUpdatePosition = UpdateCameraPosition();
        }

        private void LateUpdate()
        {
            if (character == null) return;
            if (character.IsMove) lerpTime = .8f;

            iUpdatePosition.MoveNext();

            if (mainCamera.transform.position.x <= 0)
            {
                mainCamera.transform.position =
                    new Vector3(0,
                    mainCamera.transform.position.y,
                    mainCamera.transform.position.z);
            }
        }

        private IEnumerator UpdateCameraPosition()
        {
            while (true)
            {
                Vector3 pos = character.transform.position;

                if (lerpTime >= 1f)
                {
                    mainCamera.transform.position =
                        new Vector3(pos.x + distance, pos.y - FixedHeight, pos.z);
                }
                else
                {
                    mainCamera.transform.position =
                        Vector3.Lerp(
                            pos,
                            new Vector3(pos.x + distance, pos.y - FixedHeight, pos.z),
                            lerpTime);

                    lerpTime += Time.deltaTime;
                }

                yield return null;
            }
        }

    }
}
