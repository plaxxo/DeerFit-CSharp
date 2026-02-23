using DeerFit.Core.Models;
using DeerFit.Core.Repositories;

namespace DeerFit.Core.Services;

public class CourseService
{
    private readonly CourseRepository _repo;
    public CourseService(CourseRepository repo) => _repo = repo;

    public Task<List<Course>> GetAllAsync()             => _repo.GetAllAsync();
    public Task<List<Course>> GetUpcomingAsync()        => _repo.GetUpcomingAsync();
    public Task<Course?>      GetByIdAsync(string id)  => _repo.GetByIdAsync(id);
    public Task               CreateAsync(Course c)    => _repo.CreateAsync(c);
    public Task               UpdateAsync(string id, Course c) => _repo.UpdateAsync(id, c);
    public Task               DeleteAsync(string id)   => _repo.DeleteAsync(id);
}