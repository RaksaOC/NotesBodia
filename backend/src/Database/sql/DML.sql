/*
  Seed data for local/dev SQL Server.

  - Creates a demo user (if not already present)
  - Inserts a few notes for that user

  Test credentials (see README):
    email: demo@notesbodia.local
    password: TestPassword123!
*/

SET NOCOUNT ON;

DECLARE @Email NVARCHAR(255) = 'demo@notesbodia.local';
DECLARE @PasswordHash NVARCHAR(255) = '$2y$12$QF682PHfbywoMI7QS4IQK.vWqqcvEx8PojqkohECpyGA0.LS.CWLy';
DECLARE @UserId INT;

-- Ensure demo user exists
SELECT @UserId = Id FROM Users WHERE Email = @Email;

IF @UserId IS NULL
BEGIN
  INSERT INTO Users (Email, Password, CreatedAt, UpdatedAt)
  VALUES (@Email, @PasswordHash, SYSUTCDATETIME(), SYSUTCDATETIME());

  SET @UserId = SCOPE_IDENTITY();
END

-- Seed notes if user has none
IF NOT EXISTS (SELECT 1 FROM Notes WHERE UserId = @UserId)
BEGIN
  INSERT INTO Notes (Title, Content, CreatedAt, UpdatedAt, UserId)
  VALUES
    ('Welcome to Notesbodia', 'This is a seeded note created by DML.sql. You can edit or delete it.', SYSUTCDATETIME(), SYSUTCDATETIME(), @UserId),
    ('Markdown-ish content', 'Try storing: - lists\n- quick thoughts\n- and snippets', SYSUTCDATETIME(), SYSUTCDATETIME(), @UserId),
    ('Search me', 'Use the search box to filter notes by title.', SYSUTCDATETIME(), SYSUTCDATETIME(), @UserId);
END
