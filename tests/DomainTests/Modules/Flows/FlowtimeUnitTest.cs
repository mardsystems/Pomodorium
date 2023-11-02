using FluentAssertions;

namespace Pomodorium.Modules.Flows;

public abstract class FlowtimeUnitTest : UnitTest
{
    public Guid Id { get; set; }

    public Task Task { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime StopDateTime { get; set; }

    public Flowtime Flowtime { get; set; } = default!;

    public abstract class OnCreateNewFlowtime : FlowtimeUnitTest
    {
        public override void Act()
        {
            Flowtime = new Flowtime(Task);
        }

        public class WithSuccess : OnCreateNewFlowtime
        {
            public override void Arrange()
            {
                Id = Guid.NewGuid();

                Task = new Task("Test");
            }

            [Fact]
            public void TaskId_Should_NotBeEmpty()
            {
                Flowtime.TaskId.Should().NotBeEmpty();
            }

            //[Fact]
            //public void Changes_Should_Contain_FlowtimeCreated_Type()
            //{
            //    var e = new FlowtimeCreated(Id, Task, FlowtimeState.Started);

            //    Flowtime.Changes.Should().Contain(e);
            //}
        }

        public class WithNullTask : OnCreateNewFlowtime
        {
            public override void Arrange()
            {
                Id = Guid.NewGuid();

                Task = null;

                Action = Act;
            }

            [Fact]
            public void Action_Should_Throw_ArgumentNullException()
            {
                Action.Should().Throw<ArgumentNullException>()
                    .WithParameterName("task");
            }

            //[Fact]
            //public void Changes_Should_NotContain_FlowtimeCreated_Type()
            //{
            //    var e = new FlowtimeCreated(Id, Task, FlowtimeState.Started);

            //    Flowtime.Changes.Should().NotContain(e);
            //}
        }
    }

    public abstract class OnStartFlowtime : FlowtimeUnitTest
    {
        public override void Act()
        {
            Flowtime.Start(StartDateTime);
        }

        public class AtNow : OnStartFlowtime
        {
            public override void Arrange()
            {
                Task = new Task("Test");

                StartDateTime = DateTime.Now;

                Flowtime = new Flowtime(Task);
            }

            [Fact]
            public void StartDateTime_Should_NotBeNull()
            {
                Flowtime.StartDateTime.Should().NotBeNull();
            }

            [Fact]
            public void State_Should_Be_Flow()
            {
                Flowtime.State.Should().Be(FlowtimeState.Flow);
            }

            //[Fact]
            //public void Changes_Should_Contain_FlowtimeStarted_Type()
            //{
            //    var e = new FlowtimeStarted(Id, StartDateTime.Value);

            //    Flowtime.Changes.Should().Contain(e);
            //}
        }
    }

    public abstract class OnInterruptFlowtime : FlowtimeUnitTest
    {
        public override void Act()
        {
            Flowtime.Interrupt(StopDateTime);
        }

        public class At25MinutesDuration : OnInterruptFlowtime
        {
            public TimeSpan About25Minutes { get; set; }

            public TimeSpan About5Minutes { get; set; }


            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About25Minutes = TimeSpan.FromMinutes(25);

                About5Minutes = TimeSpan.FromMinutes(5);

                StartDateTime = StopDateTime.Add(-About25Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeTrue()
            {
                Flowtime.Interrupted.Should().BeTrue();
            }

            [Fact]
            public void Worktime_Should_Be_About25Minutes()
            {
                Flowtime.Worktime.Should().Be(About25Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About5Minutes()
            {
                Flowtime.Breaktime.Should().Be(About5Minutes);
            }
        }

        public class At50MinutesDuration : OnInterruptFlowtime
        {
            public TimeSpan About50Minutes { get; set; }

            public TimeSpan About8Minutes { get; set; }

            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About50Minutes = TimeSpan.FromMinutes(50);

                About8Minutes = TimeSpan.FromMinutes(8);

                StartDateTime = StopDateTime.Add(-About50Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeTrue()
            {
                Flowtime.Interrupted.Should().BeTrue();
            }

            [Fact]
            public void Worktime_Should_Be_About50Minutes()
            {
                Flowtime.Worktime.Should().Be(About50Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About8Minutes()
            {
                Flowtime.Breaktime.Should().Be(About8Minutes);
            }
        }

        public class At90MinutesDuration : OnInterruptFlowtime
        {
            public TimeSpan About90Minutes { get; set; }

            public TimeSpan About10Minutes { get; set; }

            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About90Minutes = TimeSpan.FromMinutes(90);

                About10Minutes = TimeSpan.FromMinutes(10);

                StartDateTime = StopDateTime.Add(-About90Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeTrue()
            {
                Flowtime.Interrupted.Should().BeTrue();
            }

            [Fact]
            public void Worktime_Should_Be_About90Minutes()
            {
                Flowtime.Worktime.Should().Be(About90Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About10Minutes()
            {
                Flowtime.Breaktime.Should().Be(About10Minutes);
            }
        }

        public class At91MinutesDuration : OnInterruptFlowtime
        {
            public TimeSpan About91Minutes { get; set; }

            public TimeSpan About15Minutes { get; set; }

            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About91Minutes = TimeSpan.FromMinutes(91);

                About15Minutes = TimeSpan.FromMinutes(15);

                StartDateTime = StopDateTime.Add(-About91Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeTrue()
            {
                Flowtime.Interrupted.Should().BeTrue();
            }

            [Fact]
            public void Worktime_Should_Be_About91Minutes()
            {
                Flowtime.Worktime.Should().Be(About91Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About15Minutes()
            {
                Flowtime.Breaktime.Should().Be(About15Minutes);
            }
        }
    }

    public abstract class OnStopFlowtime : FlowtimeUnitTest
    {
        public override void Act()
        {
            Flowtime.Stop(StopDateTime);
        }

        public class At25MinutesDuration : OnStopFlowtime
        {
            public TimeSpan About25Minutes { get; set; }

            public TimeSpan About5Minutes { get; set; }

            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About25Minutes = TimeSpan.FromMinutes(25);
                
                About5Minutes = TimeSpan.FromMinutes(5);

                StartDateTime = StopDateTime.Add(-About25Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeFalse()
            {
                Flowtime.Interrupted.Should().BeFalse();
            }

            [Fact]
            public void Worktime_Should_Be_About25Minutes()
            {
                Flowtime.Worktime.Should().Be(About25Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About5Minutes()
            {
                Flowtime.Breaktime.Should().Be(About5Minutes);
            }
        }

        public class At50MinutesDuration : OnStopFlowtime
        {
            public TimeSpan About50Minutes { get; set; }

            public TimeSpan About8Minutes { get; set; }

            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About50Minutes = TimeSpan.FromMinutes(50);

                About8Minutes = TimeSpan.FromMinutes(8);

                StartDateTime = StopDateTime.Add(-About50Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeFalse()
            {
                Flowtime.Interrupted.Should().BeFalse();
            }

            [Fact]
            public void Worktime_Should_Be_About50Minutes()
            {
                Flowtime.Worktime.Should().Be(About50Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About8Minutes()
            {
                Flowtime.Breaktime.Should().Be(About8Minutes);
            }
        }

        public class At90MinutesDuration : OnStopFlowtime
        {
            public TimeSpan About90Minutes { get; set; }

            public TimeSpan About10Minutes { get; set; }

            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About90Minutes = TimeSpan.FromMinutes(90);

                About10Minutes = TimeSpan.FromMinutes(10);

                StartDateTime = StopDateTime.Add(-About90Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeFalse()
            {
                Flowtime.Interrupted.Should().BeFalse();
            }

            [Fact]
            public void Worktime_Should_Be_About90Minutes()
            {
                Flowtime.Worktime.Should().Be(About90Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About10Minutes()
            {
                Flowtime.Breaktime.Should().Be(About10Minutes);
            }
        }

        public class At91MinutesDuration : OnStopFlowtime
        {
            public TimeSpan About91Minutes { get; set; }

            public TimeSpan About15Minutes { get; set; }

            public override void Arrange()
            {
                Task = new Task("Test");

                Flowtime = new Flowtime(Task);

                StopDateTime = DateTime.Now;

                About91Minutes = TimeSpan.FromMinutes(91);

                About15Minutes = TimeSpan.FromMinutes(15);

                StartDateTime = StopDateTime.Add(-About91Minutes);

                Flowtime.Start(StartDateTime);
            }

            [Fact]
            public void State_Should_Be_Finished()
            {
                Flowtime.State.Should().Be(FlowtimeState.Finished);
            }

            [Fact]
            public void Interrupted_Should_BeFalse()
            {
                Flowtime.Interrupted.Should().BeFalse();
            }

            [Fact]
            public void Worktime_Should_Be_About91Minutes()
            {
                Flowtime.Worktime.Should().Be(About91Minutes);
            }

            [Fact]
            public void Breaktime_Should_Be_About15Minutes()
            {
                Flowtime.Breaktime.Should().Be(About15Minutes);
            }
        }
    }
}
