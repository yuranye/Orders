# Orders
Simple dummy orders service for showcase

#Protos
To get proto files run `git submodule update --init`

To update proto submodule to new commits run `git submodule update --remote --rebase`

This is one of important dependencies for http annotations
`<Protobuf Include="../../proto/google/api/*.proto" OutputDir="Protos" CompileOutputs="false" ProtoRoot="../../proto" />`

These are several examples of how to generate api clients properly
`<Protobuf Include="../../proto/clients/*.proto" OutputDir="Protos" CompileOutputs="false" ProtoRoot="../../proto" GrpcServices="Client" />`
`<Protobuf Include="../../proto/clients/v2/*.proto" OutputDir="Protos" CompileOutputs="false" ProtoRoot="../../proto" GrpcServices="Client" />`
`<Protobuf Include="../../proto/delivery/*.proto" OutputDir="Protos" CompileOutputs="false" ProtoRoot="../../proto" GrpcServices="Client" />`

Describes service which we implement in scope of this project
`<Protobuf Include="../../proto/orders/*.proto" OutputDir="Protos" CompileOutputs="false" ProtoRoot="../../proto" GrpcServices="Both" />`

If you need more clients/models/service feel free to add it with template described above

If project requires sharing of generated files you can create separate project (usually *.Grpc - Showcase.Grpc in my case) and generate required files there

If you need to generate only client add `GrpcServices="Client"`

After adding all required files hit build

For more information about dotnet grpc look here
https://docs.microsoft.com/en-US/aspnet/core/grpc/dotnet-grpc?view=aspnetcore-3.1