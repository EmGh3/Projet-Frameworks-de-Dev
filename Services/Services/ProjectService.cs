using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Services.Services
{
    public class ProjectService : GenericService<Project>,IProjectService
    {
        public ProjectService(IProjectRepository repository) : base(repository)
        {

        }
        public IEnumerable<ProjectTask> GetTasks(int projectId)
        {
            return ((IProjectRepository)_repository).GetTasks(projectId);
        }
        public void UpdateProgress(int projectId)
        {
            var project = _repository.GetByIdAsync(projectId).Result;
            var tasks = GetTasks(projectId);
            var totalTasks = tasks.Count();
            var finishedTasks = tasks.Where(t => t.Status == ProjectTaskStatus.Finished).Count();
            project.Progress = (finishedTasks * 100) / totalTasks;
            _repository.UpdateAsync(project).Wait();
        }
        public void UpdateExpenses(int projectId)
        {
            var project = _repository.GetByIdAsync(projectId).Result;
            var tasks = GetTasks(projectId);
            var totalExpense = tasks.Sum(t => t.Cost);
            project.Expenses = (decimal)totalExpense;
            _repository.UpdateAsync(project).Wait();
        }
    }
}
