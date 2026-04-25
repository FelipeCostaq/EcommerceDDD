FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Web_Ecommerce/Web_Ecommerce.csproj", "Web_Ecommerce/"]
COPY ["ApplicationApp/ApplicationApp.csproj", "ApplicationApp/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "Web_Ecommerce/Web_Ecommerce.csproj"

COPY . .
WORKDIR "/src/Web_Ecommerce"
RUN dotnet publish "Web_Ecommerce.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Web_Ecommerce.dll"]