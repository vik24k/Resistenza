# Resistenza - Modern .NET 7 Remote Access Tool

**Resistenza** is a high-performance Remote Access Tool (RAT) written from scratch in **.NET 7.0** with a **WinForms-based GUI** for the server. It leverages **TLS encryption** for secure communication and is designed for modern Windows environments. This project was developed using **Visual Studio 2022**.

---

## Features

- **Secure Communication:** Uses TLS for encrypted data transfer between client and server.
- **Modern GUI:** Server interface built with WinForms, providing a responsive and clear management experience.
- **Lightweight and Fast:** Minimal overhead while maintaining secure communication.
- **Client Authentication:** Server validates connecting clients before accepting data.
- **Packet-Based Protocol:** Custom serialization/deserialization of packets for flexible client-server communication.
- **File and Remote Operations:** Supports remote file access and execution of commands from the server.

---

## Project Structure

- **Resistenza.Server:** Server-side WinForms application to manage connected clients.  
- **Resistenza.Client:** Lightweight client application that connects securely to the server.  
- **Resistenza.Common:** Shared library containing network communication, packet serialization, and utility classes.

---

## Requirements

- **.NET 7 SDK**  
- **Visual Studio 2022** or newer  
- Windows 10 / 11 (or newer)

---

## Getting Started

1. Clone the repository:  
   ```bash
   git clone https://github.com/yourusername/resistenza.git
