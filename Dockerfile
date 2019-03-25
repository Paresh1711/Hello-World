FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["HellofreshTest/HellofreshTest.csproj", "HellofreshTest/"]
RUN dotnet restore "HellofreshTest/HellofreshTest.csproj"
COPY . .
WORKDIR "/src/HellofreshTest"
RUN dotnet build "HellofreshTest.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HellofreshTest.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HellofreshTest.dll"]

FROM alpine:edge

RUN apk add --no-cache mongodb

VOLUME /data/db
EXPOSE 27017 28017

COPY run.sh /root
ENTRYPOINT [ "/root/run.sh" ]
CMD [ "mongod", "--bind_ip", "127.0.0.1" ]