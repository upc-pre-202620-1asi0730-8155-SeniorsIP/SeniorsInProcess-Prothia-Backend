FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY Prothia.API/*.csproj Prothia.API/
RUN dotnet restore ./Prothia.API
COPY . .
RUN dotnet publish ./Prothia.API -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80
ENTRYPOINT ["dotnet", "Prothia.API.dll"]
