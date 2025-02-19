FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY src/QuizApp.sln . 
COPY src/QuizApp.API/QuizApp.API.csproj QuizApp.API/
COPY src/QuizApp.Application/QuizApp.Application.csproj QuizApp.Application/
COPY src/QuizApp.Contracts.Rest/QuizApp.Contracts.Rest.csproj QuizApp.Contracts.Rest/
COPY src/QuizApp.DataContext/QuizApp.DataContext.csproj QuizApp.DataContext/
COPY src/QuizApp.Domain/QuizApp.Domain.csproj QuizApp.Domain/
COPY src/QuizApp.Infrastructure/QuizApp.Infrastructure.csproj QuizApp.Infrastructure/

RUN dotnet restore "QuizApp.sln"

COPY src/ ./ 

WORKDIR /src/QuizApp.API
RUN dotnet build "QuizApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/QuizApp.API
RUN dotnet publish "QuizApp.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizApp.API.dll"]