# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-11-05

### Added
- Initial release of Insurance Partner Management System
- Partner management functionality
  - Create new partners with comprehensive validation
  - View all partners in sortable list (newest first)
  - View partner details in modal dialog
  - Real-time highlighting for partners with 5+ policies or 5000+ amount
- Policy management functionality
  - Add policies to partners via modal dialog
  - Real-time policy count and amount updates
  - Automatic partner highlighting based on policy metrics
- Database schema with migrations
  - Partners table with all required fields
  - Policies table with foreign key relationships
  - Proper indexes for performance
- Clean architecture implementation
  - Layered architecture (Controllers, Services, Repositories)
  - Repository pattern with Dapper
  - Service layer for business logic
  - Dependency injection throughout
- Comprehensive validation
  - Server-side validation with data annotations
  - Client-side validation with jQuery Validation
  - Unique constraint validation for external codes and policy numbers
- Responsive UI with Bootstrap 4
  - Mobile-friendly design
  - Modal dialogs for details and forms
  - Visual feedback for newly created partners
- Technical infrastructure
  - ASP.NET Core MVC 8.0
  - Dapper for data access
  - Entity Framework Core for migrations
  - SQL Server database
  - SOLID principles throughout
  - Async/await for all I/O operations

### Technical Details
- Implemented Repository Pattern
- Implemented Service Layer Pattern
- Used Dapper Micro ORM for data access
- Used EF Core for database migrations
- Bootstrap 4 for responsive UI
- jQuery for DOM manipulation and AJAX
- Proper separation of concerns
- Interface-based programming
- Dependency injection
- ViewModels for presentation logic

## [Unreleased]

### Planned Features
- Search and filtering for partners list
- Export to Excel/PDF
- Pagination for large datasets
- Audit logging
- User authentication and authorization
- RESTful API endpoints
- Dashboard with statistics
- Soft delete functionality
- Email notifications
- Multi-language support

---

[1.0.0]: https://github.com/dkrizanic/insurance-app/releases/tag/isporuka-v1
