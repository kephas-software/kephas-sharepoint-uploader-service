# Kephas Integration Services for Microsoft SharePoint Online
The application automate the business flow of the digitalized documents using SharePoint Online, helping users concentrate on their actual work. 

# System requirements
The application runs on the following operating systems:

* Windows
  * Windows 7 SP1 or newer, Windows Server 2008 R2 SP1 or newer.
  * .NET Framework 4.6.1 or newer -or- .NET Core 3.1 or newer.

* Linux
  * Ubuntu 16 or newer.
  * Mono 6.4.0 or newer -or- .NET Core 3.1 or newer.

# Installation instructions
  * Unzip the application host package into a folder of your choice.
  * Make sure the user starting the application has read/write permissions in the folder where the application is installed.
    * Important: so far, the application is only a host providing the infrastructure for the actual functionality, which will be installed as plugins in the next steps.
  * In the `Config` sub directory of the installation folder identify the `NuGet.config` file and open it with a text editor. Please change the values of the `UserName` and `ClearTextPassword` with the one provided by the software publisher. Save the file and close it.
  
# Starting the application in setup mode
  * Run the `kis-cli.exe` CLI application (command line interface) and follow the instructions on the screen.
    * On Linux run `mono kis-cli.exe` for the .NET Framework 4.6.1 targeting application, or `dotnet kis-cli.exe` for the .NET Core 3.1 alternative.
  * To terminate the CLI application issue the `quit` command in the console.
  * Note: the setup mode does not try to connect to SharePoint, Exchange, or do anything with the documents in the file system. It is used to configure the connection and other settings. When the configuration is done, exit the application command and start it again in service mode.

# Starting the application in service mode
  * Run the `kis.exe` command without any further arguments and follow the instructions on the screen.
    * Note: the service mode requires a proper application configuration, otherwise the connection to SharePoint, Exchange, or to the file system may malfunction.
  * To terminate the application issue the `quit` command in the application console.
  
# Starting the application as a Windows service
  * Start a command prompt as administrator.
  * Execute the `sc` command to create the Windows service:
  `sc create sharepointdocuploader binPath= "\"<path-to-the-exe-file>\" service" start= auto DisplayName= "SharePoint Document Uploader"`
    * Make sure you use the exact whitespace as above, as `sc` is very sensistive about it.
  * Start the service:
  `sc start sharepointdocuploader`
  * For more information about starting, stopping, creating, and deleting Windows services check https://support.microsoft.com/en-us/help/251192/how-to-create-a-windows-service-by-using-sc-exe.

# Application documentation

* General considerations
  * [Application configuration](../../wiki/Application-configuration)
  * [Logging](../../wiki/Logging)
  * [Field value expressions](../../wiki/Field-value-expressions)
* Document sources
  * [Microsoft Exchange](../../wiki/Microsoft-Exchange-source)
  * [File System](../../wiki/File-system-source)
* Commands
  * [Text encryption](../../wiki/Text-encryption)
  * [Retrying failed uploads](../../wiki/Retrying-failed-uploads)
  * [Inspecting SharePoint user permissions](../../wiki/Inspecting-permissions)
