## Mermaid Diagramming

Flowcharts can be:

- TB - Top to bottom
- TD - Top-down/ same as top to bottom
- BT - Bottom to top
- RL - Right to left
- LR - Left to right

```mermaid
flowchart LR
    subgraph frontend["Front-end (public)"]
        public-website(["The Website"])
        public-webapi("The API")
    end

    %% Setup the backend subgraph and contents
    subgraph backend["Back-end (private)"]
        public-website-->|"get some data"|private-webapi
        worker-one
        worker-two(("Worker Two"))
    end
    subgraph data["Data (very private)"]
        private-webapi-->database-one[(Database One)]
        worker-one-->database-one
        public-webapi-.->database-two[(Database Two)]
        worker-two-->|"process staged data"|database-three[(Database Three)]
        public-webapi-->database-one
    end
```

Sequence diagram:

```mermaid
sequenceDiagram
    autonumber

    actor User as Barry
    participant Website
    participant API
    participant Database

    %% Service initialisation
    critical Ensure database is available at service start
        API-->Database: Attempt initial connection
    end

    %% User query
    User-->Website: Search for text

    Note right of Website: Validate and trim input string

    Website->>API: Make search request
    API-->>Database: Submit search query parameters

    Note over Database: Run query via stored procedure

    Database-->>API: Return first 50 search results
    API->>Website: Return results as JSON

    %% Database failure
    alt Database connection failed
        Database->>API: Return SQL error message!
    end

    %% Cache refresh
    loop Every few seconds
        Website-->API: Refresh search results cache!
    end
```

Mindmap:

```mermaid
mindmap
    [The problem to solve!]

        Reasonable ideas
            Slow and steady progress
            Being methodical
                Figure out steps
                Complete steps in order of priority

            Organised tasks

        Out there ideas
            Crazy theory
                (Prototyping like a crazy person!)

            Total madness
                )Hack on it(
                ))Hack loads((

            Move fast and break stuff
```
