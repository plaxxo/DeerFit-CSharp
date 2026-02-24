using DeerFit.Core.Models;
using DeerFit.Core.Repositories;
using DeerFit.Core.Settings;

namespace DeerFit.Core.Services;

public class MemberService
{
    private readonly MemberRepository _memRepo;
    public MemberService(MemberRepository memRepo) => _memRepo = memRepo;

    public Task<List<Member>> GetAllAsync()            => _memRepo.GetAllAsync();
    public Task<List<Member>> GetActiveAsync()          => _memRepo.GetActiveAsync();
    public Task<Member?>      GetByIdAsync(string id)  => _memRepo.GetByIdAsync(id);
    public Task<List<Member>> SearchAsync(string term) => _memRepo.SearchAsync(term);

    public async Task<(bool Success, string Message)> CreateAsync(Member member)
    {
        var existing = await _memRepo.GetByEmailAsync(member.Email);
        if (existing is not null)
            return (false, string.Format(Messages.Member_AlreadyExists));

        await _memRepo.CreateAsync(member);
        return (true, string.Format(Messages.Member_Created));
    }

    public async Task<(bool Success, string Message)> UpdateAsync(string id, Member member)
    {
        var existing = await _memRepo.GetByIdAsync(id);
        if (existing is null)
            return (false, GetMemberNotFoundMessage(id));

        await _memRepo.UpdateAsync(id, member);
        return (true, string.Format(Messages.Member_Updated, member.FullName));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(string id)
    {
        var existing = await _memRepo.GetByIdAsync(id);
        if (existing is null)
            return (false, GetMemberNotFoundMessage(id));

        await _memRepo.DeleteAsync(id);
        return (true, string.Format(Messages.Member_Deleted, existing.FullName));
    }
    
    private static string GetMemberNotFoundMessage(string id) => string.Format(Messages.Member_NotFound, id);
}