# Orders
Simple dummy orders service for showcase

#Protos
To add new service to project run 
`dotnet grpc add-file -i Protos Protos/orders/*`

If you need to generate client fot another service use:
`dotnet grpc add-file -S=Client -i Protos Protos/[service_folder]/*`

-i attribute are required if you are using imports

After adding all required files hit build

https://docs.microsoft.com/ru-ru/aspnet/core/grpc/dotnet-grpc?view=aspnetcore-3.1
