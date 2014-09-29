@echo "Packing Simple Domain with refereces"
..\..\..\..\..\NugetFeed\nuget.exe pack .\RPS.Game.Domain.csproj -IncludeReferencedProjects
@echo "Copying to local feed"
xcopy *.nupkg ..\..\..\..\..\NugetFeed\