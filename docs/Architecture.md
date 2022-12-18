## Architecture Outline (Design)

**Client**

- Use Blazored Local + Session Storage Nuget
- Blazor WASM (offline support + scalability)
- Use Bootstrap / Tailwind CSS
- Use BuildWebCompiler Nuget (Sass compilation)
- Use Sass for CSS
- Use helper JavaScript libraries

---

**Api**

- Could use OData Nuget package
- Use Swagger (UI)
- Use Rate Limit
- Use Memory Cache
- Use Brotli Compression
- Use CORs Policy (if needed at all)
- Use Microsoft Identity (Authentication + Authorization)
- Use Api Versioning Nuget
- Using MediatR Nuget (CQRS Design Pattern)
- Create Command and Query classes (MediatR)
- Create DTO's for Db models
- Create Response models for Api
- Create Request models for Api
- Create DTO Validators
- Use Fluent Validation for DTO's

---

**Server**

- Use CloudFlare / LetsEncrypt
- Use TLSv1.2+
- Use Microsoft Identity (Authentication + Authorization)
- Vertical Slice Architecture (Group related things into `features` / `modules`)
- Securing user credentials properly (storing ClaimsPrincipal safely; setting Cookie server-side)
    - Use Blazor Protected Browser Storage Nuget
- Keep **ALL** secrets **OFF** Client (**ONLY SERVER**)
- Use Protected Browser Storage Nuget package
- Use Sessions over JWT (only use JWT to store Session ID or User ID)
- Evaluate GDPR compliance - [GDPR](https://gdpr.eu/)

---

**Database**

- Use PostgreSQL
- Use Lucid Chart to create an Entity Relationship Diagram
- Define clear User model
- Define Journal and Moment models
- Define Reflection model (and it's relationship b/w other models)
- Define Note model

---

**Testing**

- Test Api Endpoints
- Test Blazor Component (UI) operations
- Test CRUD Operations for Database
- Split the Unit Test projects into two:
    1. Regular Unit Tests
    2. Blazor Component Unit Tests
- End-2-End Test Project using Playwright
  1. Testing for round trip site functionality/behavior 
- Add a Benchmark Project?