using DeerFit.Core.Models;
using DeerFit.Core.Repositories;

namespace DeerFit.Core.Services;

public class BookingService
{
    private readonly BookingRepository _bookingRepo;
    private readonly CourseRepository _courseRepo;

    public BookingService(BookingRepository bookingRepo, CourseRepository courseRepo)
    {
        _bookingRepo = bookingRepo;
        _courseRepo = courseRepo;
    }

    public Task<List<Booking>> GetAllAsync() => _bookingRepo.GetAllAsync();

    public Task MarkAttendedAsync(string bookingId) =>
        _bookingRepo.UpdateStatusAsync(bookingId, BookingStatus.Attended);

    public Task<List<Booking>> GetByMemberAsync(string memberId) =>
        _bookingRepo.GetByMemberAsync(memberId);

    public async Task<(bool Success, string Message)> BookAsync(string memberId, string courseId)
    {
        // Kurs laden & prüfen
        var course = await _courseRepo.GetByIdAsync(courseId);
        if (course is null)
            return (false, "Kurs nicht gefunden.");

        if (course.IsFull)
            return (false, "Kurs ist bereits ausgebucht.");

        // Doppelbuchung verhindern
        var existing = await _bookingRepo.GetExistingAsync(memberId, courseId);
        if (existing is not null)
            return (false, "Member ist bereits für diesen Kurs gebucht.");

        // Buchung anlegen & Kurs aktualisieren
        var booking = new Booking { MemberId = memberId, CourseId = courseId };
        await _bookingRepo.CreateAsync(booking);
        await _courseRepo.AddMemberToBookedListAsync(courseId, memberId);

        return (true, "Erfolgreich gebucht!");
    }

    public async Task<(bool Success, string Message)> CancelAsync(string bookingId)
    {
        var booking = await _bookingRepo.GetByIdAsync(bookingId);
        if (booking is null)
            return (false, "Buchung nicht gefunden.");

        await _bookingRepo.UpdateStatusAsync(bookingId, BookingStatus.Cancelled);
        await _courseRepo.RemoveMemberFromBookedListAsync(booking.CourseId, booking.MemberId);

        return (true, "Buchung erfolgreich storniert.");
    }
}