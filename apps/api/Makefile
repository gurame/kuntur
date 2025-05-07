## Main commands
build:
	dotnet build Kuntur.API.sln

watch:
	dotnet watch --project src/Kuntur.API.Host/Kuntur.API.Host.csproj run

run:
	dotnet run --project src/Kuntur.API.Host/Kuntur.API.Host.csproj

compose-up:
	find config -type f -exec chmod +x {} \;
	docker-compose -f docker-compose.yml up -d --build

compose-down:
	docker-compose -f docker-compose.yml down --remove-orphans

## Database commands
.PHONY: new-migration zero-migration run-migration run-all-migrations

# Lista de módulos válidos
VALID_MODULES := Identity Marketplace

# Función de shell para validar módulo
define check_module
case "$$module" in \
  Identity|Marketplace) ;; \
  *) echo "Error: invalid module '$$module'. Allowed: $(VALID_MODULES)"; exit 1;; \
esac;
endef

new-migration:
	@module=$(MODULE); \
	if [ -z "$$module" ]; then \
	  read -p "Enter module name: " module; \
	fi; \
	$(call check_module) \
	read -p "Enter migration name: " NAME; \
	echo "› Creatong migration '$$NAME' for module $$module"; \
	dotnet ef migrations add $$NAME \
	  --context Kuntur"$$module"DbContext \
	  --project src/Kuntur.API.$$module/Kuntur.API.$$module.csproj \
	  --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj \
	  -o Infrastructure/Persistence/Migrations; \
	echo "✓ Migration created successfully"

zero-migration:
	@module=$(MODULE); \
	if [ -z "$$module" ]; then \
	  read -p "Enter module name: " module; \
	fi; \
	$(call check_module) \
	read -p "Are you sure you want to delete the migrations of $$module? (y/n) " CONFIRM; \
	if [ "$$CONFIRM" != "y" ]; then \
	  echo "Abroting..."; exit 1; \
	fi; \
	MIG_PATH=Infrastructure/Persistence/Migrations; \
	if [ -d "src/Kuntur.API.$$module/$$MIG_PATH" ]; then \
	  echo "› Deleting existing migrations"; rm -rf "src/Kuntur.API.$$module/$$MIG_PATH"; \
	fi; \
	echo "› Creating zero migration 'Initial' for module $$module"; \
	dotnet ef migrations add Initial \
	  --context Kuntur"$$module"DbContext \
	  --project src/Kuntur.API.$$module/Kuntur.API.$$module.csproj \
	  --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj \
	  -o "$$MIG_PATH"; \
	echo "✓ Zero migration created successfully"; \

run-migration:
	@module=$(MODULE); \
	if [ -z "$$module" ]; then \
	  read -p "Enter module name: " module; \
	fi; \
	$(call check_module) \
	echo "› Applying migrations for module $$module"; \
	dotnet ef database update \
	  --context Kuntur"$$module"DbContext \
	  --project src/Kuntur.API.$$module/Kuntur.API.$$module.csproj \
	  --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj

run-all-migrations:
	@echo "› Applying migrations for all modules"; \
	for module in $(VALID_MODULES); do \
	  echo "  - Module: $$module"; \
	  dotnet ef database update \
	    --context Kuntur"$$module"DbContext \
	    --project src/Kuntur.API.$$module/Kuntur.API.$$module.csproj \
	    --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj; \
	done; \
	echo "✓ All migrations applied successfully"