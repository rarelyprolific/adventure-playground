# Open API Contract Testing

Validates the API implementation in `SimpleWebApi` from `src` running on `http://localhost:5183` matches the
Open API specification in `simple-web-api.yaml` using Specmatic via Docker:

```shell
docker run -v "%cd%/simple-web-api.yaml:/simple-web-api.yaml" znsio/specmatic test "/simple-web-api.yaml" --testBaseURL=http://host.docker.internal:5183
```

If the API implementation outputs a `swagger.json` then Specmatic will verify if the API exposes endpoints which
are `missing in spec`:

```
Could not load report configuration, coverage will be calculated but no coverage threshold will be enforced

|--------------------------------------------------------------------------------|
| SPECMATIC API COVERAGE SUMMARY                                                 |
|--------------------------------------------------------------------------------|
| coverage | path             | method | response | #exercised | result          |
|----------|------------------|--------|----------|------------|-----------------|
| 100%     | /WeatherForecast | GET    | 200      | 1          | covered         |
| 0%       | /DatabaseAccess  | GET    | 0        | 0          | missing in spec |
|--------------------------------------------------------------------------------|
| 50% API Coverage reported from 2 Paths                                         |
|--------------------------------------------------------------------------------|

1 path found in the app is not documented in the spec.
```
