# CSV Uploader & Viewer Application

### Results

→ A CSV file with **1,000,000 lines was processed in 10 seconds**.

### Backend

→ Built using Clean Architecture, DDD, and SOLID principles.

→ High-level layers are decoupled from low-level ones, following a strict separation of concerns across the architecture.

→ Large CSV file processing using a data streaming approach with channels to implement a producer-consumer model. Each consumer runs in a separate worker thread for parallelism across CPU cores (multithreading).

→ Usage bounded channels to limit in-memory queue growth and apply backpressure, preventing memory overuse.

→ The number of consumers matches the number of CPU cores, reducing thread context switching in CPU and memory leak.

→ Usage `SqlBulkCopy`, the most efficient way to perform high-volume inserts in .NET.

→ Usage Server-Sent Events (SSE) to send real-time job progress updates to the frontend.

→ Usage of `SqlParameter` to prevent SQL injection attacks.

→ Usage of `record struct` instead of `class` for DTOs to improve performance by using stack allocation instead of heap, reducing Garbage Collector pressure.

→ Implementation of exponential backoff retries for external API calls.

→ Usage of `Hangfire` to run jobs in the background, enabling asynchronous workflows and decoupling file persistence from processing. Includes custom activator to support dependency injection.

→ Usage of manual database transactions during CSV processing to reduce database overhead during high-volume writes.

→ Product search supports filtering by name, expiration date, sorting (by key and direction), and pagination.

→ The CSV processor follows the Single Responsibility Principle (SRP), focusing solely on the streaming infrastructure. Business rules are handled by the caller.

→ Usage of in-memory caching to track and expose job progress.

→ Uses Conditional Aggregation and Common Table Expressions (CTEs) to join product and exchange data by job ID. Joins are performed only on filtered products to keep queries fast and efficient.

→ Usage of `Serilog` for structured logging.

### Frontend

→ Usage of `zustand` for state management

→ Usage of `Material UI` for a consistent and responsive user interface.

→ Usage of a custom debounce hook to delay filtering logic when search input changes.

→ Defining application-wide type declarations in `.d.ts` files.