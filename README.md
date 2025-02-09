# Resistenza - A Powerful File Transfer Tool for Windows

**Resistenza** is a high-performance file transfer application designed for Windows, written in C# with .NET 8.0. The application boasts a modern and sleek **WPF** (Windows Presentation Foundation) user interface, ensuring an intuitive and responsive experience.

## Features

- **Fast and Reliable File Transfers**: Efficient file transfer with progress tracking and error handling.
- **Modern GUI**: Beautiful and responsive user interface built using WPF, offering a seamless experience.
- **Drag and Drop Support**: Easily transfer files by dragging and dropping them into the application window.
- **Cross-Network Support**: Transfers over both local and wide-area networks (LAN/WAN).
- **Transfer Queue Management**: Queue multiple transfers with the ability to pause, resume, or cancel them at any time.
- **Encryption Support**: Secure your file transfers with optional encryption (future feature).
- **Platform Compatibility**: Built for **Windows** with .NET 8.0, ensuring compatibility with modern Windows environments.

## Prerequisites

- **Windows 7 or higher**
- **.NET 8.0 runtime**: Make sure you have .NET 8.0 installed on your system to run the application.

## Installation

### Method 1: Download the Precompiled Executable

1. Visit the [Releases](https://github.com/vik24k/Resistenza/releases) section of this repository.
2. Download the latest release for Windows.
3. Extract the contents and run `Resistenza.exe`.

### Method 2: Build from Source

1. Clone this repository to your local machine:

    ```bash
    git clone https://github.com/vik24k/Resistenza.git
    ```

2. Open the project in Visual Studio.
3. Restore the NuGet packages:

    ```bash
    dotnet restore
    ```

4. Build the solution:

    ```bash
    dotnet build
    ```

5. Run the application:

    ```bash
    dotnet run
    ```

## Usage

1. Open **Resistenza** after installation.
2. Drag and drop files into the main window, or use the "Browse" button to select files for transfer.
3. Select the destination for the files.
4. Click the **Transfer** button to start the transfer process.
5. Monitor progress via the progress bars and transfer status indicators.

## Contributing

We welcome contributions to **Resistenza**! If you'd like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Make your changes and test them.
4. Submit a pull request to the `main` branch.

Please make sure to follow the coding conventions and include unit tests for any new features.


## Contact

If you have any questions or suggestions, feel free to open an issue on this repository or contact the project maintainer:

- **Author**: [Aguti Vittorio](https://github.com/yourusername)
- **Email**: [vittorio.aguti@mail.polimi.it](mailto:youremail@example.com)

---

Thank you for using **Resistenza**! We hope it makes your file transfer experience easier and more efficient.
