# .NETpad

**(C#/.NET 8/Windows Forms version with ARM64 Support)**

![.NETpad hero image](/graphics/hero.jpg)

.NETpad is a basic clone of Microsoft Notepad that was created by Paul Thurrott ([@thurrott](https://www.twitter.com/thurrott)) with the help of Rafael Rivera ([@withinrafael](https://www.twitter.com/withinrafael)), several outside contributors, and, thanks to his books, Charles Petzold. It is technologically unsophisticated and has many bugs. But .NETpad provides most Notepad features, albeit in U.S. English only, and adds a few additional features that Notepad lacks, including:

- Optional auto-save every 30 seconds
- Themes with the ability to arbitrarily choose a new background and text color
- Word count, displayed in the status bar

[I documented how I created a Visual Basic version of this app on Thurrott.com](https://www.thurrott.com/tag/the-winforms-notepad-project). And I am now updating this C# version of the app with some more modern features and bug fixes. 

This version of .NETpad should work on all supported versions of Windows 10 and Windows 11.

## User Settings

User-specific settings (like window size, location, font, colors, and other preferences) are stored in a JSON file named `usersettings.json`. You can find this file in your local application data folder, typically at:

- Windows: `%LOCALAPPDATA%\.NETpad\usersettings.json`

Deleting this file will reset .NETpad to its default settings on the next launch.

## .NET 8 and ARM64 Support

This version of .NETpad has been migrated to .NET 8 and includes native support for ARM64 versions of Windows (e.g., Windows 11 on ARM). This allows .NETpad to run without x64 emulation on ARM64 devices, providing better performance and efficiency.

### Building for ARM64

To build and publish .NETpad specifically for ARM64:

1.  **Using the Publish Profile (Recommended):**
    Open a command prompt or terminal in the project's root directory and run:
    ```bash
    dotnet publish /p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
    ```
    This will create a self-contained, single-file executable optimized for ARM64 in the `bin/Release/net8.0-windows/win-arm64/publish/` directory.

2.  **Manual `dotnet publish` command:**
    Alternatively, you can use the following command:
    ```bash
    dotnet publish NotePadWF-CS.csproj -c Release -r win-arm64 --self-contained true /p:PublishSingleFile=true /p:PublishReadyToRun=true
    ```
    This achieves a similar result.

### Building for x64

To build and publish for traditional x64 systems:
    ```bash
    dotnet publish NotePadWF-CS.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishReadyToRun=true
    ```
    (An x64 publish profile could also be created for convenience).

### Running on ARM64

After publishing, take the executable from the `publish` directory (e.g., `.NETpad.exe` from the `win-arm64/publish` folder) and run it on your ARM64 Windows device. It should run as a native ARM64 application.

### Known Limitations on ARM64

*   As of this update, no specific limitations have been identified when running the native ARM64 version compared to the x64 version. All features are expected to work. Any discovered issues will be documented here.
```
