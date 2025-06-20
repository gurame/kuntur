.PHONY: clean-up up build watch run compose-up compose-down compose-delete new-migration zero-migration run-migration run-all-migrations zero-migration-all

## Quick commands
# for a clean up, delete containers and volumes, then up and run
clean-up:
	make compose-delete && make up

up:
	make compose-up && make run-all-migrations && make run

# for a clean start, delete containers and volumes, then build and up
clean-start:
	make compose-delete && make start

start:
	make compose-all && make run-all-migrations

## Main commands
build:
	@echo "› Building solution"
	dotnet build Kuntur.Backend.sln

restore:
	@echo "› Restoring solution"
	dotnet restore Kuntur.Backend.sln

watch:
	@echo "› Watching solution"
	dotnet watch --project src/Kuntur.API.Host/Kuntur.API.Host.csproj run

run:
	@echo "› Running solution"
	dotnet run --project src/Kuntur.API.Host/Kuntur.API.Host.csproj

compose-all:
	@echo "› Building and starting docker containers"
	find config -type f -exec chmod +x {} \;
	docker-compose -f docker-compose.yml up --build -d

compose-up:
	@echo "› Start docker containers"
	find config -type f -exec chmod +x {} \;
	docker-compose -f docker-compose.yml up postgres keycloak jaeger -d

compose-down: 
	@echo "› Stopping and removing docker containers"
	docker-compose -f docker-compose.yml down --remove-orphans

compose-delete:
	@echo "› Deleting docker containers and volumes"
	docker-compose -f docker-compose.yml down --remove-orphans --volumes
	
## Database commands
VALID_MODULES := Marketplace

define check_module
case "$$module" in \
  Marketplace) ;; \
  *) echo "Error: invalid module '$$module'. Allowed: $(VALID_MODULES)"; exit 1;; \
esac;
endef

ifeq ($(NO_BUILD),true)
  NO_BUILD_FLAG := --no-build
else
  NO_BUILD_FLAG :=
endif

new-migration:
	@module=$(MODULE); \
	if [ -z "$$module" ]; then \
	  read -p "Enter module name: " module; \
	fi; \
	$(call check_module) \
	read -p "Enter migration name: " NAME; \
	echo "› Creating migration '$$NAME' for module $$module"; \
	dotnet ef migrations add $$NAME \
	  --context "$$module"DbContext \
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
	  --context "$$module"DbContext \
	  --project src/Kuntur.API.$$module/Kuntur.API.$$module.csproj \
	  --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj \
	  -o $$MIG_PATH; \
	echo "✓ Zero migration created successfully";

run-migration:
	@module=$(MODULE); \
	if [ -z "$$module" ]; then \
	  read -p "Enter module name: " module; \
	fi; \
	$(call check_module) \
	echo "› Applying migrations for module $$module"; \
	dotnet ef database update \
	  --context "$$module"DbContext \
	  --project src/Kuntur.API.$$module/Kuntur.API.$$module.csproj \
	  --startup-project src/Kuntur.API.Host/Kuntur.API.Host.csproj \
	  $(NO_BUILD_FLAG); \
	echo "✓ Migrations applied successfully";

run-all-migrations:
	make build
	@for module in $(VALID_MODULES); do \
	  make run-migration MODULE=$$module NO_BUILD=true; \
	done

zero-migration-all:
	@for module in $(VALID_MODULES); do \
	  make zero-migration MODULE=$$module; \
	done