// Generated by Hybridizer version 1.2.10616
 #include "cuda_runtime.h"                                                                                  
 #include "device_launch_parameters.h"                                                                      
                                                                                                              
 #if defined(__CUDACC__)                                                                                      
 	#ifndef hyb_device                                                                                       
 		#define hyb_inline __forceinline__                                                                   
 		                                                                                                     
 		#define hyb_constant __constant__                                                                    
 		#if defined(HYBRIDIZER_NO_HOST)                                                                      
 			#define hyb_host                                                                                 
 			#define	hyb_device  __device__                                                                   
 		#else                                                                                                
 			#define hyb_host __host__                                                                        
 			#define	hyb_device  __device__                                                                   
 		#endif                                                                                               
 	#endif                                                                                                   
 #else                                                                                                        
 	#ifndef hyb_device                                                                                       
 		#define hyb_inline inline                                                                            
 		#define hyb_device                                                                                   
 		#define hyb_constant                                                                                 
 	#endif                                                                                                   
 #endif                                                                                                       
                                                                                                              
                                                                                                  
 #if defined _WIN32 || defined _WIN64 || defined __CYGWIN__                                                   
   #define BUILDING_DLL                                                                                       
   #ifdef BUILDING_DLL                                                                                        
     #ifdef __GNUC__                                                                                          
       #define DLL_PUBLIC __attribute__ ((dllexport))                                                         
     #else                                                                                                    
       #define DLL_PUBLIC __declspec(dllexport) // Note: actually gcc seems to also supports this syntax.     
     #endif                                                                                                   
   #else                                                                                                      
     #ifdef __GNUC__                                                                                          
       #define DLL_PUBLIC __attribute__ ((dllimport))                                                         
     #else                                                                                                    
       #define DLL_PUBLIC __declspec(dllimport) // Note: actually gcc seems to also supports this syntax.     
     #endif                                                                                                   
   #endif                                                                                                     
   #define DLL_LOCAL                                                                                          
 #else                                                                                                        
   #if __GNUC__ >= 4                                                                                          
     #define DLL_PUBLIC __attribute__ ((visibility ("default")))                                            
     #define DLL_LOCAL  __attribute__ ((visibility ("hidden")))                                             
   #else                                                                                                      
     #define DLL_PUBLIC                                                                                       
     #define DLL_LOCAL                                                                                        
   #endif                                                                                                     
 #endif                                                                                                       


#if CUDART_VERSION >= 9000
#include <cooperative_groups.h>
#endif
// hybridizer core types
#include <cstdint>
namespace hybridizer { struct hybridobject ; }
namespace hybridizer { struct runtime ; }

#pragma region defined enums and types
#if defined(__cplusplus) || defined(__CUDACC__)
struct RefInt ;
namespace HelloWorld { 
struct EntryPoints___c__DisplayClass1_0 ;
} // Leaving namespace
namespace HelloWorld { 
struct EntryPoints ;
} // Leaving namespace
namespace HelloWorld { 
struct EntryPoints___c__DisplayClass0_0 ;
} // Leaving namespace
namespace System { namespace Threading { namespace Tasks { 
struct Parallel ;
} } } // Leaving namespace
// Intrinsic type Nullable`1 used
#define __TYPE_DECL_hybridizer_nullable__int64_t____
namespace System { namespace Threading { namespace Tasks { 
struct ParallelLoopResult ;
} } } // Leaving namespace
// Intrinsic type Action`1 used
#define __TYPE_DECL_hybridizer_action__int____
#endif
#pragma endregion

