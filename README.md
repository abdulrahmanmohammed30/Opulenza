**Opulenza API**

A robust e-commerce API for an online electronics store specializing in laptops and computers, built with ASP.NET Core and Clean Architecture. This system seamlessly integrates modern functionalities to provide a secure and dynamic shopping experience.

**Key Features:**

- **Robust User Management:**  
  - Comprehensive account endpoints for registering, logging in (including GitHub OAuth), refreshing tokens, email confirmations, password changes, account deletion, and more.  
  - JWT-based authentication coupled with Identity for ironclad security.

- **Dynamic Catalog & Product Handling:**  
  - Full CRUD operations on products and categories, including managing images and file uploads.  
  - Intuitive endpoints that cover everything from product details (`/api/products/{id}`) to category image management.

- **Seamless Shopping Flow:**  
  - Integrated shopping cart functionality with endpoints to add, view, and manage items.  
  - Effortless checkout process for orders, enhanced by Stripe payment integration.

- **Real-Time & Interactive Communication:**  
  - Utilization of SignalR for real-time updates and notifications, ensuring a fluid user experience.

- **Cutting-Edge Middleware & Services:**  
  - Custom middlewares, Fluent Validations, and CORS policies to safeguard and streamline communications.  
  - Mediator pattern with a generic repository for decoupled business logic and scalable architecture.

- **Developer-Centric Tools & Monitoring:**  
  - Swagger-powered API documentation for a self-explanatory, interactive exploration of endpoints.  
  - Advanced logging with Serilog (MSSqlServer and Seq sinks), Redis caching, and comprehensive error handling using ErrorOr.

- **Comprehensive Package Ecosystem:**  
  - A suite of modern .NET packages such as Asp.Versioning.Mvc, AspNet.Security.OAuth.GitHub, MailKit, MediatR, and Entity Framework Core – ensuring productivity and maintainability.

Every endpoint—from `/api/auth/*` actions and cart management to product ratings and wishlist functionalities—has been meticulously crafted to offer a state-of-the-art backend solution.

![1](https://github.com/user-attachments/assets/86a37c95-0e2b-4e83-bafc-1f8f596060be)
![2](https://github.com/user-attachments/assets/8a41f60c-8c2c-4e40-b76a-6645bbe15d05)
![3](https://github.com/user-attachments/assets/c027c0d5-f5d0-47fb-804d-690cb6340efb)


