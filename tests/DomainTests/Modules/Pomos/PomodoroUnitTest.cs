using FluentAssertions;

namespace Pomodorium.Modules.Pomos;

public abstract class PomodoroUnitTest : UnitTest
{
    public Guid Id { get; set; }

    public string? Task { get; set; }

    public TimeSpan Timer { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime Moment { get; set; }

    public Pomodoro Pomodoro { get; set; } = default!;

    public abstract class OnCreateNewPomodoro : PomodoroUnitTest
    {
        public override void Act()
        {
            Pomodoro = new Pomodoro(Task, Timer, Moment);
        }

        public class WithSuccess : OnCreateNewPomodoro
        {
            public override void Arrange()
            {
                Id = Guid.NewGuid();

                Task = "Test";

                Timer = TimeSpan.FromMinutes(25);

                Moment = DateTime.Now;
            }

            [Fact]
            public void Task_Should_NotBeNull()
            {
                Pomodoro.Task.Should().NotBeNull();
            }
        }

        public class WithNullTask : OnCreateNewPomodoro
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
        }
    }
}
