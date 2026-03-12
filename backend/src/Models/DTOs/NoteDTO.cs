public class NoteCreateDTO    
{
    public string Title { get; set; }
    public string? Content { get; set; }
}

public class NoteUpdateDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}