extern "C" void* __hybridizer_init_basic_runtime();
#include <cstdio>
// generating GetTypeID function
#include <cstring> // for strcmp
extern "C" DLL_PUBLIC int HybridizerGetTypeID( const char* fullTypeName)
{
	if (strcmp (fullTypeName, "HelloWorld.EntryPoints") == 0) return 1000000 ; 
	if (strcmp (fullTypeName, "HelloWorld.EntryPoints+<>c__DisplayClass0_0") == 0) return 1000001 ; 
	if (strcmp (fullTypeName, "HelloWorld.EntryPoints+<>c__DisplayClass1_0") == 0) return 1000002 ; 
	if (strcmp (fullTypeName, "RefInt") == 0) return 1000003 ; 
	if (strcmp (fullTypeName, "System.Action<System.Int32>") == 0) return 1000004 ; 
	if (strcmp (fullTypeName, "System.Nullable<System.Int64>") == 0) return 1000005 ; 
	if (strcmp (fullTypeName, "System.Object") == 0) return 1000006 ; 
	if (strcmp (fullTypeName, "System.Threading.Tasks.Parallel") == 0) return 1000007 ; 
	if (strcmp (fullTypeName, "System.Threading.Tasks.ParallelLoopResult") == 0) return 1000008 ; 
	return 0 ;
}
extern "C" DLL_PUBLIC const char* HybridizerGetTypeFromID( const int typeId)
{
	if (typeId == 1000000) return "HelloWorld.EntryPoints" ; 
	if (typeId == 1000001) return "HelloWorld.EntryPoints+<>c__DisplayClass0_0" ; 
	if (typeId == 1000002) return "HelloWorld.EntryPoints+<>c__DisplayClass1_0" ; 
	if (typeId == 1000003) return "RefInt" ; 
	if (typeId == 1000004) return "System.Action<System.Int32>" ; 
	if (typeId == 1000005) return "System.Nullable<System.Int64>" ; 
	if (typeId == 1000006) return "System.Object" ; 
	if (typeId == 1000007) return "System.Threading.Tasks.Parallel" ; 
	if (typeId == 1000008) return "System.Threading.Tasks.ParallelLoopResult" ; 
	return "" ;
}
extern "C" DLL_PUBLIC int HybridizerGetShallowSize (const char* fullTypeName) 
{
	#ifdef __TYPE_DECL__HelloWorld_EntryPoints___
	if (strcmp (fullTypeName, "HelloWorld.EntryPoints") == 0) return 8 ; 
	#endif
	#ifdef __TYPE_DECL__HelloWorld_EntryPoints___c__DisplayClass0_0__
	if (strcmp (fullTypeName, "HelloWorld.EntryPoints+<>c__DisplayClass0_0") == 0) return 32 ; 
	#endif
	#ifdef __TYPE_DECL__HelloWorld_EntryPoints___c__DisplayClass1_0__
	if (strcmp (fullTypeName, "HelloWorld.EntryPoints+<>c__DisplayClass1_0") == 0) return 16 ; 
	#endif
	#ifdef __TYPE_DECL__RefInt___
	if (strcmp (fullTypeName, "RefInt") == 0) return 16 ; 
	#endif
	#ifdef __TYPE_DECL_hybridizer_action__T____
	if (strcmp (fullTypeName, "System.Action<System.Int32>") == 0) return 24 ; 
	#endif
	#ifdef __TYPE_DECL_hybridizer_nullable__T____
	if (strcmp (fullTypeName, "System.Nullable<System.Int64>") == 0) return 16 ; 
	#endif
	#ifdef __TYPE_DECL_hybridizer_hybridobject___
	if (strcmp (fullTypeName, "System.Object") == 0) return 8 ; 
	#endif
	#ifdef __TYPE_DECL__System_Threading_Tasks_ParallelLoopResult__
	if (strcmp (fullTypeName, "System.Threading.Tasks.ParallelLoopResult") == 0) return 24 ; 
	#endif
	return 0 ;
}

// Get various Hybridizer properties at runtime
struct __hybridizer_properties {
    int32_t UseHybridArrays;
    int32_t Flavor;
    int32_t CompatibilityMode;
    int32_t DelegateSupport;
    int32_t _dummy;
};
extern "C" DLL_PUBLIC __hybridizer_properties __HybridizerGetProperties () {
    __hybridizer_properties res;
    res.UseHybridArrays = 0;
    res.Flavor = 1;
    res.DelegateSupport = 0;
    res.CompatibilityMode = 0;
    return res ;
}
#include <cuda.h>                                     
 struct HybridModule                                  
 {                                                    
     void* module_data ;                              
     CUmodule module ;                                
 } ;                                                  
                                                      
 extern char __hybridizer_cubin_module_data [] ;      
 static HybridModule __hybridizer__gs_module = { 0 }; 
 static int __hybridizer_initialized = 0; 


// error code translation from CUresult to cudaError_t

namespace hybridizer {

	cudaError_t translateCUresult(int cures)
	{
		switch (cures)

		{
			case CUDA_SUCCESS: return cudaSuccess ;
			case CUDA_ERROR_INVALID_VALUE: return cudaErrorInvalidValue ;
			case CUDA_ERROR_LAUNCH_FAILED: return cudaErrorLaunchFailure ;
			case CUDA_ERROR_NOT_SUPPORTED: return cudaErrorNotSupported ;
			case CUDA_ERROR_ILLEGAL_INSTRUCTION : return cudaErrorLaunchFailure ;
			default: return cudaErrorUnknown ;
		}
	}

} // namespace hybridizer
#pragma region Wrappers definitions


