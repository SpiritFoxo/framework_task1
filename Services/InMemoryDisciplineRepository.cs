using System.Collections.Concurrent;
using Pr1.MinWebService.Domain;

namespace Pr1.MinWebService.Services;

/// <summary>
/// Простое хранилище в памяти процесса.
/// </summary>
public sealed class InMemoryDisciplineRepository : IDisciplineRepository
{
    private readonly ConcurrentDictionary<Guid, Discipline> _disciplines = new();

    public IReadOnlyCollection<Discipline> GetAll()
        => _disciplines.Values
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    public Discipline? GetById(Guid id)
        => _disciplines.TryGetValue(id, out var d) ? d : null;

    public Discipline Create(string name, string lecturer, int studentCount)
    {
        var id = Guid.NewGuid();
        var discipline = new Discipline(id, name, lecturer, studentCount);
        _disciplines[id] = discipline;
        return discipline;
    }
}
