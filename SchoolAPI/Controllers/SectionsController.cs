[ApiController]
[Route("api/[controller]")]
public class SectionsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public SectionsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // Get all sections
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.Sections.ToListAsync());
    }

    // Add new section
    [HttpPost]
    public async Task<IActionResult> Add(SectionDto sectionDto)
    {
        var section = new Section
        {
            Name = sectionDto.Name,
            SubjectId = sectionDto.SubjectId
        };

        _db.Sections.Add(section);
        await _db.SaveChangesAsync();

        return Ok(section);
    }
}