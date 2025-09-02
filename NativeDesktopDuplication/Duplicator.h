#pragma once
#include <Windows.h>


#ifdef DESKTOPDUPLICATOR_EXPORTS
#define DDL_API extern "C" __declspec(dllexport)
#else
#define DDL_API extern "C" __declspec(dllimport)
#endif

#pragma comment(lib, "C:\\Program Files (x86)\\Windows Kits\\10\\Lib\\10.0.22621.0\\um\\x64\\d3d11.lib")
#pragma comment(lib, "C:\\Program Files (x86)\\Windows Kits\\10\\Lib\\10.0.22621.0\\um\\x64\\dxgi.lib")



// Inizializza duplicatore su un adapter/output specifico
DDL_API int InitializeDuplicator(int adapterIndex, int outputIndex);

// Cattura un frame (puntatore ai pixel + dimensioni)
// NB: il buffer rimane valido fino a ReleaseFrame()
DDL_API int GetFrame(unsigned char** data, int* width, int* height, int* pitch);

// Rilascia il frame corrente
DDL_API void ReleaseFrame();

// Chiude e libera le risorse
DDL_API void Shutdown();