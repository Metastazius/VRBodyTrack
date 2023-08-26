using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AvatarController : MonoBehaviour
{
    private Animator animator;
    public Transform[] bones;
    private List<Vector3> position = new List<Vector3>();
    public MediapipeRTStream data;
    private string[] lines;
    public Nodes nodes;
    // Start is called before the first frame update
    void Start()
    {
        lines = data.lines;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        position = nodes.nodes.Select(p=>p.transform.position).ToList();
        bones[0].position = position[12];
        bones[1].position = position[11];
        bones[2].position = position[24];
        bones[3].position = position[23];
             
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            Debug.Log("S");
            Vector3 side1;
            Vector3 side2;

            // Set the left arm IK
         
            // Set the right arm IK

            // Set the left hand IK
            Vector3 pointLH = ((position[19] + position[17]) / 2.0f);
            Vector3 fdDirectionLH = (pointLH - position[15]).normalized;
            side1 = position[19] - position[15];
            side2 = position[17] - position[15];
            Vector3 upDirectionLH = Vector3.Cross(side2, side1).normalized;

            Quaternion rotationLH = Quaternion.LookRotation(fdDirectionLH, upDirectionLH);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.5f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, position[15]);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, rotationLH);


            // Set the right hand IK
            Vector3 pointRH = ((position[20] + position[18]) / 2.0f);
            Vector3 fdDirectionRH = (pointRH - position[16]).normalized;
            side1 = position[20] - position[16];
            side2 = position[18] - position[16];
            Vector3 upDirectionRH = Vector3.Cross(side1, side2).normalized;

            Quaternion rotationRH = Quaternion.LookRotation(fdDirectionRH, upDirectionRH);

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, position[16]);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rotationRH);

            // Set the left foot IK
            Vector3 fdDirectionLF = (position[31] - position[29]).normalized;
            Vector3 upDirectionLF = (position[25] - position[27]).normalized;
            Quaternion rotationLF = Quaternion.LookRotation(fdDirectionLF, upDirectionLF);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.5f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.5f);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, position[27]);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, rotationLF);

            // Set the right foot IK
            Vector3 fdDirectionRF = (position[32] - position[30]).normalized;
            Vector3 upDirectionRF = (position[26] - position[28]).normalized;
            Quaternion rotationRF = Quaternion.LookRotation(fdDirectionRF, upDirectionRF);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.5f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.5f);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, position[28]);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rotationRF);

            // Set the head IK
            animator.SetLookAtWeight(1.0f);
            animator.SetLookAtPosition(position[0]);

            // Set the body  
            side1 = position[11] - position[12];
            side2 = position[23] - position[12];

            Vector3 upDirection = Vector3.Cross(side1, side2).normalized;
            Vector3 bustPosition = Vector3.Lerp((position[23] + position[24]) / 2.0f, (position[11] + position[12]) / 2.0f, 0.5f);
            Vector3 upFromCenterPoint = bustPosition + upDirection;
            Vector3 upDirectionFromCenter = (upFromCenterPoint - bustPosition).normalized;
            Vector3 CenterToHead = (((position[11] + position[12]) / 2.0f) - bustPosition).normalized;

            Quaternion desiredRotation = Quaternion.LookRotation(upDirectionFromCenter, CenterToHead);
            Vector3 spineOffset = animator.GetBoneTransform(HumanBodyBones.Spine).position - animator.transform.position;

            animator.transform.position = bustPosition - spineOffset;
            animator.transform.rotation = desiredRotation;

        }
    }
}
