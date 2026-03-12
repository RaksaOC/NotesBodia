using System;

public interface IDbTable<T>
{
    string TableName { get; }
    T Cols { get; }
}

public static class Tables
{
    // for generic usage at base repo
    public static string Id => BaseCols.IdCol;

    // all tables
    public static UsersTable Users { get; } = new UsersTable();
    public static NotesTable Notes { get; } = new NotesTable();
}

public abstract class BaseCols
{
    protected BaseCols() { }

    public string Id => IdCol;
    public string CreatedAt => CreatedAtCol;
    public string UpdatedAt => UpdatedAtCol;

    public const string IdCol = "Id";
    public const string CreatedAtCol = "CreatedAt";
    public const string UpdatedAtCol = "UpdatedAt";
}


public class UsersTable : IDbTable<UsersCols>
{
    public UsersTable() { }

    public string TableName => "users";
    public UsersCols Cols => new UsersCols();
}

public class UsersCols : BaseCols
{
    public UsersCols() { }
    public string Email => EmailCol;
    public string Password => PasswordCol;

    public const string EmailCol = "Email";
    public const string PasswordCol = "Password";
}




public class NotesTable : IDbTable<NotesCols>
{
    public NotesTable() { }

    public string TableName => "notes";
    public NotesCols Cols => new NotesCols();
}

public class NotesCols : BaseCols
{
    public NotesCols() { }

    public const string TitleCol = "Title";
    public const string ContentCol = "Content";
    public const string UserIdCol = "UserId";

    public string Title => TitleCol;
    public string Content => ContentCol;
    public string UserId => UserIdCol;
}

