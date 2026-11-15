/*
  Seed data for local/dev PostgreSQL.

  - Creates a demo user (if not already present)
  - Inserts a few notes for that user (if they have none)

  Test credentials (see README):
    email: demo@notesbodia.local
    password: TestPassword123!
*/

INSERT INTO Users (Email, Password)
VALUES ('demo@notesbodia.local', '$2y$12$QF682PHfbywoMI7QS4IQK.vWqqcvEx8PojqkohECpyGA0.LS.CWLy')
ON CONFLICT (Email) DO NOTHING;

INSERT INTO Notes (Title, Content, UserId)
SELECT v.Title, v.Content, u.Id
FROM Users u
CROSS JOIN (VALUES
    ('Welcome to Notesbodia', 'This is a seeded note created by DML.sql. You can edit or delete it.'),
    ('Markdown-ish content', E'Try storing: - lists\n- quick thoughts\n- and snippets'),
    ('Search me', 'Use the search box to filter notes by title.')
) AS v(Title, Content)
WHERE u.Email = 'demo@notesbodia.local'
  AND NOT EXISTS (SELECT 1 FROM Notes n WHERE n.UserId = u.Id);
