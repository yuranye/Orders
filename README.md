# Orders
Simple dummy orders service for showcase

#Protos
To get proto files run `git submodule update --init`

To update proto submodule to new commits run `git submodule update --remote --rebase`

To add new service to project run
`<ItemGroup>`
    `<Protobuf Include="../../proto/[service_name]/*.proto" ProtoRoot="../../proto" />`
`</ItemGroup>` 

Additional imports can be added `AdditionalImportDirs="[additiona_imports_path]"` 

If you need to generate only client add `GrpcServices="Client"`

After adding all required files hit build

For more information about dotnet grpc look here
https://docs.microsoft.com/en-US/aspnet/core/grpc/dotnet-grpc?view=aspnetcore-3.1