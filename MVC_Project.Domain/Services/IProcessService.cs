using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MVC_Project.Domain.Services
{
    public interface IProcessService : IService<Process>
    {
        Process GetByCode(string code);
        ProcessExecution CreateExecution(ProcessExecution processExecution);
        ProcessExecution UpdateExecution(ProcessExecution processExecution);
        IList<ProcessExecution> GetAllExecutions();
    }
    
}
