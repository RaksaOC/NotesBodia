## Notesbodia (Techbodia Test)

A simple notes app with email/password auth and CRUD notes.

### Architecture

- **Frontend**: Vue 3 + Vite (TypeScript). Talks to the backend over HTTP and stores the JWT in `localStorage`.
- **Backend**: ASP.NET Core Web API (.NET 9). Uses:
  - **JWT Bearer auth** for protected endpoints
  - **BCrypt** for password hashing
  - **Dapper + Microsoft.Data.SqlClient** for SQL Server access
- **Database**: SQL Server with two tables:
  - `Users` (Id, Email, Password, timestamps)
  - `Notes` (belongs to `Users` via `UserId` FK, timestamps)

Auth flow:
- `POST /api/auth/signup` and `POST /api/auth/login` return `{ email, token }`
- Notes endpoints are under `api/notes/*` and require `Authorization: Bearer <token>`
- The backend reads the user id from the JWT `NameIdentifier` claim and uses it as `Notes.UserId`

### Repo structure

```
backend/   ASP.NET Core API (.NET 9)
frontend/  Vue 3 app (Vite + TS)
```

### Prerequisites

- **Node.js**: `>= 20.19` (or `>= 22.12`)
- **.NET SDK**: `9.x`
- **SQL Server**: local instance, Docker, or hosted (Azure SQL also works)

### Database setup

1) Create the schema by running:
- `backend/src/Database/sql/DDL.sql`

2) (Optional) Seed demo data:
- `backend/src/Database/sql/DML.sql`

### Backend setup (API)

The API reads settings from `appsettings.json` + `appsettings.Development.json` (when `ASPNETCORE_ENVIRONMENT=Development`).

Configure:
- **Connection string**: `ConnectionStrings:DefaultConnection`
- **JWT secret**: `Jwt:Key`

Run the API:

```bash
cd backend
dotnet restore
dotnet run
```

By default it listens on `http://localhost:5025` (see `backend/Properties/launchSettings.json`).

### Frontend setup (Web)

Set the API base URL in `frontend/.env`:

```env
VITE_API_URL=http://localhost:5025
```

Run the web app:

```bash
cd frontend
npm install
npm run dev
```

### Test credentials (seeded)

If you ran `backend/src/Database/sql/DML.sql`, you can use:

- **Email**: `demo@notesbodia.local`
- **Password**: `TestPassword123!`

### Useful endpoints

- **Auth**
  - `POST /api/auth/signup`
  - `POST /api/auth/login`
- **Notes** (JWT required)
  - `GET /api/notes`
  - `GET /api/notes/{id}`
  - `POST /api/notes`
  - `PUT /api/notes/{id}`
  - `DELETE /api/notes/{id}`

