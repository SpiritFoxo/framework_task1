namespace Pr1.MinWebService.Domain;

public sealed record CreateDisciplineRequest(string Name, string Lecturer, int StudentCount);
