[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public StudentsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // Get all students
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.Students.ToListAsync());
    }

    // Add new student
    [HttpPost]
    public async Task<IActionResult> Add(StudentDto studentDto)
    {
        var student = new Student
        {
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            Email = studentDto.Email
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return Ok(student);
    }
}