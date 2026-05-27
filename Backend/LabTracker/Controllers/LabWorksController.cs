using LabTracker.Data;
using LabTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabWorksController : ControllerBase
{
    private readonly AppDbContext _db;

    public LabWorksController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var works = await _db.LabWorks
            .Include(l => l.Student)
            .Include(l => l.Subject)
            .ToListAsync();
        return Ok(works);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var work = await _db.LabWorks
            .Include(l => l.Student)
            .Include(l => l.Subject)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (work == null) return NotFound();
        return Ok(work);
    }

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetByStudent(int studentId)
    {
        var works = await _db.LabWorks
            .Include(l => l.Subject)
            .Where(l => l.StudentId == studentId)
            .ToListAsync();
        return Ok(works);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LabWork labWork)
    {
        _db.LabWorks.Add(labWork);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = labWork.Id }, labWork);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LabWork updated)
    {
        var work = await _db.LabWorks.FindAsync(id);
        if (work == null) return NotFound();

        work.StudentId = updated.StudentId;
        work.SubjectId = updated.SubjectId;
        work.LabNumber = updated.LabNumber;
        work.Title = updated.Title;
        work.Status = updated.Status;
        work.Grade = updated.Grade;
        work.SubmittedDate = updated.SubmittedDate;
        work.Notes = updated.Notes;

        await _db.SaveChangesAsync();
        return Ok(work);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var work = await _db.LabWorks.FindAsync(id);
        if (work == null) return NotFound();

        _db.LabWorks.Remove(work);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
