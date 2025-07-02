FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Kuntur.API.Cart/Kuntur.API.Cart.csproj", "Kuntur.API.Cart/"]
COPY ["src/Kuntur.API.Catalog/Kuntur.API.Catalog.csproj", "Kuntur.API.Catalog/"]
COPY ["src/Kuntur.API.Shared/Kuntur.API.Shared.csproj", "Kuntur.API.Shared/"]
COPY ["src/Kuntur.API.Host/Kuntur.API.Host.csproj", "Kuntur.API.Host/"]
COPY ["src/Kuntur.API.Identity/Kuntur.API.Identity.csproj", "Kuntur.API.Identity/"]
COPY ["src/Kuntur.API.Identity.Contracts/Kuntur.API.Identity.Contracts.csproj", "Kuntur.API.Identity.Contracts/"]
COPY ["src/Kuntur.API.Inventory/Kuntur.API.Inventory.csproj", "Kuntur.API.Inventory/"]
COPY ["src/Kuntur.API.Logistics/Kuntur.API.Logistics.csproj", "Kuntur.API.Logistics/"]
COPY ["src/Kuntur.API.Marketplace/Kuntur.API.Marketplace.csproj", "Kuntur.API.Marketplace/"]
COPY ["src/Kuntur.API.Marketplace.Contracts/Kuntur.API.Marketplace.Contracts.csproj", "Kuntur.API.Marketplace.Contracts/"]
COPY ["src/Kuntur.API.Onboarding/Kuntur.API.Onboarding.csproj", "Kuntur.API.Onboarding/"]
COPY ["src/Kuntur.API.Orders/Kuntur.API.Orders.csproj", "Kuntur.API.Orders/"]
COPY ["src/Kuntur.API.Payments/Kuntur.API.Payments.csproj", "Kuntur.API.Payments/"]
COPY ["src/Kuntur.SharedKernel/Kuntur.SharedKernel.csproj", "Kuntur.SharedKernel/"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]
RUN dotnet restore "Kuntur.API.Host/Kuntur.API.Host.csproj"
COPY src/ ./
WORKDIR "/src/Kuntur.API.Host"
RUN dotnet publish "Kuntur.API.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .

# Change user to run the application, (geocs means don't interactively prompt for various info about the user)
RUN adduser --disabled-password --gecos '' appuser
USER appuser

ENTRYPOINT ["dotnet", "Kuntur.API.Host.dll"]