rm -rf ./pub-linux
rm -rf ./pub-windows
rm -rf ./pub-mac
dotnet publish ./src/FHOS/FHOS.vbproj -o ./pub-linux -c Release --sc -p:PublishSingleFile=true -r linux-x64
dotnet publish ./src/FHOS/FHOS.vbproj -o ./pub-windows -c Release --sc -p:PublishSingleFile=true -r win-x64
dotnet publish ./src/FHOS/FHOS.vbproj -o ./pub-mac -c Release --sc -p:PublishSingleFile=true -r osx-x64
butler push pub-windows thegrumpygamedev/freedom-haters-of-splorr:windows
butler push pub-linux thegrumpygamedev/freedom-haters-of-splorr:linux
butler push pub-mac thegrumpygamedev/freedom-haters-of-splorr:mac
git add -A
git commit -m "shipped it!"