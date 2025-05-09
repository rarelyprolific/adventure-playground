# Open API CHANGING A DOC FILE AS A TEST

## Guiding principals

Should provide a consistent and uniform interface for interactions between clients and server.
Uses standard HTTP methods (GET, PUT, POST, DELETE. etc.) and URIs to identify resources.

The client and server should have a separation of concerns. The user interface is a responsibility of the client but the server is responsible for data storage. We must guarantee the interface/contract between the client and server is never broken.

Each request from the client must contain all the information necessary for the server to understand and complete the request. The server will not retain any state. Any state required must be implemented on the client.

A response from the server should identify itself as cacheable or non-cacheable. The client can re-use cacheable response data later for an equivalent request during a specified period.

Should be composed of hierarchical layers where each component can only see within the scope of the immediate layer they are interacting with.

## Guidance

HTTP and REST are not the same. In REST, data and functionality should be considered to be resources. HTTP is typically the server protocol used but REST does not mandate it.

Metadata is used to indicate caching, detect transmission errors and negotiate formats as well as perform authentication and access control.

## Caching

**GET** requests should be cacheable by default. Browsers typically cache most GET requests.
**POST** requests are not cacheable by default but can be made cacheable via `Expires` or `Cache-Control` headers with a directive to explicitly allow caching.
**PUT** and **DELETE** are not cacheable.

The `Expires` header should be used to specify the absolute expiry time for a cached representation. Include a time up to a year in the future to indicate the representation never expires.

Also see `Cache-Control`, `ETag` and `Left-Modified` headers.

## Compression (HTTP-specific)

Consider using `Accept-Encoding` with `compress` and `gzip`
If the server cannot send an acceptable response it should return **406 (Not Acceptable)**.
If the server supports a compression algorithm, it can compress the representation and set the encoding scheme using `Content-Encoding`.
If the service cannot accept the content encoding in a request it should return **415 (Unsupported Media Type)**.

## Content Negotiation using HTTP Headers

An incoming request should have a `Content-Type` header such as `application/json` to determine what the content is in the request. The client can always specify an `Accept` header to determine the content it prefers to receive. If no Accept header is present, the server will send a pre-configured representation type.

## Idempotency

Idempotent HTTP methods can be invoked multiple times without different results. This allows caches and CDNs to store and return the results of idempotent requests. Only `POST` and `PATCH` are non-idempotent.

## Versioning

APIs only need an increase in the major version number when a breaking change is made such as:

- A change in the format of response data from existing endpoints.
- A change to any existing request or response type.
- Removing any part of the API.

It can be helpful to track minor version changes to indicate changes to customers who have cached older versions.

**URI Versioning** - `http://api.myhost.com/v1/things` is the most simple approach. It does force that clients must actively call the intended version when an API is upgraded although this could be viewed as a positive.

## Documentation

Good documentation should include example requests and responses.

## How to design an API

- Identify resources, sub-resources, the actions which are applicable to them and if they are cacheable.
- An API should be intuitive and easy to use.
- Use kebab-case nouns to represent resources.
- An API may not comply with all rules for a RESTful API but it should be consistent within itself.
- Consider custom methods with colon (:) for actions which cannot be easily expressed using standard HTTP methods. e.g. `apihost/publishers/books:archive`
- HTTP dates are always in GMT.
- Long running operations should have an endpoint to initiate the task via a POST request, a status endpoint a client can GET from to check progress and an optional resource endpoint to GET a completed asset from.

## Example API endpoints

`/customers` is a collection resource
`/customers/{id}` is a singleton resource
`/customers/{id}/accounts` is a sub-collection resource
