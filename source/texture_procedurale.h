/*
 * MATLAB Compiler: 8.0 (R2020a)
 * Date: Tue Mar 23 19:18:37 2021
 * Arguments:
 * "-B""macro_default""-W""lib:texture_procedurale""-T""link:lib""-d""C:/Users/a
 * dri/dev/generation_procedurale/bin/Debug""-g""-G""-v""C:/Users/adri/dev/gener
 * ation_procedurale/source/matlab/entrainer_ia.m""C:/Users/adri/dev/generation_
 * procedurale/source/matlab/predire_ia.m"
 */

#ifndef texture_procedurale_h
#define texture_procedurale_h 1

#if defined(__cplusplus) && !defined(mclmcrrt_h) && defined(__linux__)
#  pragma implementation "mclmcrrt.h"
#endif
#include "mclmcrrt.h"
#ifdef __cplusplus
extern "C" { // sbcheck:ok:extern_c
#endif

/* This symbol is defined in shared libraries. Define it here
 * (to nothing) in case this isn't a shared library. 
 */
#ifndef LIB_texture_procedurale_C_API 
#define LIB_texture_procedurale_C_API /* No special import/export declaration */
#endif

/* GENERAL LIBRARY FUNCTIONS -- START */

extern LIB_texture_procedurale_C_API 
bool MW_CALL_CONV texture_proceduraleInitializeWithHandlers(
       mclOutputHandlerFcn error_handler, 
       mclOutputHandlerFcn print_handler);

extern LIB_texture_procedurale_C_API 
bool MW_CALL_CONV texture_proceduraleInitialize(void);

extern LIB_texture_procedurale_C_API 
void MW_CALL_CONV texture_proceduraleTerminate(void);

extern LIB_texture_procedurale_C_API 
void MW_CALL_CONV texture_proceduralePrintStackTrace(void);

/* GENERAL LIBRARY FUNCTIONS -- END */

/* C INTERFACE -- MLX WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- START */

extern LIB_texture_procedurale_C_API 
bool MW_CALL_CONV mlxEntrainer_ia(int nlhs, mxArray *plhs[], int nrhs, mxArray *prhs[]);

extern LIB_texture_procedurale_C_API 
bool MW_CALL_CONV mlxPredire_ia(int nlhs, mxArray *plhs[], int nrhs, mxArray *prhs[]);

/* C INTERFACE -- MLX WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- END */

/* C INTERFACE -- MLF WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- START */

extern LIB_texture_procedurale_C_API bool MW_CALL_CONV mlfEntrainer_ia(mxArray* url_ia, mxArray* url_dossier_tex_label, mxArray* url_dossier_tex_target, mxArray* callback, mxArray* continuer, mxArray* utiliser_gpu);

extern LIB_texture_procedurale_C_API bool MW_CALL_CONV mlfPredire_ia(mxArray* url_ia, mxArray* url_tex_predict_label, mxArray* url_tex_predict_target, mxArray* utiliser_gpu);

#ifdef __cplusplus
}
#endif
/* C INTERFACE -- MLF WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- END */

#endif
