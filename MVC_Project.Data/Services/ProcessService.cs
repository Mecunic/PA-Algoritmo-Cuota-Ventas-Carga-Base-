using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System.Collections.Generic;
using System.Linq;

namespace MVC_Project.Data.Services
{
   
    public class ProcessService : ServiceBase<Process>, IProcessService
    {
        private IRepository<Process> _repository;
        //private IRepository<ProcessExecution> _repositoryExecutions;

        public ProcessService(IRepository<Process> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
            //_repositoryExecution = repositoryExecution;
        }

        public Process GetByCode(string code)
        {
            var payments = _repository.Session.QueryOver<Process>().Where(x => x.Code == code);
            return payments.List().FirstOrDefault();
        }

        public ProcessExecution CreateExecution(ProcessExecution processExecution)
        {
            _repository.Session.Save(processExecution);
            return processExecution;
        }

        public ProcessExecution UpdateExecution(ProcessExecution processExecution)
        {
            _repository.Session.Update(processExecution);
            return processExecution;
        }

        public IList<ProcessExecution> GetAllExecutions()
        {
            return _repository.Session.Query<ProcessExecution>().OrderByDescending(x => x.StartAt ).ToList();
        }
    }
}
