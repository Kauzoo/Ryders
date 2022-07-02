using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.ExtremeGear.Movement
{
    /// <summary>
    /// Contains Methods for both Gravity and Grounded
    /// If you are implementing ExtremeGear that uses different Ground/Gravity Behaviour,
    /// derive from this class
    /// </summary>
    public abstract class GravityPack : MonoBehaviour
    {
        // TODO: Implement Gravity Pack
        public static bool Grounded(Transform playerTransform, float maxGroundDistance, int groundLayerMask)
        {
            int layerMask = 1 << groundLayerMask;
            if (Physics.Raycast(playerTransform.position, playerTransform.up * (-1), out RaycastHit hit, maxGroundDistance, layerMask))
            {
                //if (//HasGroundChanged(hit.transform.gameObject))
                {
                    //HandleGroundChanged(hit.transform.gameObject);
                    //NewGroundChanged(hit.transform.gameObject);
                }
                //CorrectGroundDistance(hit.distance);
                return true;
            }
            /*
            else if (Physics.Raycast(playerTransform.position, Vector3.down, out RaycastHit hitAlt, grounded.maxDistance, layerMask))
            {
                {
                    if (HasGroundChanged(hitAlt.transform.gameObject))
                    {
                        // HandleGroundChanged(hitAlt.transform.gameObject);
                        NewGroundChanged(hitAlt.transform.gameObject);
                    }
                    CorrectGroundDistance(hitAlt.distance);
                    movement.grounded = true;
                }
            }
            */
            return false;
        }

        public virtual bool Grounded()
        {
            throw new NotImplementedException();
        }

        /*
        private void CorrectGroundDistance(float distance)
        {
            if (distance < grounded.maxDistance - grounded.tollerance)
            {
                Debug.Log("Code is reached");
                playerRigidbody.isKinematic = true;

                playerTransform.Translate(groundInfo.currentGround.transform.up * ((grounded.maxDistance - grounded.tollerance * 0.5f) - distance));
                Debug.Log("Angle less than 90");

                //playerRigidbody.MovePosition(playerTransform.up * (grounded.maxDistance - distance));
                playerRigidbody.isKinematic = false;
            }
        }*/

        private int Gravity(bool grounded)
        {
            if (grounded)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private bool HasGroundChanged(GameObject newGround, GameObject currentGround, Transform playerTransform)
        {
            if (currentGround == null)
            {
                currentGround = playerTransform.gameObject;
            }
            if (!currentGround.Equals(newGround))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles Ground allignment
        /// WIP: Make it work with ground that is not world axis alligned
        /// </summary>
        /// <param name="newGround"></param>
        private void HandleGroundChanged(GameObject newGround, GameObject previousGround, GameObject currentGround, Transform playerTransform)
        {
            Debug.Log("HandleGroundChanged is being executed");
            previousGround = currentGround;
            currentGround = newGround;

            float currentRotationAngle = playerTransform.rotation.eulerAngles.y; //+ groundInfo.previousGround.transform.rotation.eulerAngles.y;
            Vector3 currentRotationVector = new Vector3(0, currentRotationAngle, 0);

            // Alligns player to the new ground
            Quaternion targetRotation = Quaternion.LookRotation(currentGround.transform.forward, currentGround.transform.up);
            playerTransform.rotation = targetRotation;

            //playerTransform.rotation = Quaternion.LookRotation(groundInfo.previousGround.transform.forward, groundInfo.currentGround.transform.up);
            playerTransform.Rotate(currentRotationVector, Space.Self);
        }

        private void NewGroundChanged(GameObject newGround, GameObject previousGround, GameObject currentGround, Transform playerTransform)
        {
            previousGround = currentGround;
            currentGround = newGround;

            var localSpacePlayerForward = previousGround.transform.InverseTransformDirection(playerTransform.forward);

            //Quaternion targetRotation = Quaternion.LookRotation(groundInfo.currentGround.transform.forward, groundInfo.currentGround.transform.up);
            //playerTransform.rotation = targetRotation;

            var worldSpaceForward = currentGround.transform.TransformDirection(localSpacePlayerForward);
            playerTransform.rotation = Quaternion.LookRotation(worldSpaceForward, currentGround.transform.up);
        }

        /*
        private void ConstantGroundAllignment()
        {
            if (groundInfo.currentGround != null)
                playerTransform.rotation = Quaternion.LookRotation(playerTransform.forward, groundInfo.currentGround.transform.up);
        }*/

        /*
        private bool TryGetGroundObject(out GameObject groundObject)
        {
            int layerMask = 1 << grounded.layerMask;
            if (Physics.Raycast(visualPlayerTransform.position, playerTransform.up * (-1), out RaycastHit hit, grounded.maxDistance, layerMask))
            {
                groundObject = hit.transform.gameObject;
                return true;
            }
            groundObject = null;
            return false;
        }*/
    }
}
