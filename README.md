# LittleDebugger.Tools.TemplateTransformer
Simple template transformer for translating substitution strings in files and paths.

Steps to install as a global tool:

dotnet build
dotnet pack
(to uninstall old version)
dotnet tool uninstall -g littledebugger.tools.templatetransformer
dotnet tool install --global --add-source ./nupkg littledebugger.tools.templatetransformer
