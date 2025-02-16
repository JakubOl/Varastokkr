namespace Varastokkr.Shared.Models;

public record PaginatedResult<T>(IEnumerable<T> Result, int Count);