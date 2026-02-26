using Pr1.MinWebService.Domain;

namespace Pr1.MinWebService.Services;

public interface IDisciplineRepository
{
    IReadOnlyCollection<Discipline> GetAll();
    Discipline? GetById(Guid id);
    Discipline Create(string name, string lecturer, int studentCount);
}