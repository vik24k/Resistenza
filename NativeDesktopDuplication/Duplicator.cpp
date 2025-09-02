#include "Duplicator.h"
#include "pch.h"
#include <d3d11.h>
#include <dxgi1_2.h>
#include <wrl/client.h>


using Microsoft::WRL::ComPtr;

static ComPtr<ID3D11Device> g_device;
static ComPtr<ID3D11DeviceContext> g_context;
static ComPtr<IDXGIOutputDuplication> g_duplication;
static ComPtr<ID3D11Texture2D> g_stagingTex;

// Inizializza duplicatore
int InitializeDuplicator(int adapterIndex, int outputIndex)
{
    HRESULT hr;

    // Crea device Direct3D11
    D3D_FEATURE_LEVEL featureLevel;
    hr = D3D11CreateDevice(
        nullptr,
        D3D_DRIVER_TYPE_HARDWARE,
        nullptr,
        D3D11_CREATE_DEVICE_BGRA_SUPPORT,
        nullptr, 0,
        D3D11_SDK_VERSION,
        &g_device,
        &featureLevel,
        &g_context
    );
    if (FAILED(hr)) return hr;

    // Ottieni factory e adapter
    ComPtr<IDXGIDevice> dxgiDevice;
    g_device.As(&dxgiDevice);

    ComPtr<IDXGIAdapter> adapter;
    if (adapterIndex >= 0)
    {
        ComPtr<IDXGIFactory1> factory;
        dxgiDevice->GetAdapter(&adapter);
        adapter->GetParent(__uuidof(IDXGIFactory1), &factory);
        factory->EnumAdapters(adapterIndex, &adapter);
    }
    else
    {
        dxgiDevice->GetAdapter(&adapter);
    }

    // Seleziona output
    ComPtr<IDXGIOutput> output;
    hr = adapter->EnumOutputs(outputIndex, &output);
    if (FAILED(hr)) return hr;

    ComPtr<IDXGIOutput1> output1;
    output.As(&output1);

    // Duplica output
    hr = output1->DuplicateOutput(g_device.Get(), &g_duplication);
    return hr;
}

// Cattura frame
int GetFrame(unsigned char** data, int* width, int* height, int* pitch)
{
    if (!g_duplication) return E_FAIL;

    DXGI_OUTDUPL_FRAME_INFO frameInfo;
    ComPtr<IDXGIResource> desktopResource;

    HRESULT hr = g_duplication->AcquireNextFrame(500, &frameInfo, &desktopResource);
    if (FAILED(hr)) return hr;

    ComPtr<ID3D11Texture2D> tex;
    desktopResource.As(&tex);

    D3D11_TEXTURE2D_DESC desc;
    tex->GetDesc(&desc);

    *width = desc.Width;
    *height = desc.Height;

    // Crea staging texture se non esiste
    if (!g_stagingTex)
    {
        D3D11_TEXTURE2D_DESC stagingDesc = desc;
        stagingDesc.Usage = D3D11_USAGE_STAGING;
        stagingDesc.BindFlags = 0;
        stagingDesc.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
        stagingDesc.MiscFlags = 0;

        hr = g_device->CreateTexture2D(&stagingDesc, nullptr, &g_stagingTex);
        if (FAILED(hr)) return hr;
    }

    // Copia frame
    g_context->CopyResource(g_stagingTex.Get(), tex.Get());

    D3D11_MAPPED_SUBRESOURCE mapped;
    hr = g_context->Map(g_stagingTex.Get(), 0, D3D11_MAP_READ, 0, &mapped);
    if (FAILED(hr)) return hr;

    *data = reinterpret_cast<unsigned char*>(mapped.pData);
    *pitch = mapped.RowPitch;

    return S_OK;
}

// Rilascia frame
void ReleaseFrame()
{
    if (g_duplication)
    {
        g_context->Unmap(g_stagingTex.Get(), 0);
        g_duplication->ReleaseFrame();
    }
}

// Cleanup
void Shutdown()
{
    g_stagingTex.Reset();
    g_duplication.Reset();
    g_context.Reset();
    g_device.Reset();
}
