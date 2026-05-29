## action-review-tool

An internal tool for organizations that want to track and review updates in their ecosystem, built on blazor server app.

**Snapshots**

![alt text](image.png)

![alt text](image-1.png)

**Download**
```bash 
git clone https://github.com/falconeri-8/action-review-tool.git 
&& 
cd action-review-tool
```
```bash 
dotnet tool install --global dotnet-ef
dotnet restore
dotnet run --urls "https://localhost:5099"
```
<br>

- .NET SDK 10.0 or newer
- EFC (SQL Lite)