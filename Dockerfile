FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["FinancialProject.sln", "./"]
COPY ["src/FinancialProject.Domain/FinancialProject.Domain.csproj", "src/FinancialProject.Domain/"]
COPY ["src/FinancialProject.Application/FinancialProject.Application.csproj", "src/FinancialProject.Application/"]
COPY ["src/FinancialProject.Infrastructure/FinancialProject.Infrastructure.csproj", "src/FinancialProject.Infrastructure/"]
COPY ["src/FinancialProject.WebApi/FinancialProject.WebApi.csproj", "src/FinancialProject.WebApi/"]

RUN dotnet restore "FinancialProject.sln"

COPY . .
RUN dotnet publish "src/FinancialProject.WebApi/FinancialProject.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV DOTNET_EnableDiagnostics=0

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "FinancialProject.WebApi.dll"]
