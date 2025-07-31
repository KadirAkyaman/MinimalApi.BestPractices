# MinimalApi.BestPractices Showcase

This project is a focused showcase of the essential architectural components that form the backbone of a modern, professional ASP.NET Core API, built on the cutting-edge **.NET 9 Preview**.

Each feature is implemented with a focus on best practices, demonstrating not just the "what," but the "why" behind professional API design. It serves as a blueprint for building robust, secure, and maintainable Minimal APIs.

---

## Technical Deep Dive: Core Concepts & Technologies

### 1. Request Pipeline & Endpoint Filters
The core of this project is a masterful control of the **HTTP Request Pipeline**. Instead of placing logic inside endpoint handlers, cross-cutting concerns are managed through a chain of Endpoint Filters (`IEndpointFilter`). This approach exemplifies the Single Responsibility and DRY principles at a micro-architectural level.
-   **Validation Filter:** A generic filter intercepts incoming DTOs, validates them using FluentValidation, and rejects invalid requests before they ever reach the business logic.
-   **Rate Limiting Logic:** The rate limiting policy is applied declaratively via `.RequireRateLimiting()`, ensuring security concerns are handled cleanly and separately.

### 2. FluentValidation for Robust Data Integrity
Data integrity is guaranteed not with cluttered data annotations, but with `FluentValidation`.
-   **Decoupled Rules:** Validation rules are completely decoupled from the DTOs, living in their own dedicated, testable `Validator` classes.
-   **Complex Logic:** The implementation uses powerful features like complex Regex for password strength and username formats.

### 3. Advanced Routing with Route Constraints
The API defends itself from malformed requests at the earliest possible stage: routing. This project leverages advanced Route Constraints to harden this first line of defense.
-   **Precise Matching:** Going beyond simple type checks like `:int`, the implementation uses chained constraints such as `:alpha:length(3,10)`. This guarantees that the `source` parameter from the URL path is not only present but also semantically correct in its format.
-   **Performance and Security:** This approach ensures that a request with an invalid format (e.g., a `source` parameter containing numbers or of the wrong length) is rejected at the very beginning of the request pipeline, before it ever reaches application code or validation filters. This prevents unnecessary resource consumption, enhancing both performance and security.
-   **Maximum Flexibility with `:regex`:** The project showcases the potential for even more powerful constraints available in ASP.NET Core, such as `:regex`. This allows URL segments to be enforced against a specific pattern (e.g., a product SKU format like `SKU-1234-A`), making the API's contract extremely rigid and fault-tolerant.

### 4. Flexible and Type-Safe Model Binding
The project demonstrates advanced and modern model binding techniques to cleanly map incoming HTTP request data to strongly-typed C# objects without cluttering the endpoint handlers.
-   **Binding from Multiple Sources:** The API showcases the ability to seamlessly bind parameters from different parts of the request in a single endpoint, such as `path` variables (`source`) and the `query string` (`filter`).
-   **Complex Object Binding from Query (`[AsParameters]`):** Instead of polluting the endpoint signature with numerous individual query parameters, the project utilizes the `[AsParameters]` attribute. This modern Minimal API feature allows related filter criteria (`Tags`, `StartDate`, `SortBy`) to be cleanly grouped into a single DTO (`DataFilterDto`) while still being populated from the query string. This greatly improves code readability and maintainability.
  
### 5. API Versioning for Future-Proofing
The API is designed to evolve without breaking existing clients, using the industry-standard API Versioning (`Asp.Versioning`).
-   **URL-Based Strategy:** A clear and explicit URL-based versioning strategy (`/api/v1/...`) is implemented.
-   **Swagger Integration:** Versioning is seamlessly integrated with the Swagger UI, allowing users to switch between different versions.

### 6. Professional API Documentation (The OpenAPI Ecosystem)
The project features a rich, interactive documentation powered by the OpenAPI (Swagger) ecosystem.
-   **SwaggerGen:** Automatically generates the `swagger.json` specification by reflecting over the API's endpoints.
-   **Swagger UI:** Provides a clean, browser-based UI for developers to explore and test the API endpoints interactively.
-   **.WithOpenApi() for Minimal APIs:** Documentation is applied directly to endpoints using the modern `.WithOpenApi()` method, providing programmatic control and guaranteeing accuracy.

### 7. Security via Rate Limiting
The API implements a robust Rate Limiting strategy to protect against abuse.
-   **IP-Based Policies:** A `Fixed Window` rate limiting policy is defined and applied on a per-IP basis to mitigate brute-force or DoS attempts.
-   **Declarative Application:** The policy is applied declaratively, keeping the endpoint handler clean.

### 8. Dependency Injection (DI)
The entire application is built on a foundation of Dependency Injection.
-   **Decoupled Services:** Services like `IValidator` are registered in the DI container and injected where needed (like in the `ValidationFilter`).

---

## Prerequisites

-   [.NET 9.0 Preview SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

## How to Run

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/KadirAkyaman/MinimalApi.BestPractices.git
    ```
2.  **Navigate to the project directory:**
    ```bash
    cd MinimalApi.BestPractices
    ```
3.  **Run the application:**
    ```bash
    dotnet run
    ```
    *(The `dotnet run` command will automatically restore any necessary dependencies.)*

4.  **Access the API documentation:**
    Open your browser and navigate to the URL provided in the console (e.g., `https://localhost:<port>/swagger`) to see the Swagger UI.
