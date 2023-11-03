using FluentAssertions;

namespace Pomodorium.Modules.Pomos;

public abstract class PomodoroUnitTest : UnitTest
{
    public Guid Id { get; set; }

    public DateTime? StartDateTime { get; set; }

    public string? Description { get; set; }

    public Pomodoro Pomodoro { get; set; } = default!;

    public abstract class OnCreateNewPomodoro : PomodoroUnitTest
    {
        public override void Act()
        {
            Pomodoro = new Pomodoro(Id, Description);
        }

        public class WithSuccess : OnCreateNewPomodoro
        {
            public override void Arrange()
            {
                Id = Guid.NewGuid();

                Description = "Test";
            }

            [Fact]
            public void Description_Should_NotBeNull()
            {
                Pomodoro.Description.Should().NotBeNull();
            }

            //[Fact]
            //public void Changes_Should_Contain_PomodoroCreated_Type()
            //{
            //    var e = new PomodoroCreated(Id, Description, TimerState.Started);

            //    Pomodoro.Changes.Should().Contain(e);
            //}
        }

        public class WithNullDescription : OnCreateNewPomodoro
        {
            public override void Arrange()
            {
                Id = Guid.NewGuid();

                Description = null;

                Action = Act;
            }

            [Fact]
            public void Action_Should_Throw_ArgumentNullException()
            {
                Action.Should().Throw<ArgumentNullException>()
                    .WithParameterName("description");
            }

            //[Fact]
            //public void Changes_Should_NotContain_PomodoroCreated_Type()
            //{
            //    var e = new PomodoroCreated(Id, Description, TimerState.Started);

            //    Pomodoro.Changes.Should().NotContain(e);
            //}
        }
    }

    public abstract class OnStartPomodoro : PomodoroUnitTest
    {
        public override void Act()
        {
            Pomodoro.Start();
        }

        public class WithSuccess : OnStartPomodoro
        {
            public override void Arrange()
            {
                StartDateTime = DateTime.Now;

                Pomodoro = new Pomodoro();
            }

            [Fact]
            public void StartDateTime_Should_NotBeNull()
            {
                Pomodoro.StartDateTime.Should().NotBeNull();
            }

            [Fact]
            public void State_Should_Be_Started()
            {
                Pomodoro.State.Should().Be(TimerState.Started);
            }

            //[Fact]
            //public void Changes_Should_Contain_PomodoroStarted_Type()
            //{
            //    var e = new PomodoroStarted(Id, StartDateTime.Value);

            //    Pomodoro.Changes.Should().Contain(e);
            //}
        }
    }

    public abstract class OnPausePomodoro : PomodoroUnitTest
    {
        public override void Act()
        {
            Pomodoro.Pause();
        }

        public class WithSuccess : OnPausePomodoro
        {
            public override void Arrange()
            {
                Pomodoro = new Pomodoro();
            }

            [Fact]
            public void State_Should_Be_Paused()
            {
                Pomodoro.State.Should().Be(TimerState.Paused);
            }
        }
    }

    public abstract class OnStopPomodoro : PomodoroUnitTest
    {
        public override void Act()
        {
            Pomodoro.Stop();
        }

        public class WithSuccess : OnStopPomodoro
        {
            public override void Arrange()
            {
                Pomodoro = new Pomodoro();
            }

            [Fact]
            public void State_Should_Be_Stopped()
            {
                Pomodoro.State.Should().Be(TimerState.Stopped);
            }
        }
    }
}
