using Pomodorium.Modules.Timers;
using System;
using System.Collections.Generic;
using System.DomainModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodorium.Modules.Activities;

public class Activity : AggregateRoot
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public Pomodoro Timer { get; private set; }

    public Activity(Pomodoro timer)
    {
        Timer = timer;
    }

    public void Focus()
    {
        if (true)
        {
            // continue
        }
        else
        {
            // create
        }
    }

    public void MoveAway()
    {

    }

    public void Complete()
    {
        Timer.Stop();
    }

    public Activity()
    {

    }
}
