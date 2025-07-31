# MinimalApi.BestPractices
# Technical Deep Dive: Core Concepts & Technologies

This project serves as a focused demonstration of the essential architectural components that form the backbone of a modern, professional ASP.NET Core API. Each feature is implemented with a focus on best practices, showcasing not just the "what," but the "why."

## 1. Request Pipeline & Endpoint Filters

The core of this project is a masterful control of the HTTP Request Pipeline. Instead of placing logic inside endpoint handlers, cross-cutting concerns are managed through a chain of Endpoint Filters (`IEndpointFilter`). This approach exemplifies the Single Responsibility and DRY principles at a micro-architectural level.

*   **Validation Filter:** A generic filter intercepts incoming DTOs, validates them using FluentValidation, and rejects invalid requests before they ever reach the business logic.
*   **Rate Limiting Logic:** The rate limiting policy is applied declaratively, ensuring security and quality of service concerns are handled cleanly and separately.

## 2. FluentValidation for Robust Data Integrity

Data integrity is guaranteed not with cluttered data annotations, but with `FluentValidation`.

*   **Decoupled Rules:** Validation rules are completely decoupled from the DTOs, living in their own dedicated, testable `Validator` classes.
*   **Complex Logic:** The implementation uses powerful features like complex Regex for password strength and username formats, demonstrating a deep understanding of data validation requirements.

## 3. Advanced Routing with Route Constraints

The API defends itself from malformed requests at the earliest possible stage: routing.

*   **Strict Contracts:** By using Route Constraints like `:alpha` and `:length`, the application ensures that only structurally valid URLs are ever mapped to an endpoint, preventing unnecessary processing and improving security.

## 4. API Versioning for Future-Proofing

The API is designed to evolve without breaking existing clients, using the industry-standard API Versioning (`Asp.Versioning`).

*   **URL-Based Strategy:** A clear and explicit URL-based versioning strategy (`/api/v1/...`) is implemented.
*   **Swagger Integration:** The versioning is seamlessly integrated with the API documentation, allowing users to switch between different versions of the API directly in the Swagger UI.

## 5. Professional API Documentation (The OpenAPI Ecosystem)

The project features a rich, interactive, and self-updating documentation powered by the OpenAPI (Swagger) ecosystem.

*   **SwaggerGen:** This core component automatically generates the `swagger.json` specification by reflecting over the API's endpoints, models, and metadata.
*   **Swagger UI:** Provides a clean, browser-based UI for developers to explore, understand, and test the API endpoints interactively.
*   **.WithOpenApi() for Minimal APIs:** Instead of relying on XML comments which can be problematic with Minimal APIs, documentation is applied directly to endpoints using the modern `.WithOpenApi()` method. This provides programmatic control and guarantees accuracy.

## 6. Security via Rate Limiting

To protect against abuse and ensure fair use, the API implements a robust Rate Limiting strategy.

*   **IP-Based Policies:** A Fixed Window rate limiting policy is defined and applied on a per-IP basis, effectively mitigating brute-force or denial-of-service attempts against critical endpoints like user creation.
*   **Declarative Application:** The policy is applied declaratively to the endpoint (`.RequireRateLimiting()`), keeping the endpoint handler clean and focused on its primary task.

## 7. Dependency Injection (DI)

The entire application is built on a foundation of Dependency Injection.

*   **Decoupled Services:** Services like `IValidator` are registered in the DI container and injected into components that need them (like the `ValidationFilter`). This makes the code modular, flexible, and highly testable.
## How to Run

1.  **Clone the repository:**
    ```bash
    git clone &lt;repository-url&gt;
    ```
2.  **Navigate to the project directory:**
    ```bash
    cd MinimalApi.BestPractices
    ```
3.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```
4.  **Run the application:**
    ```bash
    dotnet run
    ```
5.  **Access the API documentation:**
    Open your browser and navigate to `http://localhost:&lt;port&gt;/swagger` to see the Swagger UI.