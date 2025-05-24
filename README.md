# ChainResource Project

This project implements a resource caching system using a **chain of responsibility** design. Each resource is resolved by checking a series of storage nodes (e.g., memory, file), moving down the chain if needed. When a valid value is found, it's optionally written back to previous nodes.

The **node** itself is responsible for traversing the chain when necessary — if its own stored value is missing or expired, it delegates to the next node.

A static `ChainResourceHolder` class holds two instances of `ChainResourceManager` — one configured according to the task's requirements, and another with shorter expiration times for testing purposes (used in `Program.cs`).

Debug print statements were added to show the flow between nodes, making it easier to understand how the value is retrieved and propagated.

The `API_ID` for OpenExchangeRates is loaded from an `.env` file using the `DotNetEnv` package. The variables are initialized via `Env.Load()` inside the `ChainResourceManager` constructor.  
Note: the `.env` file is not included in the repository and should be created manually in the project’s root folder.

