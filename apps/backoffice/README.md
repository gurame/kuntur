# Blazor

## Hosting Models

- Blazor Server
- Blazor WebAssembly
- Blazor Hybrid

### Blazor Server

(Server) Every user interaction is processed through a signalR connection
SEO Friendly

-----

Requires constant connection
No support for offline modes oe PWAs

### Blazor WebAssembly

(User Browser) Leveraging WebAssembly to run .net code in client side.
The standout feature hre is the ability to run offline, making it ideal for PWAs or applications that need to function in low conecntiviy environment

Run PWAs
Not dependent on the server

-----

Large footprint

------------------------------------------------------------------------

Static Server-side Rendering
Streaming Server-side Rendering
Per component (Decide if SignalR, WebAssembly or SSR options)
Automode 

-------------------------------------------------------------------------

## Templates

- Blazor Web App
- Blazor WebAssembly Standalone App
- .NET MAUI Blazor Hybrid App

### Blazor Web App

#### Interactive reder mode

- None : non-intercative website (only access to SSR modes)
- Server : Blazor Server (signalR things running)
- WebAssembly : Running WebAssembly but using ASP.NET Backend (Pre-render, etc)
- Auto: First spinning up SignalR, Then WASM

#### Interactive localtion

- Per page/component
- Global

Blazor United?

##### Render Mode

- Server
- Wasm
- Auto
- Server Prerendered
- Wasm Prerendered
- Auto Prerendered
- SSR

#### Dependency Injection

Per circuit (socket connection)

----------------------------------------------------------------

dotnet new blazor -n Kuntur.Backoffice -int Server --no-https

#### Components

- Microsoft.AspNetCore.Components.QuickGrid

- ValueExpression?