-- PostgreSQL schema for Notesbodia (safe to run repeatedly).
-- Unquoted identifiers are stored lowercase; Dapper maps them case-insensitively.

CREATE TABLE IF NOT EXISTS Users (
    Id        INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Email     VARCHAR(255) NOT NULL UNIQUE,
    Password  VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMPTZ  NOT NULL DEFAULT now(),
    UpdatedAt TIMESTAMPTZ  NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS Notes (
    Id        INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Title     VARCHAR(255) NOT NULL,
    Content   TEXT NULL,
    CreatedAt TIMESTAMPTZ  NOT NULL DEFAULT now(),
    UpdatedAt TIMESTAMPTZ  NOT NULL DEFAULT now(),
    UserId    INT NOT NULL,
    CONSTRAINT FK_Notes_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_Notes_UserId ON Notes (UserId);

-- keep UpdatedAt current on every update
CREATE OR REPLACE FUNCTION set_updated_at() RETURNS trigger AS $$
BEGIN
    NEW.UpdatedAt := now();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_UpdateNotesTimestamp ON Notes;
CREATE TRIGGER trg_UpdateNotesTimestamp
    BEFORE UPDATE ON Notes
    FOR EACH ROW EXECUTE FUNCTION set_updated_at();
