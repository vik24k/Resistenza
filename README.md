# Resistenza - Modern .NET 7 Remote Access Tool

**Resistenza** is a high-performance Remote Access Tool (RAT) written from scratch in **.NET 7.0** with a **WinForms-based GUI** for the server. It uses **TLS encryption** for secure client-server communication and is designed for modern Windows environments. Developed and tested in **Visual Studio 2022**.

---

## Features

- **Secure TLS Communication:** All client-server data is encrypted using TLS 1.2.
- **Modern GUI:** Server application built with WinForms for a responsive management interface.
- **High Performance:** Lightweight design ensures fast operations with minimal overhead.
- **Client Authentication:** Server validates incoming clients before accepting connections.
- **Packet-Based Protocol:** Custom serialization and deserialization of packets for flexible communication.
- **Remote File & Command Operations:** Supports remote file access and execution of commands on connected clients.

![Alt text](/Preview/server_main.png)

https://github.com/user-attachments/assets/a1eda0c5-4bff-4b93-9726-760300d2e36c


---

## Project Structure

- **Resistenza.Server:** Server application with WinForms GUI to manage clients.  
- **Resistenza.Client:** Lightweight client connecting securely to the server.  
- **Resistenza.Common:** Shared library for networking, packet serialization, and utility classes.

---

## Requirements

- **.NET 7 SDK**  
- **Visual Studio 2022** or newer  
- Windows 7 (or newer)

---

## Security

- Uses **self-signed TLS certificates** to secure communication.  
- Client authentication ensures only authorized clients can connect.  
- All packets are serialized and sent over encrypted channels for maximum security.

---

## License

This project uses a **custom license**:  

- You may use the software freely.  
- **Modification or redistribution is not allowed.**  
- **The original author must be credited.**

---

## Disclaimer

This project is for educational purposes only. Unauthorized deployment may violate local laws. Ensure you have proper authorization before using it in any environment.

---

## Contact

For questions or support, contact the author:  
**Email:** vittorio.aguti@mail.polimi.it

---

## Getting Started

1. **Open the solution** in **Visual Studio 2022**.
2. **Build the solution** to restore NuGet packages and compile all projects.
3. **Run the Server project** to start listening for clients.
4. **Deploy the Client executable** to target machines and connect to the server.
