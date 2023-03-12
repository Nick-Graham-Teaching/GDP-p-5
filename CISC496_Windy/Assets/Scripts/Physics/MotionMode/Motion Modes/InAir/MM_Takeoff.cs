using UnityEngine;

namespace Windy.MotionMode {

    public class MM_Takeoff : MM_InAir
    {
        protected internal int Method;
        protected internal float flipWingsSpeed;
        protected internal float flyDrag;

        public override void Start()
        {
            float force = Method == 0b001 ? flipWingsSpeed :
                          Method == 0b100 ? flipWingsSpeed / 3.0f * 2.0f : 0.0f;
            FlyInertia = force * Vector3.up;
            try
            {
                EnergySys.Instance.OnTakeOff(Method);
                
                MM_Executor.Instance.EnergyComsumptionSupervisor =
                                MM_Executor.Instance.StartCoroutine(MM_Executor.Instance.EnergyConsumptionSupervisor());
            } 
            catch (MyUtility.TakeOffException)
            {
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Walk, MM_Executor.Instance.B_S_Walk);
            }
        }

        public override void Update()
        {
            rb.drag = flyDrag;
        }
        public override void FixedUpdate()
        {
            if (FlyInertia != Vector3.zero)
            {
                rb.AddForce(FlyInertia, ForceMode.VelocityChange);
                FlyInertia = Vector3.zero;
            }
        }

        public override string ToString() => "MotionMode -- Takeoff";

    }
}
