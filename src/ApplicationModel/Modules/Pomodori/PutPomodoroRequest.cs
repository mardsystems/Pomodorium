using System.ComponentModel.DataAnnotations;

namespace Pomodorium;

public class PutPomodoroRequest : Request
{
    public string Description { get; set; }
}