extern "C" DLL_PUBLIC int run_OccupancyCalculator_MaxActiveBlocksPerSM(int* numBlocks, int blockSize, int sharedMemSize) 
{
    if (0 == __hybridizer_initialized)
    {
        cudaDeviceSynchronize();
        __hybridizer_initialized = 1;
    }
    CUresult cures;
    if (__hybridizer__gs_module.module_data == 0)
    {
        cures = cuModuleLoadData(&(__hybridizer__gs_module.module), __hybridizer_cubin_module_data);
        __hybridizer__gs_module.module_data = (void*)__hybridizer_cubin_module_data;
        if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures);
    }
    
    CUfunction __hybridizer__cufunc;
    
    cures = cuModuleGetFunction(&__hybridizer__cufunc, __hybridizer__gs_module.module, "run");
    if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures);
    
    return cuOccupancyMaxActiveBlocksPerMultiprocessor(numBlocks, __hybridizer__cufunc, blockSize, sharedMemSize);
}

extern "C" DLL_PUBLIC int run_ExternCWrapper_CUDA( int gridDim_x,  int gridDim_y,  int blockDim_x,  int blockDim_y,  int blockDim_z,  int shared,  int N,  RefInt** const acuda,  RefInt** const adotnet)
{
	if (0 == __hybridizer_initialized) {                                                            
		cudaDeviceSynchronize();                                                                       
		__hybridizer_initialized = 1;                                                                  
	}                                                                                               
	CUresult cures ;                                                                                 
	if (__hybridizer__gs_module.module_data == 0)                                                    
	{                                                                                              
		cures = cuModuleLoadData (&(__hybridizer__gs_module.module), __hybridizer_cubin_module_data) ; 
		__hybridizer__gs_module.module_data = (void*) __hybridizer_cubin_module_data ;                 
		if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                  
	}                                                                                              
	                                                                                                 
	CUfunction __hybridizer__cufunc ;                                                                
	                                                                                                 
	cures = cuModuleGetFunction (&__hybridizer__cufunc, __hybridizer__gs_module.module, "run") ;   
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                    
	                                                                                                 
	hybridizer::runtime* __hybridizer_runtime = (hybridizer::runtime*) __hybridizer_init_basic_runtime(); 



	void* __hybridizer_launch_config[5] = 
		{
			(void*)&__hybridizer_runtime,
			(void*)&N,
			(void*)&acuda,
			(void*)&adotnet,
			(void*)0
		} ;

	shared += 16 ; if (shared > 48*1024) shared = 48*1024 ;                                                                                                
	                                                                                                                                                       
	cures = cuLaunchKernel (__hybridizer__cufunc, gridDim_x, gridDim_y, 1, blockDim_x, blockDim_y, blockDim_z, shared, 0, __hybridizer_launch_config, 0) ; 
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ; 
	int cudaLaunchRes = (int)::cudaPeekAtLastError ();         
	if (cudaLaunchRes != 0) return cudaLaunchRes;            
	int __synchronizeRes = (int)::cudaDeviceSynchronize () ; 
	return __synchronizeRes ;                                

}

extern "C" DLL_PUBLIC int run_ExternCWrapperStream_CUDA( int gridDim_x,  int gridDim_y,  int blockDim_x,  int blockDim_y,  int blockDim_z,  int shared,  cudaStream_t st,  int N,  RefInt** const acuda,  RefInt** const adotnet)
{
	if (0 == __hybridizer_initialized) {                                                            
		cudaDeviceSynchronize();                                                                       
		__hybridizer_initialized = 1;                                                                  
	}                                                                                               
	CUresult cures ;                                                                                 
	if (__hybridizer__gs_module.module_data == 0)                                                    
	{                                                                                              
		cures = cuModuleLoadData (&(__hybridizer__gs_module.module), __hybridizer_cubin_module_data) ; 
		__hybridizer__gs_module.module_data = (void*) __hybridizer_cubin_module_data ;                 
		if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                  
	}                                                                                              
	                                                                                                 
	CUfunction __hybridizer__cufunc ;                                                                
	                                                                                                 
	cures = cuModuleGetFunction (&__hybridizer__cufunc, __hybridizer__gs_module.module, "run") ;   
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                    
	                                                                                                 
	hybridizer::runtime* __hybridizer_runtime = (hybridizer::runtime*) __hybridizer_init_basic_runtime(); 



	void* __hybridizer_launch_config[6] = 
		{
			(void*)&__hybridizer_runtime,
			(void*)&N,
			(void*)&acuda,
			(void*)&adotnet,
			(void*)0
		} ;

	shared += 16 ; if (shared > 48*1024) shared = 48*1024 ;                                                                                                
	                                                                                                                                                       
	cures = cuLaunchKernel (__hybridizer__cufunc, gridDim_x, gridDim_y, 1, blockDim_x, blockDim_y, blockDim_z, shared, st, __hybridizer_launch_config, 0) ; 
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ; 
	int cudaLaunchRes = (int)::cudaPeekAtLastError ();         
	if (cudaLaunchRes != 0) return cudaLaunchRes;            
	return cudaLaunchRes; 

}

