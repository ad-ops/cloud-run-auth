FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /app
COPY . .
RUN ["dotnet",  "publish", "--configuration", "Release"]

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=builder /app/bin/Release/netcoreapp3.1/publish /app
CMD [ "dotnet", "./cloud-run-auth.dll" ]