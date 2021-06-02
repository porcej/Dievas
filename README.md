# Dievas - backend
Dievas - backend is a C# server for dashboarding.


## Requirements
- .NET Core version 2.2 or later with command line tools.  You can use `$ dotnet --version` to check the installedc version.
- [Newtonsoft.json](https://www.newtonsoft.com/json) 

## Installation
Clone this respository and use the package manager [NuGet](https://nuget.org) to install the requirements.

```bash
$ git clone [https://github.com/porcej/Dievas.git](https://github.com/porcej/Dievas.git)
$ cd Dievas
$ nuget install Newtonsoft.json
```

## Usage
The server can be run from [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) or can be packages in an IIS instance.
```bash
dotnet run

```


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)