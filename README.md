# Solidcube.AspNetCore.ProxyHeader

This project get a specific *header *(default to "**X-Forwarded-Path**") from the request and set the value to `basePath`.

I have created this middleware for using OData Api behind a proxy for rewrite the `@OData` metadata tag with correct Url Path.

## Installation

```bash
dotnet add package Solidcube.AspNetCore.ProxyHeader
```

## Usage

With the code below use the standard header `X-Forwarded-Path`

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //[..]

    app.UseForwardedHeaderPath();
}
```

If you want to specify a custom header

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //[..]

    app.UseForwardedHeaderPath(new ProxyHelper.Models.ProxyPathOptions { XForwardedHeaderPath = "X-My-Custom-Header" });
}
```

## Result

| Url                      | Header Value | Result                           |
| ------------------------ | ------------ | -------------------------------- |
| http://localhost/api/foo | /commons     | http://localhost/commons/api/foo |
