# adventure-playground

Just a sandpit repository for playing around with github features and actions!

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
