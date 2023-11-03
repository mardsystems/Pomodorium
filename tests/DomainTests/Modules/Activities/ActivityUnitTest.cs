//using FluentAssertions;
//using Pomodorium.Modules.Pomos;

//namespace Pomodorium.Modules.Activities;

//public abstract class ActivityUnitTest : UnitTest
//{
//    public Pomodoro Timer { get; set; }

//    public Activity Activity { get; set; }

//    public abstract class OnFocusActivity : ActivityUnitTest
//    {
//        public override void Act()
//        {
//            Activity.Focus();
//        }

//        public class WithSuccess : OnFocusActivity
//        {
//            public override void Arrange()
//            {
//                Timer = new Pomodoro();

//                Activity = new Activity(Timer);
//            }

//            [Fact]
//            public void Timer_Status_Should_Be_Started()
//            {
//                Activity.Timer.Should().NotBeNull();

//                Activity.Timer.State.Should().Be(TimerState.Started);
//            }
//        }
//    }

//    public abstract class OnCompleteActivity : ActivityUnitTest
//    {
//        public override void Act()
//        {
//            Activity.Complete();
//        }

//        public class WithSuccess : OnCompleteActivity
//        {
//            public override void Arrange()
//            {
//                Timer = new Pomodoro();

//                Activity = new Activity(Timer);
//            }

//            [Fact]
//            public void Timer_Status_Should_Be_Stopped()
//            {
//                Activity.Timer.Should().NotBeNull();

//                Activity.Timer.State.Should().Be(TimerState.Stopped);
//            }
//        }
//    }
//}
