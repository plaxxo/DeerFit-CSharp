using DeerFit.Core.Models;
using DeerFit.Core.Repositories;

namespace DeerFit.Core.Services;

public class MemberService
{
    private readonly MemberRepository _repo;
    public MemberService(MemberRepository repo) => _repo = repo;

    public Task<List<Member>> GetAllAsync()            => _repo.GetAllAsync();
    public Task<List<Member>> GetActiveAsync()          => _repo.GetActiveAsync();
    public Task<Member?>      GetByIdAsync(string id)  => _repo.GetByIdAsync(id);
    public Task<List<Member>> SearchAsync(string term) => _repo.SearchAsync(term);

    public async Task<(bool Success, string Message)> CreateAsync(Member member)
    {
        var existing = await _repo.GetByEmailAsync(member.Email);
        if (existing is not null)
            return (false, "Ein Member mit dieser E-Mail existiert bereits.");

        await _repo.CreateAsync(member);
        return (true, "Member erfolgreich erstellt.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(string id, Member member)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null)
            return (false, "Member nicht gefunden.");

        await _repo.UpdateAsync(id, member);
        return (true, "Member erfolgreich aktualisiert.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(string id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null)
            return (false, "Member nicht gefunden.");

        await _repo.DeleteAsync(id);
        return (true, "Member erfolgreich gelöscht.");
    }
}