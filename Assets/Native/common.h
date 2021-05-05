//workaround: uncomment this based on export as DLL or native. Reason: __declspec is needed for dll and gives errors as native.
//#define LIBRARY_EXPORTS = true;

#ifdef LIBRARY_EXPORTS
#    define LIBRARY_API extern "C" __declspec(dllexport)
#else
#    define LIBRARY_API extern "C" 
#endif