using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    public float motorSpeed = 60f;
    public float motorForce = 200f;
    public float angleThreshold = 40f;

    private HingeJoint hinge;
    private JointMotor motor;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();

        motor = hinge.motor;
        motor.force = motorForce;
        motor.targetVelocity = motorSpeed;

        hinge.motor = motor;
        hinge.useMotor = true;
    }

    void FixedUpdate()
    {
        float angle = hinge.angle;

        // If close to right limit → swing left
        if (angle > angleThreshold)
        {
            motor.targetVelocity = -motorSpeed;
            hinge.motor = motor;
        }
        // If close to left limit → swing right
        else if (angle < -angleThreshold)
        {
            motor.targetVelocity = motorSpeed;
            hinge.motor = motor;
        }
    }
}
