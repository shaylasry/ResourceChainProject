# ChainResource Project

This project implements a resource caching system using a **chain of responsibility** design. Each chain is composed of nodes, where each node holds a single storage layer (e.g., memory, file, web). The node does not store the value itself — instead, it delegates reading and writing to its internal `IStorage` implementation.

Each node is responsible for deciding whether to use its own storage or move forward in the chain. If the current storage returns no value (or an expired one), the node traverses to the next node in the chain. When a valid value is eventually found, it is written back to earlier nodes that support writing.

A static `ChainResourceHolder` class holds two instances of `ChainResourceManager` — one configured according to the task's requirements, and another with shorter expiration times for testing purposes (used in `Program.cs`).

Debug print statements were added to show the flow between nodes, making it easier to understand how the value is retrieved and propagated.

The `API_ID` for OpenExchangeRates is loaded from an `.env` file using the `DotNetEnv` package. The variables are initialized via `Env.Load()` inside the `ChainResourceManager` constructor.  
Note: the `.env` file is not included in the repository and should be created manually in the project’s root folder.

