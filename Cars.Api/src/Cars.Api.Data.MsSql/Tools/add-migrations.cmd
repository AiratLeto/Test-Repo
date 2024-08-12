// ef commands templates for the Rider's terminal
// замените ${Name} на название вашей миграции

cd src/Cars.Api.Data.PostgreSql
dotnet restore
dotnet ef -h
dotnet ef migrations add ${Name} --verbose --project ../../src/Cars.Api.Data.PostgreSql --startup-project ../../src/Cars.Api.Data.Migrator
dotnet ef database update --verbose --project ../../src/Cars.Api.Data.PostgreSql --startup-project ../../src/Cars.Api.Data.Migrator
