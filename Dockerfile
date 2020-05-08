FROM microsoft/dotnet:2.1-sdk AS build
COPY . RabbitMQ/
WORKDIR /RabbitMQ
run dotnet restore RabbitMQ.sln

FROM build AS publish
WORKDIR /RabbitMQ/
RUN dotnet publish RabbitMQ.sln -c Release -o /app

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
COPY . RabbitMQ/
WORKDIR /app
EXPOSE 8003

FROM runtime AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RabbitMQ.dll"]
