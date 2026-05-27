using LabTracker.Data;
using LabTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _db.Students.ToListAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Student student)
    {
        _db.Students.Add(student);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Student updated)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return NotFound();

        student.LastName = updated.LastName;
        student.FirstName = updated.FirstName;
        student.MiddleName = updated.MiddleName;
        student.Group = updated.Group;

        await _db.SaveChangesAsync();
        return Ok(student);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return NotFound();

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
