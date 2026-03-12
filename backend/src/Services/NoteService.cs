using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NoteService
{
    private readonly DbConnectionFactory _factory;

    public NoteService(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Note>> GetNotes(int userId, string? search, FilterOptions filter)
    {
        var sql = $"SELECT * FROM {Tables.Notes.TableName} WHERE {Tables.Notes.Cols.UserId} = @UserId";
        if (!string.IsNullOrWhiteSpace(search))
        {
            sql += $" AND {Tables.Notes.Cols.Title} LIKE '%{search}%'";
        }
        if (filter == FilterOptions.Latest)
        {
            sql += $" ORDER BY {Tables.Notes.Cols.CreatedAt} DESC";
        }
        else if (filter == FilterOptions.Oldest)
        {
            sql += $" ORDER BY {Tables.Notes.Cols.CreatedAt} ASC";
        }
        else if (filter == FilterOptions.LastAccessed)
        {
            sql += $" ORDER BY {Tables.Notes.Cols.UpdatedAt} DESC";
        }
        else if (filter == FilterOptions.Alphabetical)
        {
            sql += $" ORDER BY {Tables.Notes.Cols.Title} ASC";
        }
        using var conn = _factory.CreateConnection();
        var notes = await conn.QueryAsync<Note>(sql, new { UserId = userId });

        // cut down content because this api is used at the sidebar whcih just shows a snippet and cut down payload size
        // since theres also no pagination 
        // full content is fetched when the note is opened (by id service below)
        foreach (var note in notes)
        {
            if (note.Content != null && note.Content.Length > 100)
            {
                note.Content = note.Content.Substring(0, 100) + "...";
            }
        }
        return notes;
    }

    public async Task<Note?> GetNoteById(int userId, int id)
    {
        var sql = $"SELECT * FROM {Tables.Notes.TableName} WHERE {Tables.Notes.Cols.Id} = @Id AND {Tables.Notes.Cols.UserId} = @UserId ";
        using var conn = _factory.CreateConnection();
        // update updated at
        var sql2 = $"UPDATE {Tables.Notes.TableName} SET {Tables.Notes.Cols.UpdatedAt} = @UpdatedAt WHERE {Tables.Notes.Cols.Id} = @Id AND {Tables.Notes.Cols.UserId} = @UserId";
        await conn.ExecuteAsync(sql2, new { UpdatedAt = DateTime.UtcNow, Id = id, UserId = userId });
        var note = await conn.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
        if (note == null)
            throw new Exception("Note not found");
        return note;
    }

    public async Task<Note> CreateNote(int userId, NoteCreateDTO note)
    {
        var sql = $@"
INSERT INTO {Tables.Notes.TableName} ({Tables.Notes.Cols.Title}, {Tables.Notes.Cols.Content}, {Tables.Notes.Cols.UserId})
OUTPUT INSERTED.*
VALUES (@Title, @Content, @UserId);";

        using var conn = _factory.CreateConnection();
        return await conn.QuerySingleAsync<Note>(sql, new { Title = note.Title, Content = note.Content, UserId = userId });
    }

    public async Task<Note> UpdateNote(int userId, int id, NoteUpdateDTO note)
    {
        using var conn = _factory.CreateConnection();

        var checkSql =
            $"SELECT COUNT(1) FROM {Tables.Notes.TableName} WHERE {Tables.Notes.Cols.Id} = @Id AND {Tables.Notes.Cols.UserId} = @UserId";
        var count = await conn.ExecuteScalarAsync<int>(checkSql, new { Id = id, UserId = userId });

        if (count == 0)
            throw new UnauthorizedAccessException("You do not have permission to update this note.");

        //  build SET clause only for supplied fields
        var setClauses = new List<string>();
        var queryParams = new DynamicParameters();
        queryParams.Add("Id", id);

        if (note.Title != null)
        {
            setClauses.Add($"{Tables.Notes.Cols.Title} = @Title");
            queryParams.Add("Title", note.Title);
        }
        if (note.Content != null)
        {
            setClauses.Add($"{Tables.Notes.Cols.Content} = @Content");
            queryParams.Add("Content", note.Content);
        }

        if (setClauses.Count == 0)
            throw new ArgumentException("You must update at least one field");

        var setClause = string.Join(", ", setClauses);

        var sql = $@"
UPDATE {Tables.Notes.TableName}
SET {setClause}
WHERE {Tables.Notes.Cols.Id} = @Id AND {Tables.Notes.Cols.UserId} = @UserId;

SELECT *
FROM {Tables.Notes.TableName}
WHERE {Tables.Notes.Cols.Id} = @Id AND {Tables.Notes.Cols.UserId} = @UserId;
";

        queryParams.Add("UserId", userId);
        // db trigger will automatically change the updated field 

        return await conn.QuerySingleAsync<Note>(sql, queryParams);
    }

    public async Task<bool> DeleteNote(int userId, int id)
    {
        using var conn = _factory.CreateConnection();


        var checkSql =
            $"SELECT COUNT(1) FROM {Tables.Notes.TableName} WHERE {Tables.Notes.Cols.Id} = @Id AND {Tables.Notes.Cols.UserId} = @UserId";
        var count = await conn.ExecuteScalarAsync<int>(checkSql, new { Id = id, UserId = userId });

        if (count == 0)
            throw new UnauthorizedAccessException("You do not have permission to delete this note.");

        var sql = $"DELETE FROM {Tables.Notes.TableName} WHERE {Tables.Notes.Cols.Id} = @Id";
        var rowsAffected = await conn.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}