#if CUDART_VERSION >= 9000
extern "C" DLL_PUBLIC int run_ExternCWrapperGridSync_CUDA( int gridDim_x,  int gridDim_y,  int blockDim_x,  int blockDim_y,  int blockDim_z,  int shared,  int N,  RefInt** const acuda,  RefInt** const adotnet)
{
	if (0 == __hybridizer_initialized) {                                                            
		cudaDeviceSynchronize();                                                                       
		__hybridizer_initialized = 1;                                                                  
	}                                                                                               
	CUresult cures ;                                                                                 
	if (__hybridizer__gs_module.module_data == 0)                                                    
	{                                                                                              
		cures = cuModuleLoadData (&(__hybridizer__gs_module.module), __hybridizer_cubin_module_data) ; 
		__hybridizer__gs_module.module_data = (void*) __hybridizer_cubin_module_data ;                 
		if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                  
	}                                                                                              
	                                                                                                 
	CUfunction __hybridizer__cufunc ;                                                                
	                                                                                                 
	cures = cuModuleGetFunction (&__hybridizer__cufunc, __hybridizer__gs_module.module, "run") ;   
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                    
	                                                                                                 
	hybridizer::runtime* __hybridizer_runtime = (hybridizer::runtime*) __hybridizer_init_basic_runtime(); 



	void* __hybridizer_launch_config[5] = 
		{
			(void*)&__hybridizer_runtime,
			(void*)&N,
			(void*)&acuda,
			(void*)&adotnet,
			(void*)0
		} ;

	shared += 16 ; if (shared > 48*1024) shared = 48*1024 ;                                                                                                
	                                                                                                                                                       
	cures = cuLaunchCooperativeKernel (__hybridizer__cufunc, gridDim_x, gridDim_y, 1, blockDim_x, blockDim_y, blockDim_z, shared, 0, __hybridizer_launch_config) ; 
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ; 
	int cudaLaunchRes = (int)::cudaPeekAtLastError ();         
	if (cudaLaunchRes != 0) return cudaLaunchRes;            
	int __synchronizeRes = (int)::cudaDeviceSynchronize () ; 
	return __synchronizeRes ;                                

}
#endif

#if CUDART_VERSION >= 9000
extern "C" DLL_PUBLIC int run_ExternCWrapperStreamGridSync_CUDA( int gridDim_x,  int gridDim_y,  int blockDim_x,  int blockDim_y,  int blockDim_z,  int shared,  cudaStream_t st,  int N,  RefInt** const acuda,  RefInt** const adotnet)
{
	if (0 == __hybridizer_initialized) {                                                            
		cudaDeviceSynchronize();                                                                       
		__hybridizer_initialized = 1;                                                                  
	}                                                                                               
	CUresult cures ;                                                                                 
	if (__hybridizer__gs_module.module_data == 0)                                                    
	{                                                                                              
		cures = cuModuleLoadData (&(__hybridizer__gs_module.module), __hybridizer_cubin_module_data) ; 
		__hybridizer__gs_module.module_data = (void*) __hybridizer_cubin_module_data ;                 
		if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                  
	}                                                                                              
	                                                                                                 
	CUfunction __hybridizer__cufunc ;                                                                
	                                                                                                 
	cures = cuModuleGetFunction (&__hybridizer__cufunc, __hybridizer__gs_module.module, "run") ;   
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ;                    
	                                                                                                 
	hybridizer::runtime* __hybridizer_runtime = (hybridizer::runtime*) __hybridizer_init_basic_runtime(); 



	void* __hybridizer_launch_config[6] = 
		{
			(void*)&__hybridizer_runtime,
			(void*)&N,
			(void*)&acuda,
			(void*)&adotnet,
			(void*)0
		} ;

	shared += 16 ; if (shared > 48*1024) shared = 48*1024 ;                                                                                                
	                                                                                                                                                       
	cures = cuLaunchCooperativeKernel (__hybridizer__cufunc, gridDim_x, gridDim_y, 1, blockDim_x, blockDim_y, blockDim_z, shared, st, __hybridizer_launch_config) ; 
	if (cures != CUDA_SUCCESS) return hybridizer::translateCUresult((int)cures) ; 
	int cudaLaunchRes = (int)::cudaPeekAtLastError ();         
	if (cudaLaunchRes != 0) return cudaLaunchRes;            
	return cudaLaunchRes; 

}
#endif

#pragma endregion
