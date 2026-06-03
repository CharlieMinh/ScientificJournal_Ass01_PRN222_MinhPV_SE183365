# Project Progress - Scientific Journal MinhPV

## 2026-06-03: Build MinhPV Flow From Class Sample

Status: Completed locally.

Purpose:
- Build a small PRN222 MVC project focused only on MinhPV's assigned flow.
- Follow the class sample style used in KhanhLQ's project:
  - Entities project.
  - Repositories project.
  - Services project.
  - WebMVCApp project.
  - Controller -> Service -> Repository.
  - Bootstrap Razor CRUD views.
  - jQuery/DataTables-style table paging/search/sort.

Implemented scope:
- CRUD Publisher.
- CRUD Journal.
- CRUD Category.
- Assign Journal to Categories.
- View journals by category.
- View papers under journal.

Main tables used:
- `Publishers`
- `Journals_MinhPV`
- `Categories_MinhPV`
- `JournalCategories_MinhPV`
- `Papers_BaoTG`

Created repository layer:
- `Base/GenericRepository.cs`
- `PublishersRepository.cs`
- `JournalsMinhPvRepository.cs`
- `CategoriesMinhPvRepository.cs`

Created service layer:
- `IPublishersService`
- `IJournalsMinhPvService`
- `ICategoriesMinhPvService`
- `PublishersService`
- `JournalsMinhPvService`
- `CategoriesMinhPvService`

Created MVC controllers:
- `PublishersController`
- `JournalsController`
- `CategoriesController`

Created views:
- `/Publishers/Index`
- `/Publishers/Create`
- `/Publishers/Edit`
- `/Publishers/Delete`
- `/Publishers/Details`
- `/Journals/Index`
- `/Journals/Create`
- `/Journals/Edit`
- `/Journals/Delete`
- `/Journals/Details`
- `/Journals/AssignCategories`
- `/Categories/Index`
- `/Categories/Create`
- `/Categories/Edit`
- `/Categories/Delete`
- `/Categories/Details`

Other setup:
- Added WebMVCApp project references to Entities, Repositories, and Services.
- Registered DbContext, repositories, and services in `Program.cs`.
- Added `DefaultConnection` to `appsettings.json`.
- Updated `_Layout.cshtml` menu for Publishers, Journals, Categories.
- Updated `.slnx` to include all 4 projects.
- Added local DataTables-compatible jQuery shim:
  - `wwwroot/lib/datatables/js/jquery.dataTables.min.js`

Verification:

```text
dotnet build WebApplication1/ScientificJournal.WebMVCApp.MinhPV.csproj
Build succeeded.
0 Warning(s)
0 Error(s)

Smoke test:
/           -> 200
/Publishers -> 200
/Journals   -> 200
/Categories -> 200
```

## 2026-06-03: Add Login / Logout Authentication

Status: Completed locally.

Purpose:
- Add authentication flow similar to the class sample project.
- Protect MinhPV CRUD pages so users must login before managing Publisher, Journal, and Category data.

Implemented:
- Added `UsersHuyDdRepository`.
- Added `IUsersHuyDdService`.
- Added `UsersHuyDdService`.
- Added `LoginRequest` model.
- Added `AccountController` with:
  - `Login` GET.
  - `Login` POST.
  - `Logout`.
  - `Forbidden`.
- Added views:
  - `/Account/Login`
  - `/Account/Forbidden`
- Registered Cookie Authentication in `Program.cs`.
- Registered user repository/service in DI.
- Added `app.UseAuthentication()` before `app.UseAuthorization()`.
- Added `[Authorize]` to:
  - `PublishersController`
  - `JournalsController`
  - `CategoriesController`
- Updated layout:
  - Show `Login` when not authenticated.
  - Show `Hello, {User}` and `Logout` when authenticated.

Login behavior:
- Uses `Users_HuyDD.Email`.
- Checks active users only.
- Loads roles from `UserRoles_HuyDD` / `Roles_HuyDD`.
- Creates claims for:
  - `NameIdentifier`
  - `Name`
  - `Email`
  - `Role`
- Password check supports both:
  - SHA256 hash stored in `PasswordHash`.
  - Plain text stored in `PasswordHash`, for simple classroom demo data.

Verification:

```text
dotnet build WebApplication1/ScientificJournal.WebMVCApp.MinhPV.csproj --no-restore
Build succeeded.
0 Warning(s)
0 Error(s)

Smoke test:
/Account/Login -> 200
/Publishers    -> 302 redirect to Login when not authenticated
/Journals      -> 302 redirect to Login when not authenticated
/Categories    -> 302 redirect to Login when not authenticated
```

## 2026-06-03: Add Register And Fix Login Page Issues

Status: Completed locally.

Purpose:
- Add missing account registration flow.
- Fix login page browser console 404 caused by the old template CSS isolation file name.
- Make password hash comparison more tolerant for existing database users.

Completed:
- Added `RegisterRequest` model.
- Added `Account/Register` GET and POST.
- Added `/Account/Register` view.
- Added Register links in:
  - `_Layout.cshtml`
  - `/Account/Login`
- Removed old template CSS link:
  - `~/WebApplication1.styles.css`
- Updated password check:
  - User input is SHA256 hashed before comparison.
  - Hash comparison is case-insensitive.
  - Plain text comparison is still supported for classroom demo data.
- Register stores `PasswordHash` as SHA256.
- Register assigns default role `LecturerStudent` if that role exists in `Roles_HuyDD`.

Verification:

```text
dotnet build WebApplication1/ScientificJournal.WebMVCApp.MinhPV.csproj --no-restore
Build succeeded.
0 Warning(s)
0 Error(s)

Smoke test:
/Account/Login    -> 200
/Account/Register -> 200
Old CSS link present: false
```
