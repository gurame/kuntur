## Main commands
build:
	dotnet build Kuntur.API.sln

run:
	dotnet run --project src/Kuntur.API.Host/Kuntur.API.Host.csproj

compose-up:
	find config -type f -exec chmod +x {} \;
	docker-compose -f docker-compose.yml up -d --build

compose-down:
	docker-compose -f docker-compose.yml down --remove-orphans

## Database commands
new-identity-migration: 
	@read -p "Enter migration name: " name; \
	echo "Creating migration with name: $$name"; \
	dotnet ef migrations add $$name --project src/Kuntur.API.Identity/Kuntur.API.Identity.csproj --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj -o Infrastructure/Persistence/Migrations
	@echo "Migration created successfully"

zero-identity-migration:
	@read -p "Are you sure you want to delete the existing migrations? (y/n) " confirm; \
	if [ "$$confirm" != "y" ]; then \
		echo "Aborting..."; \
		exit 1; \
	fi
	@if [ -d "src/Kuntur.API.Identity/Infrastructure/Persistence/Migrations" ]; then \
		echo "Deleting existing Migrations"; \
		rm -rf src/Kuntur.API.Identity/Infrastructure/Persistence/Migrations; \
	fi
	@echo "Creating zero migration with name: Initial"; \
	dotnet ef migrations add Initial --project src/Kuntur.API.Identity/Kuntur.API.Identity.csproj --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj -o Infrastructure/Persistence/Migrations
	@echo "Zero migration created successfully"

run-identity-migration:
	dotnet ef database update --project src/Kuntur.API.Identity/Kuntur.API.Identity.csproj --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj