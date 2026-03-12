using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private readonly NoteService _noteService;

    public NotesController(NoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes([FromQuery] string? search, [FromQuery] FilterOptions filter)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not found");

        var notes = await _noteService.GetNotes(int.Parse(userId), search, filter);
        return Ok(ApiResponseUtil.Success(notes));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not found");

        var note = await _noteService.GetNoteById(int.Parse(userId), id);
        return Ok(ApiResponseUtil.Success(note));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] NoteCreateDTO note)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not found");

        var created = await _noteService.CreateNote(int.Parse(userId), note);
        return Ok(ApiResponseUtil.Success(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] NoteUpdateDTO note)
    {
        // cannot be noth null and also cannot be both not null meaning at least one fireld must be available
        if (note.Content == null && note.Title == null)
            return BadRequest(ApiResponseUtil.Error<NoteUpdateDTO>("You must update at least one field", note));
        if (note.Content != null && note.Title != null)
            return BadRequest(ApiResponseUtil.Error<NoteUpdateDTO>("You can only update one field at a time", note));


        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not found");

        var updated = await _noteService.UpdateNote(int.Parse(userId), id, note);
        return Ok(ApiResponseUtil.Success(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not found");

        var deleted = await _noteService.DeleteNote(int.Parse(userId), id);
        return Ok(ApiResponseUtil.Success(deleted));
    }
}