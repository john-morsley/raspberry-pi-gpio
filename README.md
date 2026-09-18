Raspberry Pi GPIO
=================

SSH into your Raspberry Pi and run the following commands to install .NET 7.0 SDK:

```
john-morsley@raspberry-pi-5-a
```

Commands to install .NET 7.0 SDK on Raspberry Pi

```
sudo apt update 
sudo apt install -y 
sudo apt install -y curl ca-certificates tar
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh 
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10.0 --install-dir $HOME/dotnet --architecture arm64
export PATH=$HOME/dotnet:$PATH
```

To verify the installation, run the following command:
```
dotnet --version
dotnet --list-sdks
```

Git Workflow
------------

1. Commit files to GitHub.
2. On the Pi, run the following commands to clone the repository and build the project:
```
git clone https://github.com/john-morsley/raspberry-pi-gpio.git
cd raspberry-pi-gpio
dotnet build
```