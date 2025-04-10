using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.DTOs;

[ApiController]
[Route("api/[controller]")]
public class SectionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SectionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/sections
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Section>>> GetSections()
    {
        return await _context.Sections
            .Include(s => s.Subject)
            .ToListAsync();
    }

    // GET: api/sections/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Section>> GetSection(int id)
    {
        var section = await _context.Sections
            .Include(s => s.Subject)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (section == null)
        {
            return NotFound(new { message = "Section not found" });
        }

        return section;
    }

    // POST: api/sections
    [HttpPost]
    public async Task<ActionResult<Section>> CreateSection(SectionDto sectionDto)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(sectionDto.Name))
        {
            return BadRequest(new { message = "Section name is required" });
        }

        // Validate Subject exists
        var subjectExists = await _context.Subjects.AnyAsync(s => s.Id == sectionDto.SubjectId);
        if (!subjectExists)
        {
            return BadRequest(new { message = "Invalid SubjectId" }); // Now returns proper JSON
        }

        var section = new Section
        {
            Name = sectionDto.Name.Trim(),
            SubjectId = sectionDto.SubjectId
        };

        try
        {
            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSection),
                new { id = section.Id },
                new
                {
                    id = section.Id,
                    name = section.Name,
                    subjectId = section.SubjectId
                });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { message = "Error saving to database", details = ex.Message });
        }
    }

    // DELETE: api/sections/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSection(int id)
    {
        var section = await _context.Sections.FindAsync(id);
        if (section == null)
        {
            return NotFound(new { message = "Section not found" });
        }

        try
        {
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { message = "Error deleting section", details = ex.Message });
        }
    }
}