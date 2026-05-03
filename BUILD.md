# Сборка и пересборка HuffmanCodec

Корень репозитория: `C:\KT` (в командах можно использовать `cd C:\KT`).

Запускай команды в **PowerShell** или **cmd**. В **Git Bash** часто нет `dotnet` в `PATH` (ошибка 127) — тогда полный путь, например  
`"C:\Program Files\dotnet\dotnet.exe" build ...`.

---

## Обычная сборка

```powershell
cd C:\KT
dotnet build HuffmanCodec.sln
```

Release:

```powershell
dotnet build HuffmanCodec.sln -c Release
```

---

## Чистая пересборка (с нуля)

```powershell
cd C:\KT
dotnet clean HuffmanCodec.sln -c Debug
dotnet clean HuffmanCodec.sln -c Release
dotnet build HuffmanCodec.sln -c Release --no-incremental
```

Только приложение (без тестов в отдельной команде — тесты всё равно пересоберутся при полном `sln`):

```powershell
dotnet build src\HuffmanCodec.App\HuffmanCodec.App.csproj -c Release --no-incremental
```

---

## Тесты

```powershell
cd C:\KT
dotnet test HuffmanCodec.sln -c Release
```

---

## Запуск без publish

```powershell
cd C:\KT
dotnet run -c Release --project src\HuffmanCodec.App\HuffmanCodec.App.csproj
```

Готовый exe после сборки Release:

`C:\KT\src\HuffmanCodec.App\bin\Release\net8.0-windows\HuffmanCodec.App.exe`

---

## Publish: один exe для Windows x64 (self-contained, single-file)

Удалить старый выход и собрать заново в `publish\win-x64`:

```powershell
cd C:\KT
Remove-Item -Recurse -Force .\publish -ErrorAction SilentlyContinue
dotnet publish src\HuffmanCodec.App\HuffmanCodec.App.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -o .\publish\win-x64
```

Запуск:

`C:\KT\publish\win-x64\HuffmanCodec.App.exe`

В **cmd** вместо обратных кавычек для переноса строки — одна строка:

```cmd
cd C:\KT
rmdir /s /q publish 2>nul
dotnet publish src\HuffmanCodec.App\HuffmanCodec.App.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish\win-x64
```

---

## Кратко

| Задача              | Команда |
|---------------------|---------|
| Сборка Release      | `dotnet build HuffmanCodec.sln -c Release` |
| Пересборка с чисткой | `dotnet clean …` затем `dotnet build … -c Release --no-incremental` |
| Тесты               | `dotnet test HuffmanCodec.sln -c Release` |
| Запуск из исходников | `dotnet run -c Release --project src\HuffmanCodec.App\HuffmanCodec.App.csproj` |
| Установочный single-file | `dotnet publish …` как выше, выход в `publish\win-x64` |
