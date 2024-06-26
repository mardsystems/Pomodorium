﻿using Pomodorium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodorium.Models.Tasks.Integrations;

public interface ITfsIntegrationService
{
    Task<IEnumerable<TaskInfo>> GetTaskInfoList(TfsIntegration tfsIntegration);
}
