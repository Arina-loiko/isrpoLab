using LabTracker.Data;
using LabTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _db;

    public SubjectsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subjects = await _db.Subjects.ToListAsync();
        return Ok(subjects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var subject = await _db.Subjects.FindAsync(id);
        if (subject == null) return NotFound();
        return Ok(subject);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Subject subject)
    {
        _db.Subjects.Add(subject);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = subject.Id }, subject);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Subject updated)
    {
        var subject = await _db.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        subject.Name = updated.Name;
        subject.TeacherName = updated.TeacherName;

        await _db.SaveChangesAsync();
        return Ok(subject);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var subject = await _db.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        _db.Subjects.Remove(subject);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
