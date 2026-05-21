<div align="center">

# AYN — All You Need.

### A full-stack peer-to-peer trading platform built with ASP.NET Core

<br/>

[![Stars](https://img.shields.io/github/stars/georgidelchev/AYN-?style=for-the-badge&color=F59E0B&logo=github)](https://github.com/georgidelchev/AYN-/stargazers)
[![Forks](https://img.shields.io/github/forks/georgidelchev/AYN-?style=for-the-badge&color=22C55E&logo=github)](https://github.com/georgidelchev/AYN-/network/members)
[![License](https://img.shields.io/badge/License-MIT-7C3AED?style=for-the-badge)](LICENSE)
[![Build](https://img.shields.io/github/actions/workflow/status/georgidelchev/AYN-/dotnet.yml?style=for-the-badge&label=CI&logo=github-actions&logoColor=white)](https://github.com/georgidelchev/AYN-/actions)

<br/>

[![C#](https://img.shields.io/badge/C%23-36%25-239120?style=flat-square&logo=csharp&logoColor=white)](https://github.com/georgidelchev/AYN-/search?l=c%23)
[![JavaScript](https://img.shields.io/badge/JavaScript-25%25-F7DF1E?style=flat-square&logo=javascript&logoColor=black)](https://github.com/georgidelchev/AYN-/search?l=javascript)
[![CSS](https://img.shields.io/badge/CSS-15%25-1572B6?style=flat-square&logo=css3&logoColor=white)](https://github.com/georgidelchev/AYN-/search?l=css)
[![HTML](https://img.shields.io/badge/HTML-14%25-E34F26?style=flat-square&logo=html5&logoColor=white)](https://github.com/georgidelchev/AYN-/search?l=html)
[![SCSS](https://img.shields.io/badge/SCSS-9%25-CC6699?style=flat-square&logo=sass&logoColor=white)](https://github.com/georgidelchev/AYN-/search?l=scss)

<br/>

> *Buy it. Sell it. Find everything you need — all in one place.*

</div>

---

## 🧭 What is AYN?

**AYN (All You Need)** is a production-grade, peer-to-peer online marketplace built from scratch as a full-stack ASP.NET Core web application. It allows users to list items for sale, discover and purchase goods from other users, communicate in real time, and complete transactions securely — all within a single, cohesive platform.

This is not a tutorial project. It is a complete, layered application built with industry-standard architecture, a rich feature set, a live CI/CD pipeline, and a professional-grade tech stack — the result of bringing together everything learned across multiple courses and personal deep-dives into the .NET ecosystem.

---

## ✨ Feature Highlights

### 🛒 Marketplace Core
- **Create, edit, and delete listings** — title, description, price, category, subcategory, condition, and up to multiple images per ad
- **Rich-text ad descriptions** powered by TinyMCE with HTML sanitization via HtmlSanitizer
- **Image upload & storage** through Cloudinary CDN — fast, optimized, globally delivered
- **Advanced search & filtering** — by keyword, category, subcategory, price range, town, and sort order
- **Watchlist / Favourites** — save any listing and revisit it anytime from your profile

### 💬 Real-Time Chat
- **Live messaging** between buyers and sellers powered by **SignalR** WebSockets
- Persistent conversation history — no messages are ever lost
- Clean, responsive chat UI with unread message indicators

### 💳 Payments
- **Stripe integration** for secure, card-based payments directly within the platform
- Smooth checkout flow with real-time validation

### 📧 Email & Notifications
- **SendGrid** transactional email — account confirmation, password reset, and system notifications
- In-app notification system keeping users informed of activity on their listings

### ⏰ Background Jobs
- **Hangfire** scheduled tasks — automated cleanup, periodic data processing, and recurring system operations
- Full Hangfire Dashboard for monitoring jobs in real time

### 👤 User Accounts & Security
- ASP.NET Core Identity — registration, login, role-based authorization
- Email confirmation flow and secure password reset
- Admin area with user management — promote, demote, ban, and unban users
- StyleCop + custom ruleset enforcing consistent, clean code across the entire codebase

### 📊 Administration
- Full **Admin Dashboard** — manage categories, subcategories, towns, and platform users
- Data scraping powered by **AngleSharp** for seeding realistic category and location data
- Role-based access control — users vs. administrators

---

## 🏗️ Architecture

AYN follows a clean **layered architecture** separating concerns across distinct projects:

```
AYN/
├── AYN.Common/          # Shared constants, helpers, GlobalConstants
├── Data/
│   ├── AYN.Data/        # DbContext, EF Core configuration, seeding
│   ├── AYN.Data.Common/ # Repository pattern, base interfaces
│   └── AYN.Data.Models/ # Domain entities (Ad, User, Category, Message…)
├── Services/
│   ├── AYN.Services/    # Business logic — ads, users, chat, notifications…
│   └── AYN.Services.Data/ # Data-access services, AutoMapper profiles
├── Tests/
│   └── AYN.Services.Tests/ # Unit tests with nUnit + Moq
└── Web/
    ├── AYN.Web/         # ASP.NET Core MVC — controllers, views, areas
    └── AYN.Web.ViewModels/ # Input & view models, DTOs
```

Every layer communicates through interfaces, enabling full testability and clean separation of concerns. The repository pattern abstracts all data access, and AutoMapper handles all object-to-object mapping between layers.

---

## 🛠️ Tech Stack

### Backend
| Technology | Purpose |
|------------|---------|
| **ASP.NET Core** | Web framework — MVC, Razor views, middleware pipeline |
| **Entity Framework Core** | ORM — code-first, migrations, LINQ queries |
| **SignalR** | Real-time WebSocket communication (chat) |
| **ASP.NET Core Identity** | Authentication, authorization, role management |
| **Hangfire** | Background job scheduling and processing |
| **AutoMapper** | Object mapping between layers |
| **AngleSharp** | HTML parsing and data scraping for seeding |
| **HtmlSanitizer** | Sanitizes user-generated rich-text content |

### Integrations
| Integration | Purpose |
|-------------|---------|
| **Stripe** | Payment processing |
| **SendGrid** | Transactional email delivery |
| **Cloudinary** | Image upload, storage, and CDN delivery |

### Frontend
| Technology | Purpose |
|------------|---------|
| **Bootstrap** | Responsive UI framework |
| **jQuery** | DOM manipulation and AJAX calls |
| **TinyMCE** | Rich-text editor for ad descriptions |
| **SCSS** | Custom styling and theme overrides |

### Testing & Quality
| Tool | Purpose |
|------|---------|
| **nUnit** | Unit testing framework |
| **Moq** | Mocking library for isolated service tests |
| **StyleCop** | Code style enforcement across the solution |
| **GitHub Actions** | CI/CD pipeline — build and test on every push |

---

## 🗂️ Data Model Overview

The application is built around a rich relational domain model:

- **Users** — extended ASP.NET Identity user with profile image, town, phone, and activity metadata
- **Ads** — listings with category, subcategory, condition, price, location, images, and view count
- **Categories & Subcategories** — hierarchical classification of listings
- **Towns** — location data for filtering and discovery
- **Messages** — real-time chat messages between users, associated with a conversation thread
- **Notifications** — system and user-generated notifications
- **Watchlist entries** — per-user saved/favourited listings

---

## 🚀 Getting Started

### Prerequisites

- [.NET 5+ SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local or remote)
- [Visual Studio 2019+](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/)
- Accounts for: Stripe, SendGrid, Cloudinary *(optional — core features work without them)*

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/georgidelchev/AYN-.git
   cd AYN-
   ```

2. **Configure secrets** — in `Web/AYN.Web/appsettings.json` (or user secrets), set your connection string and API keys:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=AYN;Trusted_Connection=True;"
     },
     "Stripe": { "SecretKey": "...", "PublishableKey": "..." },
     "SendGrid": { "ApiKey": "..." },
     "Cloudinary": { "CloudName": "...", "ApiKey": "...", "ApiSecret": "..." }
   }
   ```

3. **Apply migrations & seed the database**
   ```bash
   cd Web/AYN.Web
   dotnet ef database update
   dotnet run
   ```

4. **Run the tests**
   ```bash
   cd Tests/AYN.Services.Tests
   dotnet test
   ```

5. Open your browser at `https://localhost:5001` 🎉

---

## 🔄 CI/CD Pipeline

AYN includes a **GitHub Actions** workflow that automatically builds and runs all tests on every push and pull request to `main`, ensuring the codebase stays green at all times.

---

## 👤 Author

**Georgi Delchev**

[![GitHub](https://img.shields.io/badge/GitHub-georgidelchev-181717?style=flat-square&logo=github)](https://github.com/georgidelchev)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-delchevgeorgi-0A66C2?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/delchevgeorgi/)
[![Facebook](https://img.shields.io/badge/Facebook-georgi.d99-1877F2?style=flat-square&logo=facebook)](https://www.facebook.com/georgi.d99/)

---

## 🙏 Acknowledgements

- Project scaffolding based on [Nikolay Kostov's ASP.NET Core Template](https://github.com/NikolayIT/ASP.NET-Core-Template)
- The entire SoftUni community for the learning environment and inspiration

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

<div align="center">

*If you found this project useful or interesting, a ⭐ goes a long way — thank you!*

</div>
