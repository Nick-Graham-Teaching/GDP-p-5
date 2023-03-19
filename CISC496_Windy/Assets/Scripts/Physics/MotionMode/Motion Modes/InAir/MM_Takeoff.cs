using UnityEngine;

namespace Windy.MotionMode {

    public class MM_Takeoff : MM_InAir
    {
        protected internal int Method;
        protected internal float flipWingsSpeed;
        protected internal float flipWingCD;

        public sealed override void Start()
        {
            float force = Method == 0b001 ? flipWingsSpeed :
                          Method == 0b100 ? flipWingsSpeed / 3.0f * 2.0f : 0.0f;
            FlyInertia = force * Vector3.up;
            try
            {
                EnergySystem.EnergySys.Instance.OnTakeOff(Method);
                MM_Executor.Instance.StartEnergySupervisor(flipWingCD);
                UI.UIEvents.OnToTakeoffMode?.Invoke();
            } 
            catch (TakeOffException)
            {
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Walk, MM_Executor.Instance.B_S_Walk);
            }
        }

        public sealed override void Update()
        {
            rb.drag = flyDrag;
        }
        
        public override string ToString() => "MotionMode -- Takeoff";

    }
}