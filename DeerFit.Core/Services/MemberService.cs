using DeerFit.Core.Models;
using DeerFit.Core.Repositories;
using DeerFit.Core.Settings;

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
            return (false, "Member with this email already exists");

        await _repo.CreateAsync(member);
        return (true, string.Format(Messages.Member_Created));
    }

    public async Task<(bool Success, string Message)> UpdateAsync(string id, Member member)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null)
            return (false, GetMemberNotFoundMessage(id));

        await _repo.UpdateAsync(id, member);
        return (true, string.Format(Messages.Member_Updated, member.FullName));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(string id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null)
            return (false, GetMemberNotFoundMessage(id));

        await _repo.DeleteAsync(id);
        return (true, string.Format(Messages.Member_Deleted, existing.FullName));
    }
    
    private static string GetMemberNotFoundMessage(string id) => string.Format(Messages.Member_NotFound, id);
}